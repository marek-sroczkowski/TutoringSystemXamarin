using System.Collections.Generic;
using System.Windows.Input;
using TutoringSystemMobile.Commands.SubjectCommands;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.SubjectViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class EditSubjectViewModel : BaseViewModel
    {
        private long id;
        private string name;
        private string description;
        private SubjectCategory category;
        private SubjectPlace place;
        private string selectedCategory;
        private string selectedPlace;

        public long Id
        {
            get => id;
            set
            {
                id = value;
                LoadSubjectById(id);
            }
        }

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

        public List<string> Categories { get; set; }
        public List<string> Places { get; set; }

        public ICommand EditSubjectCommand { get; }

        private readonly ISubjectService subjectService;

        public EditSubjectViewModel()
        {
            subjectService = DependencyService.Get<ISubjectService>();
            EditSubjectCommand = new EditSubjectCommand(this, subjectService);
            SetCategories();
            SetPlaces();
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

        private async void LoadSubjectById(long subjectId)
        {
            var subject = await subjectService.GetSubjectByIdAsync(subjectId);

            Name = subject.Name;
            Description = subject.Description;
            Category = subject.Category;
            Place = subject.Place;

            SetSelectedCategory();
            SetSelectedPlace();
        }

        private void SetSelectedCategory()
        {
            switch (Category)
            {
                case SubjectCategory.Other:
                    SelectedCategory = PickerConstans.OtherSubjectCategory;
                    break;
                case SubjectCategory.Math:
                    SelectedCategory = PickerConstans.Math;
                    break;
                case SubjectCategory.Informatics:
                    SelectedCategory = PickerConstans.Informatics;
                    break;
                case SubjectCategory.ForeignLanguage:
                    SelectedCategory = PickerConstans.ForeignLanguage;
                    break;
                case SubjectCategory.NativeLanguage:
                    SelectedCategory = PickerConstans.NativeLanguage;
                    break;
                case SubjectCategory.Physics:
                    SelectedCategory = PickerConstans.Physics;
                    break;
                case SubjectCategory.Biology:
                    SelectedCategory = PickerConstans.Biology;
                    break;
                case SubjectCategory.Chemistry:
                    SelectedCategory = PickerConstans.Chemistry;
                    break;
                case SubjectCategory.Music:
                    SelectedCategory = PickerConstans.Music;
                    break;
                case SubjectCategory.Geography:
                    SelectedCategory = PickerConstans.Geography;
                    break;
                default:
                    break;
            }
        }

        private void SetSelectedPlace()
        {
            switch (Place)
            {
                case SubjectPlace.AtTutor:
                    SelectedPlace = PickerConstans.AtTutor;
                    break;
                case SubjectPlace.AtStudent:
                    SelectedPlace = PickerConstans.AtStudent;
                    break;
                case SubjectPlace.Online:
                    SelectedPlace = PickerConstans.Online;
                    break;
                case SubjectPlace.AtTutorAndAtStudent:
                    SelectedPlace = PickerConstans.AtTutorAndAtStudent;
                    break;
                case SubjectPlace.AtTutorAndOnline:
                    SelectedPlace = PickerConstans.AtTutorAndOnline;
                    break;
                case SubjectPlace.AtStudentAndOnline:
                    SelectedPlace = PickerConstans.AtStudentAndOnline;
                    break;
                case SubjectPlace.All:
                    SelectedPlace = PickerConstans.AllPlaces;
                    break;
            }
        }
    }
}