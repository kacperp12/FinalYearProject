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
using FTOptix.Alarm;
#endregion

public class RuntimeNetLogic3 : BaseNetLogic
{
    Label label1;
    Panel panel1;

    [ExportMethod]
    public void AdjustValueOfLabel(NodeId labelNode, NodeId panelNode) 
    {
        label1 = InformationModel.Get<Label>(labelNode);
        panel1 = InformationModel.Get<Panel>(panelNode);

        var width = panel1.WidthVariable.Value;
        var height = panel1.HeightVariable.ToString();


        label1.Text = panel1.WidthVariable.Value;
        Console.WriteLine(panel1.Width.ToString());
        Console.WriteLine(panel1.Width.ToString());
    }
}
