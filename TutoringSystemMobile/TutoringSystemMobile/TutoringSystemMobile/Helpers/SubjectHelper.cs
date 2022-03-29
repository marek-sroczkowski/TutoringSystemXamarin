using System.Collections.Generic;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Helpers
{
    public class SubjectHelper
    {
        public static string GetPlace(SubjectPlace place)
        {
            return place switch
            {
                SubjectPlace.AtTutor => PickerConstans.AtTutor,
                SubjectPlace.AtStudent => PickerConstans.AtStudent,
                SubjectPlace.Online => PickerConstans.Online,
                SubjectPlace.AtTutorAndAtStudent => PickerConstans.AtTutorAndAtStudent,
                SubjectPlace.AtTutorAndOnline => PickerConstans.AtTutorAndOnline,
                SubjectPlace.AtStudentAndOnline => PickerConstans.AtStudentAndOnline,
                SubjectPlace.All => PickerConstans.AllPlaces,
                _ => PickerConstans.AllPlaces,
            };
        }

        public static SubjectCategory GetCategory(string category)
        {
            return category switch
            {
                PickerConstans.OtherSubjectCategory => SubjectCategory.Other,
                PickerConstans.Math => SubjectCategory.Math,
                PickerConstans.Informatics => SubjectCategory.Informatics,
                PickerConstans.ForeignLanguage => SubjectCategory.ForeignLanguage,
                PickerConstans.NativeLanguage => SubjectCategory.NativeLanguage,
                PickerConstans.Physics => SubjectCategory.Physics,
                PickerConstans.Biology => SubjectCategory.Biology,
                PickerConstans.Chemistry => SubjectCategory.Chemistry,
                PickerConstans.Music => SubjectCategory.Music,
                PickerConstans.Geography => SubjectCategory.Geography
            };
        }

        public static string GetCategory(SubjectCategory category)
        {
            return category switch
            {
                SubjectCategory.Math => PickerConstans.Math,
                SubjectCategory.Informatics => PickerConstans.Informatics,
                SubjectCategory.ForeignLanguage => PickerConstans.ForeignLanguage,
                SubjectCategory.NativeLanguage => PickerConstans.NativeLanguage,
                SubjectCategory.Physics => PickerConstans.Physics,
                SubjectCategory.Biology => PickerConstans.Biology,
                SubjectCategory.Chemistry => PickerConstans.Chemistry,
                SubjectCategory.Music => PickerConstans.Music,
                SubjectCategory.Geography => PickerConstans.Geography,
                _ => PickerConstans.OtherSubjectCategory,
            };
        }

        public static List<string> GetCategories()
        {
            return new List<string>
            {
                PickerConstans.OtherSubjectCategory,
                PickerConstans.Math,
                PickerConstans.Informatics,
                PickerConstans.ForeignLanguage,
                PickerConstans.NativeLanguage,
                PickerConstans.Physics,
                PickerConstans.Biology,
                PickerConstans.Chemistry,
                PickerConstans.Music,
                PickerConstans.Geography,
            };
        }

        public static List<string> GetPlaces()
        {
            return new List<string>
            {
                PickerConstans.AllPlaces,
                PickerConstans.AtTutor,
                PickerConstans.AtStudent,
                PickerConstans.Online,
                PickerConstans.AtTutorAndAtStudent,
                PickerConstans.AtTutorAndOnline,
                PickerConstans.AtStudentAndOnline,
            };
        }
    }
}