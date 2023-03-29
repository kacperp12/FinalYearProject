#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.Retentivity;
using FTOptix.NativeUI;
using FTOptix.UI;
using FTOptix.NetLogic;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.WebUI;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FTOptix.AuditSigning;
using FTOptix.Alarm;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.ODBCStore;
using FTOptix.Recipe;
using FTOptix.Datalogger;
using FTOptix.EventLogger;
#endregion

// This class provides a method for validating Eircode inputs and submitting them.
public class EircodeValidationLogic : BaseNetLogic
{
    // This is the regular expression pattern used to validate Eircode inputs.
    private const string RegexPattern = @"^(?:^[AC-FHKNPRTV-Y][0-9]{2}|D6W)[ ]{1}[0-9AC-FHKNPRTV-Y]{4}$";

    // These are the UI elements used in the Eircode validation process.
    private Label ErrorLabel;
    private TextBox Textbox;
    private Button SubmitButton;

    // This method is used to dynamically validate Eircode inputs.
    [ExportMethod]
    public void DynamicEircodeInputValidation(NodeId TextboxNodeId, NodeId ErrorLabelNodeId, NodeId SubmitButtonNodeId)
    {
        // Get the UI elements.
        ErrorLabel = InformationModel.Get<Label>(ErrorLabelNodeId);
        Textbox = InformationModel.Get<TextBox>(TextboxNodeId); 
        SubmitButton = InformationModel.Get<Button>(SubmitButtonNodeId);

        // Validate the Eircode input.
        ValidateEircode();
    }

    // This method is used to submit a valid Eircode input.
    [ExportMethod]
    public void SubmitEircode(NodeId SubmittedLabelNodeId)
    {
        // Get the submitted label from the InformationModel.
        Label SubmittedLabel = InformationModel.Get<Label>(SubmittedLabelNodeId);

        // Set the opacity of the submitted label to 100% and fade it out over 2 seconds.
        SubmittedLabel.Opacity = 100;
        FadeOutLabel(SubmittedLabel, 2);
    }

    // This method is used to validate the Eircode input.
    private void ValidateEircode()
    {
        // If the Eircode input is not valid, display an error message and disable the submit button.
        if (!IsValidEircode(Textbox.Text))
        {
            Textbox.BorderColor = new Color(255, 255, 0, 0);
            ErrorLabel.TextColor = new Color(255, 255, 0, 0);
            ErrorLabel.Text = "Please check your input!";
            SubmitButton.Enabled = false;
        }
        // If the Eircode input is valid, clear the error message and enable the submit button.
        else
        {
            Textbox.BorderColor = new Color(255, 0, 255, 0);
            ErrorLabel.Text = "";
            SubmitButton.Enabled = true;
        }
    }

    // This method is used to check if an Eircode input is valid.
    private static bool IsValidEircode(string eircode)
    {
        return Regex.IsMatch(eircode, RegexPattern);
    }

    // This method is used to fade out a label over a specified interval.
    private async void FadeOutLabel(Label LabelToBeFaded, int interval)
    {
        // Set an initial delay before fading out the label.
        int initialFadeDelay = 1000;
        await Task.Delay(initialFadeDelay);

        // Fade out the label over the specified interval.
        while(LabelToBeFaded.Opacity > 0.0) 
        {
            await Task.Delay(interval);
            LabelToBeFaded.Opacity -= 2.0F;
        }
        LabelToBeFaded.Opacity = 0;
    }
}