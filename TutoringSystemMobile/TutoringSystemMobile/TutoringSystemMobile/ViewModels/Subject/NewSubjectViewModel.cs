using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Dtos.Subject;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

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
                switch (place)
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

        public bool IsCategoryLabelVisible { get => isCategoryLabelVisible; set => SetValue(ref isCategoryLabelVisible, value); }
        public bool IsPlaceLabelVisible { get => isPlaceLabelVisible; set => SetValue(ref isPlaceLabelVisible, value); }

        public List<string> Categories { get; set; }
        public List<string> Places { get; set; }

        public Command AddNewSubjectCommand { get; }

        public NewSubjectViewModel()
        {
            MessagingCenter.Subscribe<SubjectPlaceViewModel>(this, MessagingCenterConstans.SubjectPlaceSelected, (sender) =>
            {
                Place = sender.Place;
            });

            AddNewSubjectCommand = new Command(async () => await OnAddNewSubject(), CanAddNewSubject);
            PropertyChanged += (_, __) => AddNewSubjectCommand.ChangeCanExecute();
            SetCategories();
            SetPlaces();
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
            long newOrderId = await DependencyService.Get<ISubjectService>().AddSubjectAsync(new NewSubjectDto(Name, Description, Place.Value, Category));
            IsBusy = false;

            if (newOrderId == -1)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
            else if(newOrderId == -2)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.SubjectNameTaken);
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.AddedSubject);
                await Shell.Current.GoToAsync($"//{nameof(SubjectsTutorPage)}/{nameof(SubjectDetailsTutorPage)}?{nameof(SubjectDetailsViewModel.Id)}={newOrderId}");
            }
        }

        public bool CanAddNewSubject()
        {
            return !Name.IsEmpty() &&
                   !SelectedCategory.IsEmpty() &&
                   !SelectedPlace.IsEmpty() &&
                   Place.HasValue &&
                   !IsBusy;
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