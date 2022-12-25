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
using System.Linq;
using FTOptix.AuditSigning;
using FTOptix.Alarm;
#endregion

public class RuntimeNetLogic2 : BaseNetLogic
{
    TextBox MinValueBox, MaxValueBox;
    Button UpdateButton;
    CircularGauge Gauge;
    Led GreenLEDLight, OrangeLEDLight, RedLEDLight;

    int MinValueAsInt, MaxValueAsInt;

    [ExportMethod]
    public void LevelValuesValidation(NodeId MinValueNodeId, NodeId MaxValueNodeId, NodeId UpdateButtonNodeId) {
        MinValueBox = InformationModel.Get<TextBox>(MinValueNodeId);
        MaxValueBox = InformationModel.Get<TextBox>(MaxValueNodeId);
        UpdateButton = InformationModel.Get<Button>(UpdateButtonNodeId);

        if(int.TryParse(MinValueBox.Text, out MinValueAsInt) && (MinValueAsInt >= 0 && MinValueAsInt <= 100) && 
                int.TryParse(MaxValueBox.Text, out MaxValueAsInt) && (MaxValueAsInt >= 0 && MaxValueAsInt <= 100) &&
                MinValueAsInt < MaxValueAsInt) 
        {
            UpdateButton.Enabled = true;
        }
        else 
        {
            UpdateButton.Enabled = false;
        }
    }

    [ExportMethod]
    public void UpdateGaugeValues(NodeId GaugeNodeId) {
        Gauge = InformationModel.Get<CircularGauge>(GaugeNodeId);

        string WarningZoneToUpdate = (UpdateButton.Parent.BrowseName.Equals("WarningLevelPanel")) ? "Warning Alarm" : "Danger Alarm";

        Gauge.WarningZones[WarningZoneToUpdate].From = MinValueAsInt;
        Gauge.WarningZones[WarningZoneToUpdate].To = MaxValueAsInt;
    }

    [ExportMethod]
    public void UpdateLEDLight(NodeId GreenLEDLightNodeId, NodeId OrangeLEDLightNodeId, NodeId RedLEDLightNodeId, NodeId GaugeNodeId){
        Gauge = InformationModel.Get<CircularGauge>(GaugeNodeId);
        GreenLEDLight = InformationModel.Get<Led>(GreenLEDLightNodeId);
        OrangeLEDLight = InformationModel.Get<Led>(OrangeLEDLightNodeId);
        RedLEDLight = InformationModel.Get<Led>(RedLEDLightNodeId);

        if(Gauge.Value >= Gauge.WarningZones["Warning Alarm"].From && Gauge.Value <= Gauge.WarningZones["Warning Alarm"].To) {
            GreenLEDLight.Active = false;
            OrangeLEDLight.Active = true;
            RedLEDLight.Active = false;
        }
        else if(Gauge.Value >= Gauge.WarningZones["Danger Alarm"].From && Gauge.Value <= Gauge.WarningZones["Danger Alarm"].To) {
            GreenLEDLight.Active = false;
            OrangeLEDLight.Active = false;
            RedLEDLight.Active = true;
        }
        else {
            GreenLEDLight.Active = true;
            OrangeLEDLight.Active = false;
            RedLEDLight.Active = false;
        }
    }
}
