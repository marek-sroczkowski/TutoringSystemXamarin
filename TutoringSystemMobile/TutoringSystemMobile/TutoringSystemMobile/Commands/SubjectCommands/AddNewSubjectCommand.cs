﻿using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.SubjectDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.SubjectViewModels;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.SubjectCommands
{
    public class AddNewSubjectCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly NewSubjectViewModel viewModel;
        private readonly ISubjectService subjectService;

        public AddNewSubjectCommand(NewSubjectViewModel viewModel, ISubjectService subjectService)
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
            return !viewModel.Name.IsEmpty() &&
                !viewModel.SelectedCategory.IsEmpty() &&
                !viewModel.SelectedPlace.IsEmpty() &&
                !viewModel.IsBusy;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            long newOrderId = await subjectService.AddSubjectAsync(new NewSubjectDto(viewModel.Name, viewModel.Description, viewModel.Place, viewModel.Category));
            viewModel.IsBusy = false;

            if (newOrderId == -1)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.AddedSubject);
                await Shell.Current.GoToAsync($"//{nameof(SubjectsTutorPage)}/{nameof(SubjectDetailsTutorPage)}?{nameof(SubjectDetailsViewModel.Id)}={newOrderId}");
            }
        }
    }
}
