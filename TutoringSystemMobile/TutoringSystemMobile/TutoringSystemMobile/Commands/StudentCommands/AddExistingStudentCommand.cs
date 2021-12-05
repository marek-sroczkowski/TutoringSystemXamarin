using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.StudentDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.StudentViewModels;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.StudentCommands
{
    public class AddExistingStudentCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly AddExistingStudentViewModel viewModel;
        private readonly IStudentService studentService;

        public AddExistingStudentCommand(AddExistingStudentViewModel viewModel, IStudentService studentService)
        {
            this.viewModel = viewModel;
            this.studentService = studentService;
            this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return !viewModel.Username.IsEmpty() &&
                double.TryParse(viewModel.HourRate, out double hourRate) &&
                hourRate > 0 &&
                !viewModel.IsBusy;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            var status = await studentService.AddStudentToTutorAsync(new NewExistingStudentDto(viewModel.Username, double.Parse(viewModel.HourRate), viewModel.Note));
            viewModel.IsBusy = false;

            switch (status)
            {
                case AddStudentToTutorStatus.InternalError:
                    DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
                    break;
                case AddStudentToTutorStatus.Added:
                    DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.AddedStudent);
                    await Shell.Current.GoToAsync($"//{nameof(StudentsTutorPage)}");
                    break;
                case AddStudentToTutorStatus.IncorrectUsername:
                    await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, AlertConstans.StudentNotExist, GeneralConstans.Ok);
                    break;
                case AddStudentToTutorStatus.StudentWasAlreadyAdded:
                    await Application.Current.MainPage.DisplayAlert(AlertConstans.Attention, AlertConstans.StudentAlreadyExist, GeneralConstans.Ok);
                    break;
            }
        }
    }
}