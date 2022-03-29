using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Dtos.Reservation;
using TutoringSystemMobile.Models.Dtos.Student;
using TutoringSystemMobile.Models.Dtos.Subject;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;
using TutoringSystemMobile.Helpers;

namespace TutoringSystemMobile.ViewModels.Reservation
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

        private readonly IStudentService studentService = DependencyService.Get<IStudentService>();
        private readonly ISubjectService subjectService = DependencyService.Get<ISubjectService>();
        private readonly ISingleReservationService singleReservationService = DependencyService.Get<ISingleReservationService>();
        private readonly IRecurringReservationService recurringReservationService = DependencyService.Get<IRecurringReservationService>();

        public NewTutorReservationViewModel()
        {
            DisplayedStudents = new ObservableCollection<StudentDto>();
            DisplayedSubjects = new ObservableCollection<SubjectDto>();
            SubjectPlaces = new ObservableCollection<string>();
            Frequencies = ReservationHelper.GetFrequencies();
            Durations = ReservationHelper.GetDurations();
            PageAppearingCommand = new Command(async () => await OnAppearing());
            AddNewReservationCommand = new Command(async () => await OnAddNewReservation(), CanAddNewReservation);
            PropertyChanged += (_, __) => AddNewReservationCommand.ChangeCanExecute();
        }

        public bool CanAddNewReservation()
        {
            return StartTime.HasValue
                && int.TryParse(Duration, out int duration)
                && duration > 0
                && double.TryParse(Cost, out double cost)
                && cost > 0
                && !string.IsNullOrEmpty(SelectedPlace)
                && SelectedStudent != null
                && SelectedSubject != null
                && (!IsRecurring || !string.IsNullOrEmpty(SelectedFrequency));
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
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.AddedReservation);
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
                Place = ReservationHelper.GetPlace(SelectedPlace),
                StudentId = SelectedStudent.Id,
                SubjectId = SelectedSubject.Id
            };

            return await singleReservationService.AddReservationByTutorAsync(newReservation);
        }

        private async Task<long> AddRecurringReservationAsync()
        {
            var newReservation = new NewTutorRecurringReservationDto
            {
                Cost = double.Parse(Cost),
                Duration = int.Parse(Duration),
                StartTime = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Value.Hours, StartTime.Value.Minutes, StartTime.Value.Seconds),
                Description = Description,
                Place = ReservationHelper.GetPlace(SelectedPlace),
                Frequency = ReservationHelper.GetFrequency(SelectedFrequency),
                StudentId = SelectedStudent.Id,
                SubjectId = SelectedSubject.Id
            };

            return await recurringReservationService.AddReservationByTutorAsync(newReservation);
        }

        private async Task GetStudents()
        {
            var students = await studentService.GetStudentsAsync();
            students.ToList().ForEach(student => DisplayedStudents.Add(student));
        }

        private async Task GetSubjects()
        {
            var subjects = await subjectService.GetSubjectsAsync();
            subjects.ToList().ForEach(subject => DisplayedSubjects.Add(subject));
        }

        private void GetPlaces()
        {
            SubjectPlaces.Clear();

            if (SelectedSubject.Place.ToString().Contains(GeneralConstans.TutorEn))
            {
                SubjectPlaces.Add(PickerConstans.AtTutor);
            }

            if (SelectedSubject.Place.ToString().Contains(GeneralConstans.StudentEn))
            {
                SubjectPlaces.Add(PickerConstans.AtStudent);
            }

            if (SelectedSubject.Place.ToString().Contains(GeneralConstans.Online))
            {
                SubjectPlaces.Add(PickerConstans.Online);
            }
        }
    }
}
