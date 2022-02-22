using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
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
            SetCategory();
            SetPlace();

            IsBusy = false;
        }

        private void SetCategory()
        {
            switch (Category)
            {
                case SubjectCategory.Other:
                    DisplayedCategory = PickerConstans.OtherSubjectCategory;
                    break;
                case SubjectCategory.Math:
                    DisplayedCategory = PickerConstans.Math;
                    break;
                case SubjectCategory.Informatics:
                    DisplayedCategory = PickerConstans.Informatics;
                    break;
                case SubjectCategory.ForeignLanguage:
                    DisplayedCategory = PickerConstans.ForeignLanguage;
                    break;
                case SubjectCategory.NativeLanguage:
                    DisplayedCategory = PickerConstans.NativeLanguage;
                    break;
                case SubjectCategory.Physics:
                    DisplayedCategory = PickerConstans.Physics;
                    break;
                case SubjectCategory.Biology:
                    DisplayedCategory = PickerConstans.Biology;
                    break;
                case SubjectCategory.Chemistry:
                    DisplayedCategory = PickerConstans.Chemistry;
                    break;
                case SubjectCategory.Music:
                    DisplayedCategory = PickerConstans.Music;
                    break;
                case SubjectCategory.Geography:
                    DisplayedCategory = PickerConstans.Geography;
                    break;
                default:
                    break;
            }
        }

        private void SetPlace()
        {
            switch (Place)
            {
                case SubjectPlace.AtTutor:
                    DisplayedPlace = PickerConstans.AtTutor;
                    break;
                case SubjectPlace.AtStudent:
                    DisplayedPlace = PickerConstans.AtStudent;
                    break;
                case SubjectPlace.Online:
                    DisplayedPlace = PickerConstans.Online;
                    break;
                case SubjectPlace.AtTutorAndAtStudent:
                    DisplayedPlace = PickerConstans.AtTutorAndAtStudent;
                    break;
                case SubjectPlace.AtTutorAndOnline:
                    DisplayedPlace = PickerConstans.AtTutorAndOnline;
                    break;
                case SubjectPlace.AtStudentAndOnline:
                    DisplayedPlace = PickerConstans.AtStudentAndOnline;
                    break;
                case SubjectPlace.All:
                    DisplayedPlace = PickerConstans.AllPlaces;
                    break;
            }
        }

        private async Task OnRedirectToSubjectEditPage()
        {
            await Shell.Current.GoToAsync($"{nameof(EditSubjectTutorPage)}?{nameof(EditSubjectViewModel.Id)}={Id}");
        }

        private async Task OnRemoveRequest()
        {
            var result = await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, AlertConstans.ConfirmationSubjectDeletion, GeneralConstans.Yes, GeneralConstans.No);
            if (result)
                await RemoveSubjectAsync();
        }

        private async Task RemoveSubjectAsync()
        {
            var removed = await subjectService.DeleteSubjectAsync(Id);
            if (removed)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.SubjectRemoved);
                await Shell.Current.GoToAsync($"//{nameof(SubjectsTutorPage)}");
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }
    }
}