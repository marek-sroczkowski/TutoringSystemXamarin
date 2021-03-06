using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Dtos.Images;
using TutoringSystemMobile.Models.Dtos.Reservation;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.SQLite;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.Student;
using TutoringSystemMobile.Views;
using Xamarin.Forms;
using TutoringSystemMobile.Helpers;

namespace TutoringSystemMobile.ViewModels.Reservation
{
    [QueryProperty(nameof(Id), nameof(Id))]
    [QueryProperty(nameof(StartTime), nameof(StartTime))]
    [QueryProperty(nameof(StartDate), nameof(StartDate))]
    public class TutorReservationDetailsViewModel : BaseViewModel
    {
        private DateTime startTime;
        private DateTime endTime;
        private long studentId;
        private ReservationType reservationType;

        private long id;
        private string cost;
        private string lessonTime;
        private string duration;
        private string description;
        private string place;
        private string type;
        private string subjectName;
        private string student;
        private string paymentStatus;
        private ImageSource image;
        private string lessonDate;
        private DateTime startDate;

        public long Id
        {
            get => id;
            set
            {
                id = value;
                LoadReservationById(id);
            }
        }
        public string Cost { get => cost; set => SetValue(ref cost, value); }
        public string LessonTime { get => lessonTime; set => SetValue(ref lessonTime, value); }
        public string LessonDate { get => lessonDate; set => SetValue(ref lessonDate, value); }
        public string Duration { get => duration; set => SetValue(ref duration, value); }
        public string Description { get => description; set => SetValue(ref description, value); }
        public string Place { get => place; set => SetValue(ref place, value); }
        public string Type { get => type; set => SetValue(ref type, value); }
        public string SubjectName { get => subjectName; set => SetValue(ref subjectName, value); }
        public string Student { get => student; set => SetValue(ref student, value); }
        public string PaymentStatus { get => paymentStatus; set => SetValue(ref paymentStatus, value); }
        public ImageSource Image { get => image; set => SetValue(ref image, value); }
        public DateTime StartTime { get => startTime; set => SetValue(ref startTime, value); }
        public DateTime StartDate { get => startDate; set => SetValue(ref startDate, value); }
        public DateTime EndTime { get => endTime; set => SetValue(ref endTime, value); }

        public Command EditReservationCommand { get; }
        public Command RemoveReservationCommand { get; }
        public Command StudentTappedCommand { get; }

        private readonly ISingleReservationService singleReservationService = DependencyService.Get<ISingleReservationService>();
        private readonly IRecurringReservationService recurringReservationService = DependencyService.Get<IRecurringReservationService>();
        private readonly IReservationService reservationService = DependencyService.Get<IReservationService>();

        public TutorReservationDetailsViewModel()
        {
            EditReservationCommand = new Command(async () => await OnEditReservation());
            RemoveReservationCommand = new Command(async () => await OnRemoveReservation());
            StudentTappedCommand = new Command(async () => await OnStudentTapped());
        }

        private async Task OnStudentTapped()
        {
            await Shell.Current.GoToAsync($"{nameof(StudentDetailsTutorPage)}?{nameof(StudentDetailsViewModel.Id)}={studentId}");
        }

        private async Task OnRemoveReservation()
        {
            var result = await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, $"{AlertConstans.ConfirmationReservationDeletion}", GeneralConstans.Yes, GeneralConstans.No);
            if (result)
            {
                await RemoveReservationAsync();
            }
        }

        private async Task RemoveReservationAsync()
        {
            if (reservationType == ReservationType.Single)
            {
                await RemoveSingleReservationAsync();
            }
            else
            {
                await RemoveRecurringReservationAsync();
            }
        }

        private async Task RemoveSingleReservationAsync()
        {
            bool removed = await singleReservationService.RemoveReservationAsync(Id);

            if (removed)
            {
                ToastHelper.MakeLongToast(ToastConstans.ReservationRemoved);
                await Shell.Current.GoToAsync($"//{nameof(ReservationsTutorPage)}");
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }

        private async Task RemoveRecurringReservationAsync()
        {
            var result = await Shell.Current.DisplayActionSheet(AlertConstans.ChangeOrderPaymentStatus, GeneralConstans.Cancel, null, AlertConstans.OneLessonRemoving, AlertConstans.OneLessonAndFutureRemoving);
            if (result is null || result == GeneralConstans.Cancel)
            {
                return;
            }

            bool removed = result == AlertConstans.OneLessonRemoving
                ? await recurringReservationService.RemoveReservationAsync(Id, RecurringReservationRemovingMode.OneLesson)
                : await recurringReservationService.RemoveReservationAsync(Id, RecurringReservationRemovingMode.OneLessonAndFuture);

            if (removed)
            {
                ToastHelper.MakeLongToast(ToastConstans.ReservationRemoved);
                await Shell.Current.GoToAsync($"//{nameof(ReservationsTutorPage)}");
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }

        private async Task OnEditReservation()
        {
            await Shell.Current.GoToAsync($"{nameof(EditReservationTutorPage)}?{nameof(EditTutorReservationViewModel.Id)}={Id}");
        }

        private async void LoadReservationById(long id)
        {
            IsBusy = true;

            var reservation = await reservationService.GetReservationByIdAsync(id);
            EndTime = reservation.StartTime.AddMinutes(reservation.Duration);
            studentId = reservation.StudentId;
            reservationType = reservation.Type;
            Cost = $"{reservation.Cost} {GeneralConstans.Pln}";
            SubjectName = reservation.SubjectName;
            Student = reservation.Student;
            LessonTime = $"{StartTime.ToShortTimeString()} - {EndTime.ToShortTimeString()}";
            LessonDate = DateTime.Parse(StartDate.ToShortDateString(), new CultureInfo("pl-PL")).ToString("dd MMMM yyyy");
            Duration = $"{reservation.Duration} {PickerConstans.MinutesShort}";
            Description = reservation.Description;
            SetImage(reservation.StudentId);
            SetType(reservation);
            Place = ReservationsHelper.GetPlace(reservation.Place);
            PaymentStatus = ReservationsHelper.GetPaymentStatus(reservation.IsPaid);

            IsBusy = false;
        }

        private void SetImage(long userId)
        {
            var image = SQLiteManager.Instance.Get<UserImageDto>(userId);
            Image = image is null
                ? ResourceConstans.DefaultStudentPicture
                : ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(image.ProfilePictureBase64)));
        }

        private void SetType(ReservationDetailsDto reservation)
        {
            Type = reservation.Type == ReservationType.Single
                ? PickerConstans.SingleReservation
                : ReservationsHelper.GetRecurringReservationType(reservation.Frequency.Value);
        }
    }
}