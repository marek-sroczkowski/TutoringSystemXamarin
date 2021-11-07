using System.Collections.Generic;
using System.Windows.Input;
using TutoringSystemMobile.Commands.SubjectCommands;
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
                IsPlaceLabelVisible = true;
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

        public bool IsCategoryLabelVisible { get => isCategoryLabelVisible; set => SetValue(ref isCategoryLabelVisible, value); }
        public bool IsPlaceLabelVisible { get => isPlaceLabelVisible; set => SetValue(ref isPlaceLabelVisible, value); }

        public List<string> Categories { get; set; }
        public List<string> Places { get; set; }

        public ICommand AddNewSubjectCommand { get; }

        public NewSubjectViewModel()
        {
            AddNewSubjectCommand = new AddNewSubjectCommand(this, DependencyService.Get<ISubjectService>());
            Categories = new List<string> { "Inna", "Matematyka", "Informatyka", "Język obcy", "Język polski", "Fizyka", "Biologia", "Chemia", "Muzyka" };
            Places = new List<string> { "Dowolne", "U nauczyciela", "U ucznia", "Online", "U nauczyciela / ucznia", "U nauczyciela / online", "U ucznia / online" };
            IsCategoryLabelVisible = false;
            IsPlaceLabelVisible = false;
        }
    }
}