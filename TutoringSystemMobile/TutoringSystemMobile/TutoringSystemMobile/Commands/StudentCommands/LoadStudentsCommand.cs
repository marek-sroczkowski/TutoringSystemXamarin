using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.ViewModels.StudentViewModels;

namespace TutoringSystemMobile.Commands.StudentCommands
{
    public class LoadStudentsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly StudentsViewModel viewModel;
        private readonly IStudentService studentService;

        public LoadStudentsCommand(StudentsViewModel viewModel, IStudentService studentService)
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
            return true;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            viewModel.Students.Clear();
            var students = await studentService.GetStudentsAsync();
            foreach (var student in students)
                viewModel.Students.Add(student);

            viewModel.IsBusy = false;
        }
    }
}
