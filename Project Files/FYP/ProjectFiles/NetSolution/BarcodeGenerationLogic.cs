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
using ZXing.Rendering;
using System.Text.RegularExpressions;
#endregion

public class BarcodeGenerationLogic : BaseNetLogic
{
    private TextBox Textbox;

    private Image BarcodeImg;

    private Label BarcodeText, ErrorLabelText;


    // A timer for debouncing text input
    private Timer debounceTimer;

    // A lock object for thread safety
    private object debounceLock = new object();

    [ExportMethod]
    public void GenerateBarcode(NodeId TextboxId, NodeId ImageId, NodeId LabelId, NodeId Label2Id)
    {
        // Get the relevant UI controls
        Textbox = InformationModel.Get<TextBox>(TextboxId);
        BarcodeImg = InformationModel.Get<Image>(ImageId);
        BarcodeText = InformationModel.Get<Label>(LabelId);
        ErrorLabelText = InformationModel.Get<Label>(Label2Id);

        // Cancel previous debounce timer and start new one
        lock (debounceLock)
        {
            debounceTimer?.Dispose();
            debounceTimer = null;
        }
        debounceTimer = new Timer(state =>
        {
            // Call GenerateBarcodeInternal with textbox text as argument
            GenerateBarcodeInternal(Textbox.Text);
        }, null, 750, Timeout.Infinite);

        // Update the label control with the text from the textbox control
        BarcodeText.Text = Textbox.Text;
    }

    private void GenerateBarcodeInternal(string text)
    {
        // Check if the text input is valid
        if (string.IsNullOrEmpty(text) || text.Length > 15 || !Regex.IsMatch(text, "^[ -~]*$"))
            return;

        // Generate a random string of characters
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var result = new string(
            Enumerable.Repeat(chars, 6)
                      .Select(s => s[random.Next(s.Length)])
                      .ToArray());

        // Create a project-relative resource URI for the SVG file
        var projectRelativeResourceUri = ResourceUri.FromProjectRelativePath($"{result}.svg");

        // Generate a barcode image in SVG format using the text input
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
        var bt = barcodeWriter.Write(text);

        // Write the SVG image to a file
        File.WriteAllText(projectRelativeResourceUri.Uri, bt.ToString());

        // Modify the SVG image
        modifySVG(projectRelativeResourceUri);

        // Update the image control with the new SVG file path
        BarcodeImg.Path = projectRelativeResourceUri;
    }

    /*
    This method modifies the SVG file. The generated barcode has a white background with no option to modify this 
    within the ZXing library. This method converts the white background into the same background colour as my application.
    */
    private void modifySVG(ResourceUri projectRel)
    {
        // Load the SVG file into an XML document
        XmlDocument svgDoc = new XmlDocument();
        svgDoc.Load(projectRel.Uri);

        // Modify the style attribute of the SVG node to add the correct background colour
        foreach (XmlNode childNode in svgDoc.SelectNodes("//svg"))
        {
            if (childNode.Attributes != null)
            {
                XmlAttribute fillAttribute = childNode.Attributes["style"];
                fillAttribute.Value = "background-color:rgb(192,232,251);background-color:rgba(192, 232, 251, 1)";
                break;
            }
        }

        // Save the modified SVG document to the file
        svgDoc.Save(projectRel.Uri);
    }

    [ExportMethod]
    public void InputChecker(NodeId TextboxId, NodeId Label2Id)
    {
        // Get the relevant UI controls
        Textbox = InformationModel.Get<TextBox>(TextboxId);
        ErrorLabelText = InformationModel.Get<Label>(Label2Id);

        // Check the length of the text input
        if (Textbox.Text.Length > 15)
        {
            // If the text input is too long, set the text box border color and error label text color to red and display an error message
            Textbox.BorderColor = new Color(255, 255, 0, 0);
            ErrorLabelText.TextColor = new Color(255, 255, 0, 0);
            ErrorLabelText.Text = "Please check your input! (15 chars max)";
        }
        else if (Textbox.Text.Length == 0)
        {
            // If the text input is empty, set the text box border color and error label text color to red and display an error message
            Textbox.BorderColor = new Color(255, 255, 0, 0);
            ErrorLabelText.TextColor = new Color(255, 255, 0, 0);
            ErrorLabelText.Text = "Input needs to be greater than 0!";
        }
        else if (!Regex.IsMatch(Textbox.Text, "^[ -~]*$"))
        {
            // If the text input contains non-ASCII characters, set the text box border color and error label text color to red and display an error message
            Textbox.BorderColor = new Color(255, 255, 0, 0);
            ErrorLabelText.TextColor = new Color(255, 255, 0, 0);
            ErrorLabelText.Text = "Input contains invalid characters!";
        }
        else
        {
            // If the text input is valid, set the text box border color to green and clear the error message
            Textbox.BorderColor = new Color(255, 0, 255, 0);
            ErrorLabelText.Text = "";
        }
    }
}
