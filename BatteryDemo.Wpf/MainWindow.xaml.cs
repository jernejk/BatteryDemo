using System;
using System.Windows;
using System.Windows.Threading;

namespace BatteryDemo.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateBattery();
        }

        private void Timer_Tick(object sender, EventArgs e)
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
