using Windows.Phone.Devices.Power;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641
namespace BatteryDemo.Wp8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly Battery battery;

        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;

            battery = Battery.GetDefault();
            battery.RemainingChargePercentChanged += Battery_RemainingChargePercentChanged;

            UpdateBattery();
        }

        private void Battery_RemainingChargePercentChanged(object sender, object e)
        {
            UpdateBattery();
        }

        private void UpdateBattery()
        {
            BatteryLife.Text = string.Format("{0} %", battery.RemainingChargePercent);
            BatteryLife.Text = string.Format("{0} %", battery.RemainingChargePercent);
        }
    }
}
