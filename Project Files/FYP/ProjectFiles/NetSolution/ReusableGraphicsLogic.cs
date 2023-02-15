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
#endregion

public class ReusableGraphicsLogic : BaseNetLogic
{
    [ExportMethod]
    public void VariableInputValidation() {

    }

    [ExportMethod]
    public void EnableUserInput(NodeId inputBox) {
        TextBox textbox = InformationModel.Get<TextBox>(inputBox);
        textbox.Enabled = true;
    }
}
