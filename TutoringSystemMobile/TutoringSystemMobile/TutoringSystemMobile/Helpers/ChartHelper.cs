using System.Collections.Generic;
using TutoringSystemMobile.Constans;

namespace TutoringSystemMobile.Helpers
{
    public class ChartHelper
    {
        public static List<string> GetProfitNames()
        {
            var chartDataSources = new List<string>
            {
                PickerConstans.TotalProfit,
                PickerConstans.TutoringProfit,
                PickerConstans.OrderProfit,
                PickerConstans.TotalHours
            };

            return chartDataSources;
        }

        public static List<string> GetChartTypes()
        {
            var chartTypes = new List<string>
            {
                PickerConstans.DonutChart,
                PickerConstans.BarChart
            };

            return chartTypes;
        }

        public static List<string> GetReportTypes()
        {
            var dataSources = new List<string>
            {
                PickerConstans.Profit,
                PickerConstans.ReservationCount,
                PickerConstans.TotalHours
            };

            return dataSources;
        }
    }
}