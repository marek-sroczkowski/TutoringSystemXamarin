using System.Windows.Input;
using TutoringSystemMobile.Commands.ContactCommands;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.ContactViewModels
{
    public class EditContactViewModel : BaseViewModel
    {
        private long id;
        private string discordName;

        public long Id { get => id; set => SetValue(ref id, value); }
        public string DiscordName { get => discordName; set => SetValue(ref discordName, value); }

        public Command PageAppearingCommand { get; }
        public ICommand EditContactCommand { get; }

        private readonly IContactService contactService;

        public EditContactViewModel(long contactId)
        {
            Id = contactId;
            contactService = DependencyService.Get<IContactService>();
            PageAppearingCommand = new Command(OnAppearing);
            EditContactCommand = new EditContactCommand(this, contactService);
        }

        private async void OnAppearing()
        {
            IsBusy = true;

            var contact = await contactService.GetContactByIdAsync(Id);
            DiscordName = contact.DiscordName;

            IsBusy = false;
        }
    }
}