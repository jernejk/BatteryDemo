using BatteryDemo.Universal.Utils;
using System;
using Windows.Devices.Power;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
namespace BatteryDemo.Universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            UpdateBatteryStatus();
            Battery.AggregateBattery.ReportUpdated += AggregateBattery_ReportUpdated;
        }

        private async void AggregateBattery_ReportUpdated(Battery sender, object args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, UpdateBatteryStatus);
        }

        private void UpdateBatteryStatus()
        {
            BatteryReport report = Battery.AggregateBattery.GetReport();

            // Built-in values
            DesignedMaxCapacity.Text = $"Designed max capacity: {report.DesignCapacityInMilliwattHours?.ToString() ?? "NaN"} mWh";
            CurrentCapacity.Text = $"Current/full capacity: {report.RemainingCapacityInMilliwattHours?.ToString() ?? "NaN"}/{report.FullChargeCapacityInMilliwattHours?.ToString() ?? "NaN"} mWh";
            BatteryState.Text = $"Battery status: {report.Status}";
            ChargeRate.Text = $"Charge rate: {report.ChargeRateInMilliwatts?.ToString() ?? "NaN"} mWh";
            
            // Calculated values (Utils/BatteryReportExtensions.cs)
            BatteryPercentage.Text = $"Battery life in percents: {report.BatteryLevelInPercentage()?.ToString() ?? "--"} %";

            if (report.IsChargingBasedOnConsumption())
            {
                string timeText = GetTimeAsString(report.EstimateTimeToCharge());
                TimeToCharge.Text = $"Time to charge: {timeText}";

                timeText = GetTimeAsString(report.EstimateTimeToChargeFromZeroToFull());
                TimeToChargeFromZeroToFull.Text = $"Charge from zero to full: {timeText}";
            }
            else if (report.IsDischargingBasedOnConsumption())
            {
                string timeText = GetTimeAsString(report.EstimateTimeToDischarge());
                TimeToCharge.Text = $"Time to discharge: {timeText}";

                timeText = GetTimeAsString(report.EstimateTimeToDischargeFromFullToZero());
                TimeToChargeFromZeroToFull.Text = $"Discharge from full to zero: {timeText}";
            }
            else
            {
                TimeToCharge.Text = "Time to charge: --:--";
                TimeToChargeFromZeroToFull.Text = "Charge from zero to full: --:--";
            }

            BatteryHealth.Text = $"Battery health: {report.BatteryHealthInPercentage()?.ToString() ?? "--"} %";
        }

        private static string GetTimeAsString(TimeSpan? time)
        {
            return time.HasValue ? time.Value.ToString("hh\\:mm\\:ss") : "--:--";
        }
    }
}
