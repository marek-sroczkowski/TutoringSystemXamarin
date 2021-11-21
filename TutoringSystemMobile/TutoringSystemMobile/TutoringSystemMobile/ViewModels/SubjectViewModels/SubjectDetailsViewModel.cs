using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.SubjectViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class SubjectDetailsViewModel : BaseViewModel
    {
        private long id;
        private string name;
        private string description;
        private SubjectPlace place;
        private SubjectCategory category;
        private string categoryPl;
        private string placePl;

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
        public SubjectPlace Place { get => place; set => SetValue(ref place, value); }
        public SubjectCategory Category { get => category; set => SetValue(ref category, value); }
        public string CategoryPl { get => categoryPl; set => SetValue(ref categoryPl, value); }
        public string PlacePl { get => placePl; set => SetValue(ref placePl, value); }

        public Command EditSubjectCommand { get; }
        public Command RemoveSubjectCommand { get; }

        private readonly ISubjectService subjectService;

        public SubjectDetailsViewModel()
        {
            subjectService = DependencyService.Get<ISubjectService>();
            EditSubjectCommand = new Command(async () => await OnRedirectToSubjectEditPage());
            RemoveSubjectCommand = new Command(async () => await OnRemoveRequest());
        }

        private async void LoadSubjectById(long subjectId)
        {
            IsBusy = true;
            var subject = await subjectService.GetSubjectByIdAsync(subjectId);

            Name = subject.Name;
            Description = subject.Description;
            Place = subject.Place;
            Category = subject.Category;
            SetCategoryPl();
            SetPlacePl();

            IsBusy = false;
        }

        private void SetCategoryPl()
        {
            switch (Category)
            {
                case SubjectCategory.Other:
                    CategoryPl = "Inny";
                    break;
                case SubjectCategory.Math:
                    CategoryPl = "Matematyka";
                    break;
                case SubjectCategory.Informatics:
                    CategoryPl = "Informatyka";
                    break;
                case SubjectCategory.ForeignLanguage:
                    CategoryPl = "Język obcy";
                    break;
                case SubjectCategory.NativeLanguage:
                    CategoryPl = "Język polski";
                    break;
                case SubjectCategory.Physics:
                    CategoryPl = "Fizyka";
                    break;
                case SubjectCategory.Biology:
                    CategoryPl = "Biologia";
                    break;
                case SubjectCategory.Chemistry:
                    CategoryPl = "Chemia";
                    break;
                case SubjectCategory.Music:
                    CategoryPl = "Muzyka";
                    break;
            }
        }

        private void SetPlacePl()
        {
            switch (Place)
            {
                case SubjectPlace.AtTutor:
                    PlacePl = "U nauczyciela";
                    break;
                case SubjectPlace.AtStudent:
                    PlacePl = "U ucznia";
                    break;
                case SubjectPlace.Online:
                    PlacePl = "Online";
                    break;
                case SubjectPlace.AtTutorAndAtStudent:
                    PlacePl = "U nauczyciela / ucznia";
                    break;
                case SubjectPlace.AtTutorAndOnline:
                    PlacePl = "U nauczyciela / online";
                    break;
                case SubjectPlace.AtStudentAndOnline:
                    PlacePl = "U ucznia / online";
                    break;
                case SubjectPlace.All:
                    PlacePl = "Dowolne";
                    break;
            }
        }

        private async Task OnRedirectToSubjectEditPage()
        {
            await Shell.Current.GoToAsync($"{nameof(EditSubjectTutorPage)}?{nameof(EditSubjectViewModel.Id)}={Id}");
        }

        private async Task OnRemoveRequest()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Uwaga!", "Czy na pewno chcesz usunąć ten przedmiot?", "Tak", "Nie");
            if (result)
                await RemoveSubjectAsync();
        }

        private async Task RemoveSubjectAsync()
        {
            var removed = await subjectService.DeleteSubjectAsync(Id);
            if (removed)
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Usunięto przedmiot!");
                await Shell.Current.GoToAsync($"//{nameof(SubjectsTutorPage)}");
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast("Błąd! Spróbuj później!");
            }
        }
    }
}