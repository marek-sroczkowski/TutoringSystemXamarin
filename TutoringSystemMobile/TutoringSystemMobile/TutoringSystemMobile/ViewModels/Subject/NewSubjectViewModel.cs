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
    public class NewSubjectViewModel : BaseViewModel
    {
        private string name;
        private string description;
        private SubjectCategory category;
        private SubjectPlace? place = null;
        private string selectedCategory;
        private string selectedPlace;
        private bool isCategoryLabelVisible;
        private bool isPlaceLabelVisible;

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
                IsPlaceLabelVisible = true;
                SelectedPlace = SubjectHelper.GetPlace(place.Value);
            }
        }
        public string SelectedCategory
        {
            get => selectedCategory;
            set
            {
                SetValue(ref selectedCategory, value);
                IsCategoryLabelVisible = true;
                Category = SubjectHelper.GetCategory(selectedCategory);
            }
        }

        public bool IsCategoryLabelVisible { get => isCategoryLabelVisible; set => SetValue(ref isCategoryLabelVisible, value); }
        public bool IsPlaceLabelVisible { get => isPlaceLabelVisible; set => SetValue(ref isPlaceLabelVisible, value); }

        public List<string> Categories { get; set; }
        public List<string> Places { get; set; }

        public Command AddNewSubjectCommand { get; }

        private readonly ISubjectService subjectService = DependencyService.Get<ISubjectService>();

        public NewSubjectViewModel()
        {
            MessagingCenter.Subscribe<SubjectPlaceViewModel>(this, MessagingCenterConstans.SubjectPlaceSelected, (sender) =>
            {
                Place = sender.Place;
            });

            AddNewSubjectCommand = new Command(async () => await OnAddNewSubject(), CanAddNewSubject);
            PropertyChanged += (_, __) => AddNewSubjectCommand.ChangeCanExecute();
            Categories = SubjectHelper.GetCategories();
            Places = SubjectHelper.GetPlaces();
            IsCategoryLabelVisible = false;
            IsPlaceLabelVisible = false;
        }

        public async Task OnSelectPlace()
        {
            await PopupNavigation.Instance.PushAsync(new SubjectPlacePopupPage(Place));
        }

        private async Task OnAddNewSubject()
        {
            IsBusy = true;
            var subject = new NewSubjectDto(Name, Description, Place.Value, Category);
            long newOrderId = await subjectService.AddSubjectAsync(subject);
            IsBusy = false;

            if (newOrderId == -1)
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
            else if(newOrderId == -2)
            {
                ToastHelper.MakeLongToast(ToastConstans.SubjectNameTaken);
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.AddedSubject);
                await Shell.Current.GoToAsync($"//{nameof(SubjectsTutorPage)}/{nameof(SubjectDetailsTutorPage)}?{nameof(SubjectDetailsViewModel.Id)}={newOrderId}");
            }
        }

        public bool CanAddNewSubject()
        {
            return !Name.IsEmpty()
                && !SelectedCategory.IsEmpty()
                && !SelectedPlace.IsEmpty()
                && Place.HasValue
                && !IsBusy;
        }
    }
}