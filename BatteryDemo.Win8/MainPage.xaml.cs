using BatteryDemo.Wpf;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
namespace BatteryDemo.Win8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer timer = new DispatcherTimer();

        public MainPage()
        {
            this.InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Timer_Tick1;
            timer.Start();

            UpdateBattery();
        }

        private void Timer_Tick1(object sender, object e)
        {
            UpdateBattery();
        }

        private void UpdateBattery()
        {
            BatteryService.SYSTEM_POWER_STATUS status = BatteryService.GetStatus();
            BatteryLifePercent.Text = string.Format("{0} %", status.BatteryLifePercent);
            BatteryFlag.Text = string.Format("Battery flag: {0}", status.BatteryFlag);
            ACLineStatus.Text = string.Format("AC line status: {0}", status.ACLineStatus);
            BatteryLifeTime.Text = string.Format("Battery life time: {1} ({0} sec)", status.BatteryLifeTime, status.CalculatedBatteryLifeTime);
            BatteryFullLifeTime.Text = string.Format("Battery full life time: {1} ({0} sec)", status.BatteryFullLifeTime, status.CalculatedBatteryFullLifeTime);
            IsCharging.Text = string.Format("Is charging: {0}", status.IsCharging);
        }
    }
}
