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

public sealed class EircodeValidation : BaseNetLogic
{
    Label ErrorLabel;
    TextBox Textbox;
    Button SubmitButton;

    [ExportMethod]
    public void DynamicEircodeInputValidation(NodeId TextboxNodeId, NodeId ErrorLabelNodeId, NodeId SubmitButtonNodeId)
    {
        string regexPattern = @"^[ACDEFHKNPRTVWXY]{1}[0-9]{1}[0-9W]{1}[\ ]{1}[0-9ACDEFHKNPRTVWXY]{4}";
        ErrorLabel = InformationModel.Get<Label>(ErrorLabelNodeId);
        Textbox = InformationModel.Get<TextBox>(TextboxNodeId); 
        SubmitButton = InformationModel.Get<Button>(SubmitButtonNodeId);

        if(!Regex.Match(Textbox.Text, regexPattern).Success || Textbox.Text.Length != 8) {
            Textbox.BorderColor = new Color(255, 255, 0, 0);
            ErrorLabel.TextColor = new Color(255, 255, 0, 0);
            ErrorLabel.Text = "Please check your input!";
            SubmitButton.Enabled = false;
        }
        else {
            Textbox.BorderColor = new Color(255, 0, 255, 0);
            ErrorLabel.Text = "";
            SubmitButton.Enabled = true;
        }
    }

    [ExportMethod]
    public void SubmitEircode(NodeId SubmittedLabelNodeId)
    {
        Label SubmittedLabel = InformationModel.Get<Label>(SubmittedLabelNodeId);
        SubmittedLabel.Opacity = 100;
        FadeOutLabel(SubmittedLabel, 2);
    }

    private async void FadeOutLabel(Label LabelToBeFaded, int interval)
    {
        int initialFadeDelay = 1000;
        await Task.Delay(initialFadeDelay);

        while(LabelToBeFaded.Opacity > 0.0) 
        {
            await Task.Delay(interval);
            LabelToBeFaded.Opacity -= 2.0F;
        }
        LabelToBeFaded.Opacity = 0;
    }
}
