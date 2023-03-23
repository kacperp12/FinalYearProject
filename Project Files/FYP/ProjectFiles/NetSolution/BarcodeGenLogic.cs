#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.Retentivity;
using FTOptix.NativeUI;
using FTOptix.UI;
using FTOptix.WebUI;
using FTOptix.NetLogic;
using FTOptix.CoreBase;
using FTOptix.Core;
using ZXing.Common;
using ZXing;
using System.IO;
using System.Linq;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.ODBCStore;
using System.Xml;
using FTOptix.Recipe;
using FTOptix.Datalogger;
using FTOptix.EventLogger;
using System.Threading;
#endregion

public class BarcodeGenLogic : BaseNetLogic
{
    TextBox Textbox;
    Image BarcodeImg;
    Label BarcodeText, ErrorLabelText;
    static string filePath = "";
    private Timer debounceTimer;
    private object debounceLock = new object();

    [ExportMethod]
    public void GenerateBarcode(NodeId TextboxId, NodeId ImageId, NodeId LabelId, NodeId Label2Id) {
        Textbox = InformationModel.Get<TextBox>(TextboxId);
        BarcodeImg = InformationModel.Get<Image>(ImageId);
        BarcodeText = InformationModel.Get<Label>(LabelId);
        ErrorLabelText = InformationModel.Get<Label>(Label2Id);

        ErrorLabelText.Text = "";

        //Ensures each svg file is deleted once it is redundant
        //Prevents a build up of svg files
        if(!filePath.Equals(""))
            if(File.Exists(filePath))
                File.Delete(filePath);

        //CREATE ERROR TO BE DISPLAYED HERE AT SOME POINT!!!
        if(Textbox.Text.Equals(""))
            return;

        // Cancel previous debounce timer
        lock (debounceLock)
        {
            debounceTimer?.Dispose();
            debounceTimer = null;
        }

        // Start new debounce timer
        debounceTimer = new Timer(state =>
        {
            // Call GenerateBarcodeInternal with textbox text as argument
            GenerateBarcodeInternal(Textbox.Text, Label2Id);
        }, null, 750, Timeout.Infinite);

        BarcodeText.Text = Textbox.Text;
    }

    private void GenerateBarcodeInternal(string text, NodeId barcodeText)
    {
        if (string.IsNullOrEmpty(text) || text.Length > 15)
            return;

        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var result = new string(
            Enumerable.Repeat(chars, 6)
                      .Select(s => s[random.Next(s.Length)])
                      .ToArray());

        var projectRelativeResourceUri = ResourceUri.FromProjectRelativePath($"{result}.svg");

        string Data = text;

        if (File.Exists(filePath))
            File.Delete(filePath);

        var barcodeWriter = new BarcodeWriterSvg
        {
            Format = BarcodeFormat.CODE_128,
            Options = new EncodingOptions
            {
                Width = 300,
                Height = 150,
                PureBarcode = true
            }
        };

        var bt = barcodeWriter.Write(Data);
        File.WriteAllText(projectRelativeResourceUri.Uri, bt.ToString());

        // Move to separate function
        XmlDocument svgDoc = new XmlDocument();
        svgDoc.Load(projectRelativeResourceUri.Uri);

        foreach (XmlNode childNode in svgDoc.ChildNodes)
        {
            if (childNode.Name == "svg")
            {
                if(childNode.Attributes != null) {
                    XmlAttribute fillAttribute = childNode.Attributes["style"];
                    fillAttribute.Value = "background-color:rgb(192,232,251);background-color:rgba(192, 232, 251, 1)";
                    svgDoc.Save(projectRelativeResourceUri.Uri);
                    break;
                }
            }
        }

        svgDoc.Save(projectRelativeResourceUri.Uri);
        
        BarcodeImg.Path = projectRelativeResourceUri;
        }

    [ExportMethod]
    public void InputChecker(NodeId TextboxId, NodeId Label2Id) {
        Textbox = InformationModel.Get<TextBox>(TextboxId);
        ErrorLabelText = InformationModel.Get<Label>(Label2Id);

        if(Textbox.Text.Length > 15) {
            Textbox.BorderColor = new Color(255, 255, 0, 0);
            ErrorLabelText.TextColor = new Color(255, 255, 0, 0);
            ErrorLabelText.Text = "Please check your input! (15 chars max)";
        }
        else {
            Textbox.BorderColor = new Color(255, 0, 255, 0);
            ErrorLabelText.Text = "";
        }
    }
}
