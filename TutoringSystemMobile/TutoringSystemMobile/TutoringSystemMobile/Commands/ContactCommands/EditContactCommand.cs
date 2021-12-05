using Rg.Plugins.Popup.Services;
using System;
using System.ComponentModel;
using System.Windows.Input;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.ContactDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.ContactViewModels;
using Xamarin.Forms;

namespace TutoringSystemMobile.Commands.ContactCommands
{
    public class EditContactCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly EditContactViewModel viewModel;
        private readonly IContactService contactService;
        private readonly IReloadContactService reloadContactService;

        public EditContactCommand(EditContactViewModel viewModel, IContactService contactService)
        {
            this.viewModel = viewModel;
            this.contactService = contactService;
            reloadContactService = DependencyService.Get<IReloadContactService>();
            this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return !viewModel.IsBusy;
        }

        public async void Execute(object parameter)
        {
            viewModel.IsBusy = true;
            var updated = await contactService.UpdateContactAsync(new UpdatedContactDto(viewModel.Id, viewModel.DiscordName));
            viewModel.IsBusy = false;

            if (updated)
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.Updated);
                await PopupNavigation.Instance.PopAsync();
                reloadContactService.ReloadContact();
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }
    }
}
