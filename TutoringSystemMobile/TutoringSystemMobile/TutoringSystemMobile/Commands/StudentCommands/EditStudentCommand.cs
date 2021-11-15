using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Models.StudentDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.StudentViewModels;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.StudentCommands
{
    public class EditStudentCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly EditStudentGeneralInformationViewModel viewModel;
        private readonly IStudentService studentService;

        public EditStudentCommand(EditStudentGeneralInformationViewModel viewModel, IStudentService studentService)
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
            return double.TryParse(viewModel.HourRate, out double hourRate) &&
                hourRate > 0;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            var updated = await studentService.UpdateStudentAsync(new UpdatedStudentDto(viewModel.Id, double.Parse(viewModel.HourRate), viewModel.Note));
            viewModel.IsBusy = false;

            if(updated)
                await Shell.Current.GoToAsync($"..");
            else
                DependencyService.Get<IToast>()?.MakeLongToast("Błąd! Spróbuj ponownie później!");
        }
    }
}
