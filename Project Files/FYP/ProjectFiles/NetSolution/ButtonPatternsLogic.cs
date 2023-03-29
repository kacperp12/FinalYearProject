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
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.ODBCStore;
using FTOptix.Recipe;
using FTOptix.Datalogger;
using FTOptix.EventLogger;
#endregion

public class ButtonPatternsLogic : BaseNetLogic
{
    // Declare private fields for the controls and values used in the methods
    private TextBox MinValueBox, MaxValueBox;
    private Button UpdateButton;
    private CircularGauge Gauge;
    private Led GreenLEDLight, OrangeLEDLight, RedLEDLight;
    private int MinValueAsInt, MaxValueAsInt;
    private const string WARNING_ZONE_WARNING_ALARM = "Warning Alarm";
    private const string WARNING_ZONE_DANGER_ALARM = "Danger Alarm";

    // Method for validating the minimum and maximum values provided by the user.
    [ExportMethod]
    public void LevelValuesValidation(NodeId MinValueNodeId, NodeId MaxValueNodeId, NodeId UpdateButtonNodeId)
    {
        // Get the controls from the InformationModel using their NodeIds.
        MinValueBox = InformationModel.Get<TextBox>(MinValueNodeId);
        MaxValueBox = InformationModel.Get<TextBox>(MaxValueNodeId);
        UpdateButton = InformationModel.Get<Button>(UpdateButtonNodeId);

        // Convert the values in the TextBoxes to integers and check if they are valid.
        if (int.TryParse(MinValueBox.Text, out MinValueAsInt) && (MinValueAsInt >= 0 && MinValueAsInt <= 100) &&
            int.TryParse(MaxValueBox.Text, out MaxValueAsInt) && (MaxValueAsInt >= 0 && MaxValueAsInt <= 100) &&
            MinValueAsInt < MaxValueAsInt)
        {
            // Enable the UpdateButton if the values are valid.
            UpdateButton.Enabled = true;
        }
        else
        {
            // Disable the UpdateButton if the values are not valid.
            UpdateButton.Enabled = false;
        }
    }

    // Method for updating the warning zones of the CircularGauge based on the values provided by the user.
    [ExportMethod]
    public void UpdateGaugeValues(NodeId GaugeNodeId, NodeId UpdateButtonNodeId, NodeId MinValueNodeId, NodeId MaxValueNodeId)
    {
        // Get the controls from the InformationModel using their NodeIds.
        Gauge = InformationModel.Get<CircularGauge>(GaugeNodeId);
        UpdateButton = InformationModel.Get<Button>(UpdateButtonNodeId);
        MinValueBox = InformationModel.Get<TextBox>(MinValueNodeId);
        MaxValueBox = InformationModel.Get<TextBox>(MaxValueNodeId);

        // Check if the update button is inside WarningRectangle.
        if (UpdateButton.Parent.BrowseName.Equals("WarningRectangle"))
        {
            // Update the warning zone for the warning alarm if the update button is inside WarningRectangle.
            UpdateWarningZone(Gauge.WarningZones[WARNING_ZONE_WARNING_ALARM]);
        }
        else
        {
            // Update the warning zone for the danger alarm if the update button is not inside WarningRectangle.
            UpdateWarningZone(Gauge.WarningZones[WARNING_ZONE_DANGER_ALARM]);
        }
    }

    // Method for updating a given warning zone based on the values provided by the user.
    private void UpdateWarningZone(WarningZone warningZone)
    {
        // Convert the values in the textboxes to integers and update the warning zone.
        if (int.TryParse(MinValueBox.Text, out int minValue) && int.TryParse(MaxValueBox.Text, out int maxValue))
        {
            warningZone.From = minValue;
            warningZone.To = maxValue;
        }
    }

    // Method for updating the LED lights based on the current value of CircularGauge.
    [ExportMethod]
    public void UpdateLEDLight(NodeId GreenLEDLightNodeId, NodeId OrangeLEDLightNodeId, NodeId RedLEDLightNodeId, NodeId GaugeNodeId)
    {
        // Get the controls from the InformationModel using their NodeIds
        Gauge = InformationModel.Get<CircularGauge>(GaugeNodeId);
        GreenLEDLight = InformationModel.Get<Led>(GreenLEDLightNodeId);
        OrangeLEDLight = InformationModel.Get<Led>(OrangeLEDLightNodeId);
        RedLEDLight = InformationModel.Get<Led>(RedLEDLightNodeId);

        // Check if the current value of CircularGauge falls inside the warning zone for the warning alarm
        if (Gauge.Value >= Gauge.WarningZones[WARNING_ZONE_WARNING_ALARM].From && Gauge.Value <= Gauge.WarningZones[WARNING_ZONE_WARNING_ALARM].To)
        {
            // Set the LED lights accordingly if the current value falls inside the warning zone for the warning alarm
            GreenLEDLight.Active = false;
            OrangeLEDLight.Active = true;
            RedLEDLight.Active = false;
        }
        // Check if the current value of CircularGauge falls inside the warning zone for the danger alarm
        else if (Gauge.Value >= Gauge.WarningZones[WARNING_ZONE_DANGER_ALARM].From && Gauge.Value <= Gauge.WarningZones[WARNING_ZONE_DANGER_ALARM].To)
        {
            // Set the LED lights accordingly if the current value falls inside the warning zone for the danger alarm
            GreenLEDLight.Active = false;
            OrangeLEDLight.Active = false;
            RedLEDLight.Active = true;
        }
        else
        {
            // Set the LED lights accordingly if the current value does not fall inside any of the warning zones
            GreenLEDLight.Active = true;
            OrangeLEDLight.Active = false;
            RedLEDLight.Active = false;
        }
    }
}
