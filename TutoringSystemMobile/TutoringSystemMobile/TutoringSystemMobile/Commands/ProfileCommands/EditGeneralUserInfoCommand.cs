using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.AccountDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.ProfileViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.ProfileCommands
{
    public class EditGeneralUserInfoCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly EditGeneralUserInfoViewModel viewModel;
        private readonly IUserService userService;

        public EditGeneralUserInfoCommand(EditGeneralUserInfoViewModel viewModel, IUserService userService)
        {
            this.viewModel = viewModel;
            this.userService = userService;
            this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return !viewModel.FirstName.IsEmpty() &&
                !viewModel.IsBusy;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            var updated = await userService.UpdateGeneralUserInfoAsync(new UpdatedUserDto(viewModel.FirstName, viewModel.LastName));
            viewModel.IsBusy = false;

            if (updated)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.Updated);
                await SecureStorage.SetAsync(SecureStorageConstans.UserName, $"{viewModel.FirstName} {viewModel.LastName}");
                MessagingCenter.Send(this, MessagingCenterConstans.NameChanged);
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }
    }
}