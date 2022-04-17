using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Dtos.Subject;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;
using TutoringSystemMobile.Helpers;

namespace TutoringSystemMobile.ViewModels.Subject
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class EditSubjectViewModel : BaseViewModel
    {
        private long id;
        private string name;
        private string description;
        private SubjectCategory category;
        private SubjectPlace? place = null;
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
        public string SelectedPlace { get => selectedPlace; set => SetValue(ref selectedPlace, value); }
        public SubjectPlace? Place
        {
            get => place;
            set
            {
                SetValue(ref place, value);
                SelectedPlace = SubjectHelper.GetPlace(place.Value);
            }
        }

        public string SelectedCategory
        {
            get => selectedCategory;
            set
            {
                SetValue(ref selectedCategory, value);
                Category = SubjectHelper.GetCategory(selectedCategory);
            }
        }

        public List<string> Categories { get; set; }
        public List<string> Places { get; set; }

        public Command EditSubjectCommand { get; }

        private readonly ISubjectService subjectService = DependencyService.Get<ISubjectService>();

        public EditSubjectViewModel()
        {
            MessagingCenter.Subscribe<SubjectPlaceViewModel>(this, MessagingCenterConstans.SubjectPlaceSelected, (sender) =>
            {
                Place = sender.Place;
            });

            EditSubjectCommand = new Command(async () => await OnEditSubject(), CanEditSubject);
            PropertyChanged += (_, __) => EditSubjectCommand.ChangeCanExecute();
            Categories = SubjectHelper.GetCategories();
            Places = SubjectHelper.GetPlaces();
        }

        public async Task OnSelectPlace()
        {
            await PopupNavigation.Instance.PushAsync(new SubjectPlacePopupPage(Place));
        }

        private async Task OnEditSubject()
        {
            IsBusy = true;
            var result = await subjectService.UpdateSubjectAsync(new UpdatedSubjectDto(Id, Name, Description, Place.Value, Category));
            IsBusy = false;

            if (result == 1)
            {
                ToastHelper.MakeLongToast(ToastConstans.Updated);
                await Shell.Current.GoToAsync($"//{nameof(SubjectsTutorPage)}/{nameof(SubjectDetailsTutorPage)}?{nameof(Id)}={Id}");
            }
            else if (result == -2)
            {
                ToastHelper.MakeLongToast(ToastConstans.SubjectNameTaken);
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }

        public bool CanEditSubject()
        {
            return !Name.IsEmpty()
                && !SelectedCategory.IsEmpty()
                && !SelectedPlace.IsEmpty()
                && !IsBusy;
        }

        private async void LoadSubjectById(long subjectId)
        {
            var subject = await subjectService.GetSubjectByIdAsync(subjectId);

            Name = subject.Name;
            Description = subject.Description;
            Category = subject.Category;
            Place = subject.Place;

            SelectedCategory = SubjectHelper.GetCategory(Category);
            SelectedPlace = SubjectHelper.GetPlace(Place.Value);
        }
    }
}