#region Using directives
using System;
using UAManagedCore;
using FTOptix.HMIProject;
using FTOptix.UI;
using FTOptix.NetLogic;
using System.Collections.Generic;
using System.Linq;
#endregion

public class VectorGraphicsLogic : BaseNetLogic
{
    // Private fields to hold references to the objects
    private Panel ShapePanel, StartPanel;
    private CheckBox ShapeName;
    private AdvancedSVGImage SvgImage;

    [ExportMethod]
    public void ShapePicker(NodeId startPanelId, NodeId ShapePanelId, NodeId shapeNameId, NodeId svgImageId)
    {
        // Get the IUANode objects for the ShapePanel, StartPanel, ShapeName, and SvgImage
        ShapePanel = InformationModel.Get<Panel>(ShapePanelId);
        StartPanel = InformationModel.Get<Panel>(startPanelId);
        ShapeName = InformationModel.Get<CheckBox>(shapeNameId);
        SvgImage = InformationModel.Get<AdvancedSVGImage>(svgImageId);

        // If the checkbox pressed by the user is the first one, delete the OptionButton4 radio button
        if (ShapeName.BrowseName.Equals("TriangleCheckbox"))
        {
            var radioButtonToDisable = SearchNode(ShapePanel, "OptionButton4");
            
            if(radioButtonToDisable != null)
                radioButtonToDisable.Delete();
            else
                throw new NullReferenceException("Target is null. Cannot delete object!");
        }

        // Make the ShapePanel and SvgImage visible, and hide the StartPanel
        ShapePanel.Visible = true;
        StartPanel.Visible = false;
        SvgImage.Visible = true;
    }

    // Helper method to search for a specific node within a NodeId using breadth-first search
    private IUANode SearchNode(IUANode node, string targetNodeName)
    {
        // Create a queue to perform a breadth-first search for the target node
        Queue<IUANode> queue = new Queue<IUANode>();
        queue.Enqueue(node);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            // If the current node has the target browse name, return it
            if (current.BrowseName.Equals(targetNodeName))
            {
                return current;
            }

            // Otherwise, add all the current node's children to the queue
            foreach (var child in current.Children)
            {
                queue.Enqueue(child);
            }
        }

        // If we reach here, the target node was not found
        return null;
    }

    // This method is used to get the selected options from two panels
    // The EdgePanelId and ColourPanelId are the NodeIds of the respective panels
    [ExportMethod]
    public void GetSelectedOptions(NodeId EdgePanelId, NodeId ColourPanelId)
    {
        // Get the panels from the InformationModel using their NodeIds
        Panel edgePanel = InformationModel.Get<Panel>(EdgePanelId);
        Panel colourPanel = InformationModel.Get<Panel>(ColourPanelId);

        // Get the children of the panels
        var edgeChildren = edgePanel.Children;
        var colourChildren = colourPanel.Children;

        // Get the selected option from each panel
        var selectedEdge = GetOption(edgeChildren);
        var selectedColour = GetOption(colourChildren);

        // If both options are selected, change the shape
        if (!selectedEdge.Equals("") && !selectedColour.Equals(""))
            ChangeShape(selectedEdge, selectedColour);
    }

    // This method is used to get the selected option from a collection of child items
    private string GetOption(ChildItemCollection children)
    {
        // Loop through each child item in the collection
        foreach (var child in children)
        {
            // Check if the child item is an OptionButton
            if (child.BrowseName.Contains("OptionButton"))
            {
                // Get the RadioButton object from the InformationModel using the child item's NodeId
                var rad = InformationModel.Get<RadioButton>(child.NodeId);

                // Check if the RadioButton is checked
                if (rad.Checked)
                {
                    // If it is checked, return the text of the RadioButton
                    return rad.Text;
                }
            }
        }
        // If no option is selected, return null
        return null;
    }

    // This method is used to change the shape based on the selected edge and colour
    private void ChangeShape(String edge, String colour)
    {
        // Convert the colour from a string to an Android.graphics colour value
        var value = colour switch
        {
            "Red" => 4294901760,
            "Green" => 4278255360,
            "Blue" => 4278190335,
            "Orange" => 4294944000,
            _ => throw new ArgumentException("No such colour exists.")
        };

        // Find the SVGElementProperty with the matching BrowseName to the selected edge
        var matchingProp = SvgImage.SVGElementProperties.FirstOrDefault(prop => prop.BrowseName.Equals(edge));

        // If a matching property is found, set its value to the numerical value of the selected colour
        if (matchingProp != null)
        {
            matchingProp.Value = value;
        }
    }
}
