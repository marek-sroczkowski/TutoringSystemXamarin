﻿using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Student;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Student
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class EditStudentGeneralInformationViewModel : BaseViewModel
    {
        private long id;
        private string hourRate;
        private string note;
        private string firstName;
        private string lastName;

        public long Id
        {
            get => id;
            set
            {
                id = value;
                LoadStudentById(id);
            }
        }
        public string HourRate { get => hourRate; set => SetValue(ref hourRate, value); }
        public string Note { get => note; set => SetValue(ref note, value); }
        public string FirstName { get => firstName; set => SetValue(ref firstName, value); }
        public string LastName { get => lastName; set => SetValue(ref lastName, value); }

        public Command EditStudentCommand { get; }

        private readonly IStudentService studentService = DependencyService.Get<IStudentService>();

        public EditStudentGeneralInformationViewModel()
        {
            EditStudentCommand = new Command(async () => await OnEditStudent(), CanEditStudent);
            PropertyChanged += (_, __) => EditStudentCommand.ChangeCanExecute();
        }

        private async Task OnEditStudent()
        {
            IsBusy = true;
            var student = new UpdatedStudentDto(Id, double.Parse(HourRate), Note, FirstName, LastName);
            var updated = await studentService.UpdateStudentAsync(student);
            IsBusy = false;

            if (updated)
            {
                await Shell.Current.GoToAsync($"..");
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }

        private async void LoadStudentById(long studentId)
        {
            IsBusy = true;

            var student = await studentService.GetStudentByIdAsync(studentId);
            HourRate = student.HourlRate.ToString();
            Note = student.Note;
            FirstName = student.FirstName;
            LastName = student.LastName;

            IsBusy = false;
        }

        public bool CanEditStudent()
        {
            return !FirstName.IsEmpty()
                && double.TryParse(HourRate, out double hourRate)
                && hourRate > 0
                && !IsBusy;
        }
    }
}