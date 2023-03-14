#region Using directives
using System;
using System.Drawing;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.NativeUI;
using FTOptix.UI;
using FTOptix.WebUI;
using FTOptix.NetLogic;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Core;
using Color = System.Drawing.Color;
using System.IO;
using FTOptix.Recipe;
using FTOptix.Datalogger;
using System.Xml;
using FTOptix.EventLogger;
#endregion

public class VectorGr : BaseNetLogic
{
    Panel ShapePanel, StartPanel;
    CheckBox ShapeName;

    AdvancedSVGImage SvgImage;

    [ExportMethod]
    public void ShapePicker(NodeId startPanelId, NodeId ShapePanelId, NodeId shapeNameId, NodeId svgImageId) {
        ShapePanel = InformationModel.Get<Panel>(ShapePanelId);
        StartPanel = InformationModel.Get<Panel>(startPanelId);
        ShapeName = InformationModel.Get<CheckBox>(shapeNameId);
        SvgImage = InformationModel.Get<AdvancedSVGImage>(svgImageId);

        if(ShapeName.BrowseName.Equals("CheckBox1")) {
            var radioButtonToDisable = SearchNode(ShapePanel, "OptionButton4");
            radioButtonToDisable.Delete();
        }

        ShapePanel.Visible = true;
        StartPanel.Visible = false;
        SvgImage.Visible = true;
    }


    // Perform a recursive depth-first search to find the radio button to delete
    private IUANode SearchNode(IUANode node, String targetNodeName) {
        if (node.BrowseName.Equals(targetNodeName))
        {
            return node;
        }
        
        foreach (var child in node.Children)
        {
            var result = SearchNode(child, targetNodeName);
            if (result != null)
            {
            return result;
            }
        }
        return null;
    }

    // public void DisplayShape(string shapeType) {
    //     var projectRelativeResourceUri = ResourceUri.FromProjectRelativePath($"{shapeType}.svg");
    //     svgImage.Path = projectRelativeResourceUri;
    // }

    [ExportMethod]
    public void GetSelectedOptions(NodeId EdgePanelId, NodeId ColourPanelId) {
        RadioButton rad;
        Panel edgePanel = InformationModel.Get<Panel>(EdgePanelId);
        Panel colourPanel = InformationModel.Get<Panel>(ColourPanelId);
        var edgeChildren = edgePanel.Children;
        var colourChildren = colourPanel.Children;

        String selectedEdge = "";
        String selectedColour = "";

        foreach(var child in edgeChildren) {
            if(child.BrowseName.Contains("OptionButton")) {
                var childNode = child.NodeId;
                rad = InformationModel.Get<RadioButton>(childNode);
                
                if(rad.Checked == true) {
                    selectedEdge = rad.Text;
                    break;
                }

            }
        }

        foreach(var child in colourChildren) {
            if(child.BrowseName.Contains("OptionButton")) {
                var childNode = child.NodeId;
                rad = InformationModel.Get<RadioButton>(childNode);
                
                if(rad.Checked == true) {
                    selectedColour = rad.Text;
                    break;
                }

            }
        }

        if(!selectedEdge.Equals("") && !selectedColour.Equals(""))
            ChangeShape(selectedEdge, selectedColour);
    }

    private void ChangeShape(String edge, String colour) {
        var properties = SvgImage.SVGElementProperties;
        var red = "#cf1212f2";
        var green = "#08d723e7";
        var blue = "#0f1bd3ed";
        var black = "#030303fb";

        edge = "Property2";

        Color color = Color.FromArgb(Int32.Parse(red.Replace("#", ""), System.Globalization.NumberStyles.HexNumber));
        long value = 3474068210;

        foreach(var prop in properties) {
            if(prop.BrowseName.Equals(edge)) {
                prop.Value = value;
            }
        }

        // var projectRelativeResourceUri = ResourceUri.FromProjectRelativePath($"{Shape.Text}.svg");

        // XmlDocument svgDoc = new XmlDocument();
        // svgDoc.Load(projectRelativeResourceUri.Uri);

        // var insideSvgDoc = svgDoc.FirstChild;

        // foreach (XmlNode childNode in insideSvgDoc.ChildNodes)
        // {
        //     if (childNode.Name == "line" && childNode.Attributes["id"].Value == edge)
        //     {
        //         childNode.Attributes["stroke"].Value = colour;
        //         break;
        //     }
        // }

        // svgDoc.Save(projectRelativeResourceUri.Uri);

        // svgImage.Path = projectRelativeResourceUri;
    }
}
