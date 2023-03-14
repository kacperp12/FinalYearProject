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

public class RuntimeNetLogic1 : BaseNetLogic
{
    Panel mainContentPanel, inputConfirmationPanel;

    [ExportMethod]
    public void ChangePanelFocus(NodeId mainId, NodeId confirmId) {
        mainContentPanel = InformationModel.Get<Panel>(mainId);
        inputConfirmationPanel = InformationModel.Get<Panel>(confirmId);

        mainContentPanel.Enabled = false;
        inputConfirmationPanel.Visible = true;
    }

    [ExportMethod]
    public void ReenableMainPanel() {
        mainContentPanel.Enabled = true;
        inputConfirmationPanel.Visible = false;
    }
}
