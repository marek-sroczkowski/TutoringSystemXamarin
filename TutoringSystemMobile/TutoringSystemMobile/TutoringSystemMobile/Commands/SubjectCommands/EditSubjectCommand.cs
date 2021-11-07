using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Models.SubjectDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.SubjectViewModels;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.SubjectCommands
{
    public class EditSubjectCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly EditSubjectViewModel viewModel;
        private readonly ISubjectService subjectService;

        public EditSubjectCommand(EditSubjectViewModel viewModel, ISubjectService subjectService)
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
            return !string.IsNullOrEmpty(viewModel.Name) &&
                !string.IsNullOrEmpty(viewModel.SelectedCategory) &&
                !string.IsNullOrEmpty(viewModel.SelectedPlace) &&
                !viewModel.IsBusy;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            bool updated = await subjectService.UpdateSubjectAsync(new UpdatedSubjectDto(viewModel.Id, viewModel.Name, viewModel.Description, viewModel.Place, viewModel.Category));
            viewModel.IsBusy = false;

            if (updated)
            {
                await Shell.Current.GoToAsync($"//{nameof(SubjectsTutorPage)}/{nameof(SubjectDetailsTutorPage)}?{nameof(EditSubjectViewModel.Id)}={viewModel.Id}");
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeToast("Błąd! Spróbuj ponownie później");
            }
        }
    }
}
