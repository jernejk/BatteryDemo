using System;
using System.Runtime.InteropServices;

namespace BatteryDemo.Wpf
{
    public class BatteryService
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int GetSystemPowerStatus(ref SYSTEM_POWER_STATUS lpSystemPowerStatus);

        public struct SYSTEM_POWER_STATUS
        {
            public ACLineStatus ACLineStatus;
            public BatteryFlag BatteryFlag;
            public byte BatteryLifePercent;
            public byte Reserved1;
            public int BatteryLifeTime;
            public int BatteryFullLifeTime;

            public TimeSpan CalculatedBatteryLifeTime { get { return TimeSpan.FromSeconds(BatteryLifeTime); } }

            public TimeSpan CalculatedBatteryFullLifeTime { get { return TimeSpan.FromSeconds(BatteryFullLifeTime); } }

            public bool IsCharging { get { return (BatteryFlag & BatteryFlag.Charging) > 0; } }
        }

        [Flags]
        public enum BatteryFlag : byte
        {
            High = 1,
            Low = 2,
            Critical = 4,
            Charging = 8,
            NoSystemBattery = 128,
            Unknown = 255
        }

        public enum ACLineStatus : byte
        {
            Offline = 0,
            Online = 1,
            Unknown = 255
        }

        public static SYSTEM_POWER_STATUS GetStatus()
        {
            SYSTEM_POWER_STATUS SPS = new SYSTEM_POWER_STATUS();
            GetSystemPowerStatus(ref SPS);
            return SPS;
        }
    }
}
