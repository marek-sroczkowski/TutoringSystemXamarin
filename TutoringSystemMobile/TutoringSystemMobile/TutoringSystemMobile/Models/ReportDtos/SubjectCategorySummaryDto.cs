using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.ReportDtos
{
    public class SubjectCategorySummaryDto
    {
        public string CategoryName { get; set; }
        public string ReservationsCount { get; set; }
        public string TotalHours { get; set; }
        public string TotalProfit { get; set; }

        public SubjectCategorySummaryDto()
        {
        }

        public SubjectCategorySummaryDto(SubjectCategory category, int reservationsCount, double totalHours, double totalProfit)
        {
            SetCategoryName(category);
            TotalProfit = $" {totalProfit} zł ";
            ReservationsCount = $" {reservationsCount} ";
            TotalHours = $" {totalHours}h ";
        }

        private void SetCategoryName(SubjectCategory category)
        {
            switch (category)
            {
                case SubjectCategory.Other:
                    CategoryName = "Inna";
                    break;
                case SubjectCategory.Math:
                    CategoryName = "Matematyka";
                    break;
                case SubjectCategory.Informatics:
                    CategoryName = "Informatyka";
                    break;
                case SubjectCategory.ForeignLanguage:
                    CategoryName = "Język obcy";
                    break;
                case SubjectCategory.NativeLanguage:
                    CategoryName = "Język polski";
                    break;
                case SubjectCategory.Physics:
                    CategoryName = "Fizyka";
                    break;
                case SubjectCategory.Biology:
                    CategoryName = "Biologia";
                    break;
                case SubjectCategory.Chemistry:
                    CategoryName = "Chemia";
                    break;
                case SubjectCategory.Music:
                    CategoryName = "Muzyka";
                    break;
                case SubjectCategory.Geography:
                    CategoryName = "Geografia";
                    break;
                default:
                    break;
            }
        }
    }
}