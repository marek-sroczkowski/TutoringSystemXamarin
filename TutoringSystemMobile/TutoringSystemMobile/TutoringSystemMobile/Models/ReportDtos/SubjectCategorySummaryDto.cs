using TutoringSystemMobile.Constans;
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
                    CategoryName = PickerConstans.OtherSubjectCategory;
                    break;
                case SubjectCategory.Math:
                    CategoryName = PickerConstans.Math;
                    break;
                case SubjectCategory.Informatics:
                    CategoryName = PickerConstans.Informatics;
                    break;
                case SubjectCategory.ForeignLanguage:
                    CategoryName = PickerConstans.ForeignLanguage;
                    break;
                case SubjectCategory.NativeLanguage:
                    CategoryName = PickerConstans.NativeLanguage;
                    break;
                case SubjectCategory.Physics:
                    CategoryName = PickerConstans.Physics;
                    break;
                case SubjectCategory.Biology:
                    CategoryName = PickerConstans.Biology;
                    break;
                case SubjectCategory.Chemistry:
                    CategoryName = PickerConstans.Chemistry;
                    break;
                case SubjectCategory.Music:
                    CategoryName = PickerConstans.Music;
                    break;
                case SubjectCategory.Geography:
                    CategoryName = PickerConstans.Geography;
                    break;
                default:
                    break;
            }
        }
    }
}