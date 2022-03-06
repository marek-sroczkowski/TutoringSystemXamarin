namespace TutoringSystemMobile.Models.Dtos.Report
{
    public class StudentSummaryDto
    {
        public string StudentFullName { get; set; }
        public string ReservationsCount { get; set; }
        public string TotalHours { get; set; }
        public string TotalProfit { get; set; }

        public StudentSummaryDto()
        {
        }

        public StudentSummaryDto(string username, string studentName, int reservationsCount, double totalHours, double totalProfit)
        {
            StudentFullName = $"{studentName} ({username})";
            TotalProfit = $" {totalProfit} zł ";
            ReservationsCount = $" {reservationsCount} ";
            TotalHours = $" {totalHours}h ";
        }
    }
}