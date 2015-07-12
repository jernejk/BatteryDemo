using BatteryDemo.Wpf;
using System.Threading.Tasks;

namespace BatteryDemo.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Task task = StartAsync();
            task.Wait();
        }

        public static async Task StartAsync()
        {
            while (true)
            {
                BatteryService.SYSTEM_POWER_STATUS status = BatteryService.GetStatus();

                System.Console.WriteLine("Battery data (same implementation as WPF)");
                System.Console.WriteLine();

                System.Console.WriteLine("Battery life: {0} %", status.BatteryLifePercent);
                System.Console.WriteLine("Battery flag: {0}", status.BatteryFlag);
                System.Console.WriteLine("AC line status: {0}", status.ACLineStatus);
                System.Console.WriteLine("Battery life time: {1} ({0} sec)", status.BatteryLifeTime, status.CalculatedBatteryLifeTime);
                System.Console.WriteLine("Battery full life time: {1} ({0} sec)", status.BatteryFullLifeTime, status.CalculatedBatteryFullLifeTime);
                System.Console.WriteLine("Is charging: {0}", status.IsCharging);

                System.Console.WriteLine();
                System.Console.WriteLine();

                System.Console.WriteLine("Press any key to exit console or R to refresh...");

                bool closeApp = false;
                // Wait for any key to be pressed and exit app every 500 ms for 10s.
                for (int i = 0; i < 10000; i += 500)
                {
                    await Task.Delay(500);
                    if (System.Console.KeyAvailable)
                    {
                        closeApp = System.Console.ReadKey().KeyChar != 'r' && System.Console.ReadKey().KeyChar != 'R';
                        break;
                    }
                }

                if (closeApp)
                {

                    break;
                }
                
                System.Console.Clear();
            }
        }
    }
}
