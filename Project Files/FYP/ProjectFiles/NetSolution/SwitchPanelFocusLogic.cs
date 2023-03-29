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
#endregion

public class SwitchPanelFocusLogic : BaseNetLogic
{
    // Define two variables to hold references to the two panels
    Panel mainContentPanel, inputConfirmationPanel;

    // Define a method that can be called to change the focus between the panels
    [ExportMethod]
    public void ChangePanelFocus(NodeId mainId, NodeId confirmId)
    {
        // Retrieve the mainContentPanel and inputConfirmationPanel from the InformationModel using their NodeIds
        mainContentPanel = InformationModel.Get<Panel>(mainId);
        inputConfirmationPanel = InformationModel.Get<Panel>(confirmId);

        // Disable the mainContentPanel and make the inputConfirmationPanel visible
        mainContentPanel.Enabled = false;
        inputConfirmationPanel.Visible = true;
    }

    // Define a method that can be called to re-enable the mainContentPanel and hide the inputConfirmationPanel
    [ExportMethod]
    public void ReenableMainPanel()
    {
        // Re-enable the mainContentPanel and hide the inputConfirmationPanel
        mainContentPanel.Enabled = true;
        inputConfirmationPanel.Visible = false;
    }
}
