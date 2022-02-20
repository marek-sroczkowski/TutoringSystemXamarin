using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.ReservationDtos;
using TutoringSystemMobile.Models.StudentDtos;
using TutoringSystemMobile.Models.SubjectDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.ReservationViewModels
{
    [QueryProperty(nameof(StartDate), nameof(StartDate))]
    public class NewTutorReservationViewModel : BaseViewModel
    {
        private string cost;
        private DateTime startDate;
        private TimeSpan? startTime = null;
        private string duration;
        private string description;
        private StudentDto selectedStudent;
        private SubjectDto selectedSubject;
        private string selectedPlace;
        private bool isRecurring;
        private string selectedFrequency;
        private bool isCostVisible;
        private bool isPlaceVisible;
        private bool isStudentLabelVisible;
        private bool isCostLabelVisible;
        private bool isSubjectLabelVisible;
        private bool isPlaceLabelVisible;
        private bool isFrequencyLabelVisible;
        private bool isDurationLabelVisible;

        public ObservableCollection<StudentDto> DisplayedStudents { get; }
        public ObservableCollection<SubjectDto> DisplayedSubjects { get; }
        public ObservableCollection<string> SubjectPlaces { get; }
        public ObservableCollection<string> Frequencies { get; set; }
        public ObservableCollection<string> Durations { get; set; }

        public string Cost { get => cost; set => SetValue(ref cost, value); }
        public DateTime StartDate { get => startDate; set => SetValue(ref startDate, value); }
        public TimeSpan? StartTime { get => startTime; set => SetValue(ref startTime, value); }
        public string Description { get => description; set => SetValue(ref description, value); }
        public string Duration
        {
            get => duration;
            set
            {
                SetValue(ref duration, value);
                IsDurationLabelVisible = true;
            }
        }
        public StudentDto SelectedStudent
        {
            get => selectedStudent;
            set
            {
                SetValue(ref selectedStudent, value);
                if (int.TryParse(Duration, out int duration))
                {
                    Cost = $"{selectedStudent.HourlRate * (duration / 60.0)}";
                    IsCostLabelVisible = true;
                }
                IsCostVisible = true;
                IsStudentLabelVisible = true;
            }
        }
        public SubjectDto SelectedSubject
        {
            get => selectedSubject;
            set
            {
                SetValue(ref selectedSubject, value);
                GetPlaces();
                IsPlaceVisible = true;
                IsSubjectLabelVisible = true;
            }
        }
        public string SelectedPlace
        {
            get => selectedPlace;
            set
            {
                SetValue(ref selectedPlace, value);
                IsPlaceLabelVisible = true;
            }
        }
        public string SelectedFrequency
        {
            get => selectedFrequency;
            set
            {
                SetValue(ref selectedFrequency, value);
                IsFrequencyLabelVisible = true;
            }
        }

        public bool IsRecurring
        {
            get => isRecurring;
            set
            {
                SetValue(ref isRecurring, value);
                IsFrequencyLabelVisible = isRecurring && !string.IsNullOrEmpty(SelectedFrequency);
            }
        }
        public bool IsCostVisible { get => isCostVisible; set => SetValue(ref isCostVisible, value); }
        public bool IsPlaceVisible { get => isPlaceVisible; set => SetValue(ref isPlaceVisible, value); }
        public bool IsStudentLabelVisible { get => isStudentLabelVisible; set => SetValue(ref isStudentLabelVisible, value); }
        public bool IsCostLabelVisible { get => isCostLabelVisible; set => SetValue(ref isCostLabelVisible, value); }
        public bool IsSubjectLabelVisible { get => isSubjectLabelVisible; set => SetValue(ref isSubjectLabelVisible, value); }
        public bool IsPlaceLabelVisible { get => isPlaceLabelVisible; set => SetValue(ref isPlaceLabelVisible, value); }
        public bool IsFrequencyLabelVisible { get => isFrequencyLabelVisible; set => SetValue(ref isFrequencyLabelVisible, value); }
        public bool IsDurationLabelVisible { get => isDurationLabelVisible; set => SetValue(ref isDurationLabelVisible, value); }

        public Command PageAppearingCommand { get; }
        public Command AddNewReservationCommand { get; }

        public NewTutorReservationViewModel()
        {
            DisplayedStudents = new ObservableCollection<StudentDto>();
            DisplayedSubjects = new ObservableCollection<SubjectDto>();
            SubjectPlaces = new ObservableCollection<string>();
            SetFrequencies();
            SetDurations();
            PageAppearingCommand = new Command(async () => await OnAppearing());
            AddNewReservationCommand = new Command(async () => await OnAddNewReservation(), CanAddNewReservation);
            PropertyChanged += (_, __) => AddNewReservationCommand.ChangeCanExecute();
        }

        public bool CanAddNewReservation()
        {
            return StartTime.HasValue &&
                int.TryParse(Duration, out int duration) &&
                duration > 0 &&
                double.TryParse(Cost, out double cost) &&
                cost > 0 &&
                !string.IsNullOrEmpty(SelectedPlace) &&
                SelectedStudent != null &&
                SelectedSubject != null &&
                (!IsRecurring || !string.IsNullOrEmpty(SelectedFrequency));
        }

        private async Task OnAppearing()
        {
            await GetStudents();
            await GetSubjects();
        }

        private async Task OnAddNewReservation()
        {
            IsBusy = true;
            long newReservationId = await AddReservationAsync();
            IsBusy = false;

            if (newReservationId == -1)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.AddedReservation);
                await Shell.Current.GoToAsync($"{nameof(ReservationDetailsTutorPage)}?{nameof(TutorReservationDetailsViewModel.Id)}={newReservationId}&{nameof(TutorReservationDetailsViewModel.StartTime)}={StartTime}&{nameof(TutorReservationDetailsViewModel.StartDate)}={StartDate.ToShortDateString()}");
            }
        }

        private async Task<long> AddReservationAsync()
        {
            return IsRecurring
                ? await AddRecurringReservationAsync()
                : await AddSingleReservationAsync();
        }

        private async Task<long> AddSingleReservationAsync()
        {
            var newReservation = new NewTutorSingleReservationDto
            {
                Cost = double.Parse(Cost),
                Duration = int.Parse(Duration),
                StartTime = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Value.Hours, StartTime.Value.Minutes, StartTime.Value.Seconds),
                Description = Description,
                Place = GetPlace(),
                StudentId = SelectedStudent.Id,
                SubjectId = SelectedSubject.Id
            };

            return await DependencyService.Get<ISingleReservationService>().AddReservationByTutorAsync(newReservation);
        }

        private async Task<long> AddRecurringReservationAsync()
        {
            var newReservation = new NewTutorRecurringReservationDto
            {
                Cost = double.Parse(Cost),
                Duration = int.Parse(Duration),
                StartTime = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Value.Hours, StartTime.Value.Minutes, StartTime.Value.Seconds),
                Description = Description,
                Place = GetPlace(),
                Frequency = GetFrequency(),
                StudentId = SelectedStudent.Id,
                SubjectId = SelectedSubject.Id
            };

            return await DependencyService.Get<IRecurringReservationService>().AddReservationByTutorAsync(newReservation);
        }

        private void SetFrequencies()
        {
            Frequencies = new ObservableCollection<string>
            {
                PickerConstans.WeeklyReservation,
                PickerConstans.OnceTwoWeeksReservation,
                PickerConstans.MonthlyReservation
            };
        }

        private void SetDurations()
        {
            Durations = new ObservableCollection<string>
            {
                "30", "45", "60", "90", "120", "135", "150", "180", "210", "225", "240"
            };
        }

        private async Task GetStudents()
        {
            var students = await DependencyService.Get<IStudentService>().GetStudentsAsync();
            students.ToList().ForEach(student => DisplayedStudents.Add(student));
        }

        private async Task GetSubjects()
        {
            var subjects = await DependencyService.Get<ISubjectService>().GetSubjectsAsync();
            subjects.ToList().ForEach(subject => DisplayedSubjects.Add(subject));
        }

        private void GetPlaces()
        {
            SubjectPlaces.Clear();

            if (SelectedSubject.Place.ToString().Contains(GeneralConstans.TutorEn))
                SubjectPlaces.Add(PickerConstans.AtTutor);

            if (SelectedSubject.Place.ToString().Contains(GeneralConstans.StudentEn))
                SubjectPlaces.Add(PickerConstans.AtStudent);

            if (SelectedSubject.Place.ToString().Contains(GeneralConstans.Online))
                SubjectPlaces.Add(PickerConstans.Online);
        }

        private ReservationPlace GetPlace()
        {
            return SelectedPlace switch
            {
                PickerConstans.AtTutor => ReservationPlace.AtTutor,
                PickerConstans.AtStudent => ReservationPlace.AtStudent,
                PickerConstans.Online => ReservationPlace.Online,
                _ => ReservationPlace.AtTutor
            };
        }

        private ReservationFrequency GetFrequency()
        {
            return SelectedFrequency switch
            {
                PickerConstans.WeeklyReservation => ReservationFrequency.Weekly,
                PickerConstans.OnceTwoWeeksReservation => ReservationFrequency.OnceTwoWeeks,
                PickerConstans.MonthlyReservation => ReservationFrequency.Monthly,
                _ => ReservationFrequency.Weekly
            };
        }
    }
}
