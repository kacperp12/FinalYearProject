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
#endregion

public class BarcodeGen : BaseNetLogic
{
    TextBox BarcodeTextbox;
    Image ImageForBarcode;
    Label BarcodeText;
    static string filePath = "";

    [ExportMethod]
    public void GenerateBarcode(NodeId TextboxId, NodeId ImageId, NodeId LabelId) {
        BarcodeTextbox = InformationModel.Get<TextBox>(TextboxId);
        ImageForBarcode = InformationModel.Get<Image>(ImageId);
        BarcodeText = InformationModel.Get<Label>(LabelId);

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

        string Data = BarcodeTextbox.Text;
        filePath = $"C:\\Users\\Kacper\\OneDrive - National University of Ireland, Galway\\4th Year College\\Final Year Project\\Project Files\\FYP\\ProjectFiles\\NetSolution\\{result}.svg";

        if(File.Exists(filePath))
            File.Delete(filePath);

        var barcodeWriter = new BarcodeWriterSvg
        {
            Format = BarcodeFormat.CODE_128,
            Options = new EncodingOptions
            {
                Width = 300,
                Height = 150
            }
        };

        var bt = barcodeWriter.Write(Data);
        File.WriteAllText(filePath, bt.ToString());
        
        ImageForBarcode.Path = filePath;
        BarcodeText.Text = BarcodeTextbox.Text;
    }
}
