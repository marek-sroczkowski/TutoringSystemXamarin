using System;
using System.ComponentModel;
using System.Windows.Input;
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
            return !string.IsNullOrWhiteSpace(viewModel.Username) &&
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
                    DependencyService.Get<IToast>()?.MakeLongToast("Błąd! Spróbuj ponownie później!");
                    break;
                case AddStudentToTutorStatus.Added:
                    DependencyService.Get<IToast>()?.MakeLongToast("Dodano ucznia!");
                    await Shell.Current.GoToAsync($"//{nameof(StudentsTutorPage)}");
                    break;
                case AddStudentToTutorStatus.IncorrectUsername:
                    await Application.Current.MainPage.DisplayAlert("Uwaga!", "Uczeń o podanym loginie nie istnieje!", "OK");
                    break;
                case AddStudentToTutorStatus.StudentWasAlreadyAdded:
                    await Application.Current.MainPage.DisplayAlert("Uwaga!", "Uczeń jest już dodany!", "OK");
                    break;
            }
        }
    }
}