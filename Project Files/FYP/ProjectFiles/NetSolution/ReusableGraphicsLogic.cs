#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.Datalogger;
using FTOptix.NativeUI;
using FTOptix.UI;
using FTOptix.WebUI;
using FTOptix.NetLogic;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.EventLogger;
using System.Collections.Generic;
using System.Text.RegularExpressions;
#endregion

public class ReusableGraphicsLogic : BaseNetLogic
{
    // List of validation patterns for different input types
    private List<(string, string, string)> validationPatterns = new List<(string, string, string)>
{
    // Tuple for Eircode input type
    ("Eircode", @"^[ACDEFHKNPRTVWXY]{1}[0-9]{1}[0-9W]{1}[ ]{1}[0-9ACDEFHKNPRTVWXY]{4}", "3 characters, a space and 4 characters"),
    // Tuple for Email input type
    ("Email", @"^[a-zA-Z0-9]{1,15}@[a-zA-Z]{1,8}\.com$", "Maximum of 15 chars, @ symbol, maximum of 8 chars, ends with .com"),
    // Tuple for Number input type
    ("Number", @"^08[0-9]{8}$", "10 digits long, beginning with 08")
};

    // UI elements
    private Label ErrorLabel, InputType, UserHelp;
    private TextBox Textbox;
    private Button SubmitButton;

    // Flag to check if an input type has been selected
    private bool isSelected = false;

    // Method to validate user input based on the selected input type
    [ExportMethod]
    public void VariableInputValidation(NodeId InputTypeLabelId, NodeId TextboxNodeId, NodeId ErrorLabelNodeId, NodeId SubmitButtonNodeId, NodeId UserHelpLabelNodeId)
    {
        // Get UI elements from their respective NodeIds
        ErrorLabel = InformationModel.Get<Label>(ErrorLabelNodeId);
        InputType = InformationModel.Get<Label>(InputTypeLabelId);
        Textbox = InformationModel.Get<TextBox>(TextboxNodeId);
        SubmitButton = InformationModel.Get<Button>(SubmitButtonNodeId);
        UserHelp = InformationModel.Get<Label>(UserHelpLabelNodeId);

        // Variables to store the regex pattern and error message for the selected input type
        var returnedRegex = string.Empty;
        var errorMessage = string.Empty;

        // Loop through the validation patterns to find the one that matches the selected input type
        foreach ((string key, string value, string message) in validationPatterns)
        {
            if (key == InputType.Text)
            {
                returnedRegex = value;
                errorMessage = message;
                break;
            }
        }

        // If the user input does not match the regex pattern for the selected input type
        if (!Regex.Match(Textbox.Text, returnedRegex).Success)
        {
            // Set UI elements to indicate an error
            Textbox.BorderColor = new Color(255, 255, 0, 0);
            ErrorLabel.TextColor = new Color(255, 255, 0, 0);
            ErrorLabel.Text = $"Please check your input. {InputType.Text} requires:";
            SubmitButton.Enabled = false;
            UserHelp.TextColor = new Color(255, 255, 0, 0);
            UserHelp.Text = errorMessage;
        }
        else
        {
            // Set UI elements to indicate successful validation
            Textbox.BorderColor = new Color(255, 0, 255, 0);
            ErrorLabel.Text = "";
            SubmitButton.Enabled = true;
            UserHelp.Text = "";
        }
    }

    [ExportMethod]
    public void EnableUserInput(NodeId inputBox, NodeId listBox, NodeId SubmitButtonNodeId)
    {
        TextBox textbox = InformationModel.Get<TextBox>(inputBox);
        ListBox listbox = InformationModel.Get<ListBox>(listBox);

        // Disable the textbox and submit button if no input is selected
        if (listbox.UISelectedItem == null)
        {
            textbox.Text = "";
            textbox.Enabled = false;
        }
        else
        {
            // Enable the textbox
            textbox.Enabled = true;

            // Check if the selected item has changed
            bool isItemSelected = listbox.UISelectedValue != null;
            if (isItemSelected && !isSelected)
            {
                isSelected = true;
            }
            else if (!isItemSelected && isSelected)
            {
                isSelected = false;
            }
        }
    }

    [ExportMethod]
    public void ResetUserInput(NodeId inputBox)
    {
        TextBox textbox = InformationModel.Get<TextBox>(inputBox);
        textbox.Text = "";
        textbox.BorderColor = new Color(128, 128, 128, 0);

        if ((SubmitButton != null) && (SubmitButton.IsValid))
            SubmitButton.Enabled = false;

        if ((UserHelp != null) && (UserHelp.IsValid))
            UserHelp.Text = "";

        if ((ErrorLabel != null) && (ErrorLabel.IsValid))
            ErrorLabel.Text = "";
    }
}
