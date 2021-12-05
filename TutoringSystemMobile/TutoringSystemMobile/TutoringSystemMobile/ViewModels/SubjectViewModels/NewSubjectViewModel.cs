using System.Collections.Generic;
using System.Windows.Input;
using TutoringSystemMobile.Commands.SubjectCommands;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.SubjectViewModels
{
    public class NewSubjectViewModel : BaseViewModel
    {
        private string name;
        private string description;
        private SubjectCategory category;
        private SubjectPlace place;
        private string selectedCategory;
        private string selectedPlace;
        private bool isCategoryLabelVisible;
        private bool isPlaceLabelVisible;

        public string Name { get => name; set => SetValue(ref name, value); }
        public string Description { get => description; set => SetValue(ref description, value); }
        public SubjectCategory Category { get => category; set => SetValue(ref category, value); }
        public SubjectPlace Place { get => place; set => SetValue(ref place, value); }

        public string SelectedCategory
        {
            get => selectedCategory;
            set
            {
                SetValue(ref selectedCategory, value);
                IsCategoryLabelVisible = true;
                switch (selectedCategory)
                {
                    case PickerConstans.OtherSubjectCategory:
                        Category = SubjectCategory.Other;
                        break;
                    case PickerConstans.Math:
                        Category = SubjectCategory.Math;
                        break;
                    case PickerConstans.Informatics:
                        Category = SubjectCategory.Informatics;
                        break;
                    case PickerConstans.ForeignLanguage:
                        Category = SubjectCategory.ForeignLanguage;
                        break;
                    case PickerConstans.NativeLanguage:
                        Category = SubjectCategory.NativeLanguage;
                        break;
                    case PickerConstans.Physics:
                        Category = SubjectCategory.Physics;
                        break;
                    case PickerConstans.Biology:
                        Category = SubjectCategory.Biology;
                        break;
                    case PickerConstans.Chemistry:
                        Category = SubjectCategory.Chemistry;
                        break;
                    case PickerConstans.Music:
                        Category = SubjectCategory.Music;
                        break;
                    case PickerConstans.Geography:
                        Category = SubjectCategory.Geography;
                        break;
                }
            }
        }
        public string SelectedPlace
        {
            get => selectedPlace;
            set
            {
                SetValue(ref selectedPlace, value);
                IsPlaceLabelVisible = true;
                switch (selectedPlace)
                {
                    case PickerConstans.AtTutor:
                        Place = SubjectPlace.AtTutor;
                        break;
                    case PickerConstans.AtStudent:
                        Place = SubjectPlace.AtStudent;
                        break;
                    case PickerConstans.Online:
                        Place = SubjectPlace.Online;
                        break;
                    case PickerConstans.AtTutorAndAtStudent:
                        Place = SubjectPlace.AtTutorAndAtStudent;
                        break;
                    case PickerConstans.AtTutorAndOnline:
                        Place = SubjectPlace.AtTutorAndOnline;
                        break;
                    case PickerConstans.AtStudentAndOnline:
                        Place = SubjectPlace.AtStudentAndOnline;
                        break;
                    case PickerConstans.AllPlaces:
                        Place = SubjectPlace.All;
                        break;
                }
            }
        }

        public bool IsCategoryLabelVisible { get => isCategoryLabelVisible; set => SetValue(ref isCategoryLabelVisible, value); }
        public bool IsPlaceLabelVisible { get => isPlaceLabelVisible; set => SetValue(ref isPlaceLabelVisible, value); }

        public List<string> Categories { get; set; }
        public List<string> Places { get; set; }

        public ICommand AddNewSubjectCommand { get; }

        public NewSubjectViewModel()
        {
            AddNewSubjectCommand = new AddNewSubjectCommand(this, DependencyService.Get<ISubjectService>());
            SetCategories();
            SetPlaces();
            IsCategoryLabelVisible = false;
            IsPlaceLabelVisible = false;
        }

        private void SetCategories()
        {
            Categories = new List<string>
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

        private void SetPlaces()
        {
            Places = new List<string>
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