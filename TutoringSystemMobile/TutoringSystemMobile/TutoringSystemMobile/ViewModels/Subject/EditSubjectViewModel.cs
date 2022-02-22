using Rg.Plugins.Popup.Services;
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

        public List<string> Categories { get; set; }
        public List<string> Places { get; set; }

        public Command EditSubjectCommand { get; }

        private readonly ISubjectService subjectService;

        public EditSubjectViewModel()
        {
            MessagingCenter.Subscribe<SubjectPlaceViewModel>(this, MessagingCenterConstans.SubjectPlaceSelected, (sender) =>
            {
                Place = sender.Place;
            });
            subjectService = DependencyService.Get<ISubjectService>();
            EditSubjectCommand = new Command(async () => await OnEditSubject(), CanEditSubject);
            PropertyChanged += (_, __) => EditSubjectCommand.ChangeCanExecute();
            SetCategories();
            SetPlaces();
        }

        public async Task OnSelectPlace()
        {
            await PopupNavigation.Instance.PushAsync(new SubjectPlacePopupPage(Place));
        }

        private async Task OnEditSubject()
        {
            IsBusy = true;
            bool updated = await subjectService.UpdateSubjectAsync(new UpdatedSubjectDto(Id, Name, Description, Place.Value, Category));
            IsBusy = false;

            if (updated)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.Updated);
                await Shell.Current.GoToAsync($"//{nameof(SubjectsTutorPage)}/{nameof(SubjectDetailsTutorPage)}?{nameof(Id)}={Id}");
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }

        public bool CanEditSubject()
        {
            return !Name.IsEmpty() &&
                !SelectedCategory.IsEmpty() &&
                !SelectedPlace.IsEmpty() &&
                !IsBusy;
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