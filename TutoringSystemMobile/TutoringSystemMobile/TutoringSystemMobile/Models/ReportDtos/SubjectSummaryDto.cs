namespace TutoringSystemMobile.Models.ReportDtos
{
    public class SubjectSummaryDto
    {
        public string SubjectName { get; set; }
        public string ReservationsCount { get; set; }
        public string TotalHours { get; set; }
        public string TotalProfit { get; set; }

        public SubjectSummaryDto()
        {
        }

        public SubjectSummaryDto(string subject, int reservationsCount, double totalHours, double totalProfit)
        {
            SubjectName = $"{subject}";
            TotalProfit = $" {totalProfit} zł ";
            ReservationsCount = $" {reservationsCount} ";
            TotalHours = $" {totalHours}h ";
        }
    }
}
