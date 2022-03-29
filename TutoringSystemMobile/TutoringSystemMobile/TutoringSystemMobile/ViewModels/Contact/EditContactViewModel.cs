using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Contact;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Contact
{
    public class EditContactViewModel : BaseViewModel
    {
        private long id;
        private string discordName;

        public long Id { get => id; set => SetValue(ref id, value); }
        public string DiscordName { get => discordName; set => SetValue(ref discordName, value); }

        public Command PageAppearingCommand { get; }
        public Command EditContactCommand { get; }

        private readonly IContactService contactService = DependencyService.Get<IContactService>();
        private readonly IReloadContactService reloadContactService = DependencyService.Get<IReloadContactService>();

        public EditContactViewModel(long contactId)
        {
            Id = contactId;

            PageAppearingCommand = new Command(async () => await OnAppearing());
            EditContactCommand = new Command(async () => await OnEditContact(), CanEditContact);
            PropertyChanged += (_, __) => EditContactCommand.ChangeCanExecute();
        }

        public bool CanEditContact()
        {
            return !IsBusy;
        }

        private async Task OnEditContact()
        {
            IsBusy = true;
            var updatedContact = new UpdatedContactDto(Id, DiscordName);
            var updated = await contactService.UpdateContactAsync(updatedContact);
            IsBusy = false;

            if (updated)
            {
                ToastHelper.MakeLongToast(ToastConstans.Updated);
                await PopupNavigation.Instance.PopAsync();
                reloadContactService.ReloadContact();
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }

        private async Task OnAppearing()
        {
            IsBusy = true;

            var contact = await contactService.GetContactByIdAsync(Id);
            DiscordName = contact.DiscordName;

            IsBusy = false;
        }
    }
}