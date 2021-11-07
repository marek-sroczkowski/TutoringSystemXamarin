using System.Collections.Generic;
using System.Windows.Input;
using TutoringSystemMobile.Commands.SubjectCommands;
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
                    case "Inna":
                        Category = SubjectCategory.Other;
                        break;
                    case "Matematyka":
                        Category = SubjectCategory.Math;
                        break;
                    case "Informatyka":
                        Category = SubjectCategory.Informatics;
                        break;
                    case "Język obcy":
                        Category = SubjectCategory.ForeignLanguage;
                        break;
                    case "Język polski":
                        Category = SubjectCategory.NativeLanguage;
                        break;
                    case "Fizyka":
                        Category = SubjectCategory.Physics;
                        break;
                    case "Biologia":
                        Category = SubjectCategory.Biology;
                        break;
                    case "Chemia":
                        Category = SubjectCategory.Chemistry;
                        break;
                    case "Muzyka":
                        Category = SubjectCategory.Music;
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
                    case "U nauczyciela":
                        Place = SubjectPlace.AtTutor;
                        break;
                    case "U ucznia":
                        Place = SubjectPlace.AtStudent;
                        break;
                    case "Online":
                        Place = SubjectPlace.Online;
                        break;
                    case "U nauczyciela / ucznia":
                        Place = SubjectPlace.AtTutorAndAtStudent;
                        break;
                    case "U nauczyciela / online":
                        Place = SubjectPlace.AtTutorAndOnline;
                        break;
                    case "U ucznia / online":
                        Place = SubjectPlace.AtStudentAndOnline;
                        break;
                    case "Dowolne":
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
            Categories = new List<string> { "Inna", "Matematyka", "Informatyka", "Język obcy", "Język polski", "Fizyka", "Biologia", "Chemia", "Muzyka" };
            Places = new List<string> { "Dowolne", "U nauczyciela", "U ucznia", "Online", "U nauczyciela / ucznia", "U nauczyciela / online", "U ucznia / online" };
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
                    SelectedCategory = "Inna";
                    break;
                case SubjectCategory.Math:
                    SelectedCategory = "Matematyka";
                    break;
                case SubjectCategory.Informatics:
                    SelectedCategory = "Informatyka";
                    break;
                case SubjectCategory.ForeignLanguage:
                    SelectedCategory = "Język obcy";
                    break;
                case SubjectCategory.NativeLanguage:
                    SelectedCategory = "Język polski";
                    break;
                case SubjectCategory.Physics:
                    SelectedCategory = "Fizyka";
                    break;
                case SubjectCategory.Biology:
                    SelectedCategory = "Biologia";
                    break;
                case SubjectCategory.Chemistry:
                    SelectedCategory = "Chemia";
                    break;
                case SubjectCategory.Music:
                    SelectedCategory = "Muzyka";
                    break;
            }
        }

        private void SetSelectedPlace()
        {
            switch (Place)
            {
                case SubjectPlace.AtTutor:
                    SelectedPlace = "U nauczyciela";
                    break;
                case SubjectPlace.AtStudent:
                    SelectedPlace = "U ucznia";
                    break;
                case SubjectPlace.Online:
                    SelectedPlace = "Online";
                    break;
                case SubjectPlace.AtTutorAndAtStudent:
                    SelectedPlace = "U nauczyciela / ucznia";
                    break;
                case SubjectPlace.AtTutorAndOnline:
                    SelectedPlace = "U nauczyciela / online";
                    break;
                case SubjectPlace.AtStudentAndOnline:
                    SelectedPlace = "U ucznia / online";
                    break;
                case SubjectPlace.All:
                    SelectedPlace = "Dowolne";
                    break;
            }
        }
    }
}