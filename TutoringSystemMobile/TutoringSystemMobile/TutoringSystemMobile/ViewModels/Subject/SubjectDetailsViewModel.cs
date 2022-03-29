using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Subject
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
        public string DisplayedCategory { get => categoryPl; set => SetValue(ref categoryPl, value); }
        public string DisplayedPlace { get => placePl; set => SetValue(ref placePl, value); }

        public Command EditSubjectCommand { get; }
        public Command RemoveSubjectCommand { get; }

        private readonly ISubjectService subjectService = DependencyService.Get<ISubjectService>();

        public SubjectDetailsViewModel()
        {
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
            DisplayedCategory = SubjectHelper.GetCategory(Category);
            DisplayedPlace = SubjectHelper.GetPlace(Place);

            IsBusy = false;
        }

        private async Task OnRedirectToSubjectEditPage()
        {
            await Shell.Current.GoToAsync($"{nameof(EditSubjectTutorPage)}?{nameof(EditSubjectViewModel.Id)}={Id}");
        }

        private async Task OnRemoveRequest()
        {
            var result = await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, AlertConstans.ConfirmationSubjectDeletion, GeneralConstans.Yes, GeneralConstans.No);
            if (result)
            {
                await RemoveSubjectAsync();
            }
        }

        private async Task RemoveSubjectAsync()
        {
            var removed = await subjectService.DeleteSubjectAsync(Id);
            if (removed)
            {
                ToastHelper.MakeLongToast(ToastConstans.SubjectRemoved);
                await Shell.Current.GoToAsync($"//{nameof(SubjectsTutorPage)}");
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }
    }
}