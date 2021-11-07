using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.ViewModels.SubjectViewModels;

namespace TutoringSystemMobile.Commands.SubjectCommands
{
    public class LoadSubjectsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly SubjectsViewModel viewModel;
        private readonly ISubjectService subjectService;

        public LoadSubjectsCommand(SubjectsViewModel viewModel, ISubjectService subjectService)
        {
            this.viewModel = viewModel;
            this.subjectService = subjectService;
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
            viewModel.Subjects.Clear();
            var subjects = await subjectService.GetSubjectsAsync();
            foreach (var subject in subjects)
                viewModel.Subjects.Add(subject);

            viewModel.IsBusy = false;
        }
    }
}