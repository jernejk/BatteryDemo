using System;
using Windows.Devices.Power;

namespace BatteryDemo.Universal.Utils
{
    public static class BatteryReportExtensions
    {
        public static bool IsBatteryPresent(this BatteryReport report)
            => report.Status != Windows.System.Power.BatteryStatus.NotPresent;

        public static bool IsChargingBasedOnConsumption(this BatteryReport report) =>
            report.ChargeRateInMilliwatts.HasValue && report.ChargeRateInMilliwatts.Value > 0;

        public static bool IsDischargingBasedOnConsumption(this BatteryReport report) =>
            report.ChargeRateInMilliwatts.HasValue && report.ChargeRateInMilliwatts.Value < 0;

        public static double? BatteryLevel(this BatteryReport report)
        {
            if (report.RemainingCapacityInMilliwattHours.HasValue && report.FullChargeCapacityInMilliwattHours.HasValue)
            {
                int remainingCapacity = report.RemainingCapacityInMilliwattHours.Value;
                int fullCapacity = report.FullChargeCapacityInMilliwattHours.Value;
                return remainingCapacity / (double)fullCapacity;
            }

            // Not enough data available
            return null;
        }

        public static double? BatteryLevelInPercentage(this BatteryReport report)
        {
            double? batteryLevel = BatteryLevel(report);
            return batteryLevel.HasValue ? batteryLevel.Value * 100d : (double?)null;
        }

        public static double? BatteryHealthInPercentage(this BatteryReport report)
        {
            if (report.FullChargeCapacityInMilliwattHours.HasValue && report.DesignCapacityInMilliwattHours.HasValue)
            {
                return (report.FullChargeCapacityInMilliwattHours.Value / (double)report.DesignCapacityInMilliwattHours.Value) * 100d;
            }

            // Not enough data
            return null;
        }

        public static TimeSpan? EstimateTimeToCharge(this BatteryReport report)
        {
            // Ignore battery status because we can have charge data even if status is not charging (rare but possible)
            // Also check if full and remaining battery capacity is available.
            if (report.ChargeRateInMilliwatts.HasValue && report.ChargeRateInMilliwatts.Value > 0 &&
                report.FullChargeCapacityInMilliwattHours.HasValue && report.RemainingCapacityInMilliwattHours.HasValue)
            {
                int remainingCapacityToCharge = report.FullChargeCapacityInMilliwattHours.Value - report.RemainingCapacityInMilliwattHours.Value;
                double hoursToCharge = remainingCapacityToCharge / (double)report.ChargeRateInMilliwatts.Value;

                return TimeSpan.FromHours(hoursToCharge);
            }

            // Not enough data or valid data
            return null;
        }

        public static TimeSpan? EstimateTimeToChargeFromZeroToFull(this BatteryReport report)
        {
            // Ignore battery status because we can have charge data even if status is not charging (rare but possible)
            if (report.ChargeRateInMilliwatts.HasValue && report.ChargeRateInMilliwatts.Value > 0 &&
                report.FullChargeCapacityInMilliwattHours.HasValue)
            {
                double hoursToCharge = CalculateHoursLeft(report, report.FullChargeCapacityInMilliwattHours.Value);
                return TimeSpan.FromHours(hoursToCharge);
            }

            // Not enough data or valid data
            return null;
        }

        public static TimeSpan? EstimateTimeToDischarge(this BatteryReport report)
        {
            // Ignore battery status because battery consumption can be higher than charging. (playing high-demand games or using navigation in a car with a weak charger or recording videos while charging battery)
            // Also check if full battery capacity is available.
            if (report.ChargeRateInMilliwatts.HasValue && report.ChargeRateInMilliwatts.Value < 0 &&
                report.RemainingCapacityInMilliwattHours.HasValue)
            {
                double hoursToDischarge = CalculateHoursLeft(report, report.RemainingCapacityInMilliwattHours.Value);
                return TimeSpan.FromHours(-hoursToDischarge);
            }

            // Not enough data or valid data
            return null;
        }

        public static TimeSpan? EstimateTimeToDischargeFromFullToZero(this BatteryReport report)
        {
            // Ignore battery status because battery consumption can be higher than charging. (playing high-demand games or using navigation in a car with a weak charger or recording videos while charging battery)
            // Also check if full battery capacity is available.
            if (report.ChargeRateInMilliwatts.HasValue && report.ChargeRateInMilliwatts.Value < 0 &&
                report.RemainingCapacityInMilliwattHours.HasValue)
            {
                double hoursToDischarge = CalculateHoursLeft(report, report.FullChargeCapacityInMilliwattHours.Value);
                return TimeSpan.FromHours(-hoursToDischarge);
            }

            // Not enough data or valid data
            return null;
        }

        /// <summary>
        /// Calculate in how long will it take to charge or discharge a battery with set capacity and current charge rate.
        /// NOTE: Charge is negative when discharging!
        /// </summary>
        /// <param name="report">Battery report</param>
        /// <param name="capacityInMiliwattHours">Capacity used for calculating charge/discharge time</param>
        /// <returns>Returns how long it will take to charge or discharge in hours. Positive number is charging and negative is discharging.</returns>
        private static double CalculateHoursLeft(BatteryReport report, double capacityInMiliwattHours)
            => capacityInMiliwattHours / report.ChargeRateInMilliwatts.Value;
    }
}
