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
#endregion

public class BarcodeGen : BaseNetLogic
{
    TextBox BarcodeTextbox;
    Image ImageForBarcode;
    Label BarcodeText, ErrorLabelText;
    Button SubmitButton;
    static string filePath = "";

    [ExportMethod]
    public void GenerateBarcode(NodeId TextboxId, NodeId ImageId, NodeId LabelId, NodeId Label2Id) {
        BarcodeTextbox = InformationModel.Get<TextBox>(TextboxId);
        ImageForBarcode = InformationModel.Get<Image>(ImageId);
        BarcodeText = InformationModel.Get<Label>(LabelId);
        ErrorLabelText = InformationModel.Get<Label>(Label2Id);

        ErrorLabelText.Text = "";

        //Ensures each svg file is deleted once it is redundant
        //Prevents a build up of svg files
        if(!filePath.Equals(""))
            if(File.Exists(filePath))
                File.Delete(filePath);

        //CREATE ERROR TO BE DISPLAYED HERE AT SOME POINT!!!
        if(BarcodeTextbox.Text.Equals(""))
            return;

        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var result = new string(
            Enumerable.Repeat(chars, 6)
                      .Select(s => s[random.Next(s.Length)])
                      .ToArray());

        var projectRelativeResourceUri = ResourceUri.FromProjectRelativePath($"{result}.svg");

        string Data = BarcodeTextbox.Text;

        if(File.Exists(filePath))
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
        
        ImageForBarcode.Path = projectRelativeResourceUri;
        BarcodeText.Text = BarcodeTextbox.Text;
    }

    [ExportMethod]
    public void InputChecker(NodeId TextboxId, NodeId Label2Id, NodeId SubmitButtonId) {
        BarcodeTextbox = InformationModel.Get<TextBox>(TextboxId);
        ErrorLabelText = InformationModel.Get<Label>(Label2Id);
        SubmitButton = InformationModel.Get<Button>(SubmitButtonId);

        if(BarcodeTextbox.Text.Length > 15) {
            BarcodeTextbox.BorderColor = new Color(255, 255, 0, 0);
            ErrorLabelText.TextColor = new Color(255, 255, 0, 0);
            ErrorLabelText.Text = "Please check your input! (15 chars max)";
            SubmitButton.Enabled = false;
        }
        else {
            BarcodeTextbox.BorderColor = new Color(255, 0, 255, 0);
            ErrorLabelText.Text = "";
            SubmitButton.Enabled = true;
        }
    }
}
