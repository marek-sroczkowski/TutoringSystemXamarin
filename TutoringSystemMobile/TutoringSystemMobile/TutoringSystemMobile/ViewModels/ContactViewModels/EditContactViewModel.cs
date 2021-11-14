using System.Windows.Input;
using TutoringSystemMobile.Commands.ContactCommands;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.ContactViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class EditContactViewModel : BaseViewModel
    {
        private long id;
        private string discordName;

        public long Id { get => id; set => SetValue(ref id, value); }
        public string DiscordName { get => discordName; set => SetValue(ref discordName, value); }

        public Command PageAppearingCommand { get; }
        public ICommand EditContactCommand { get; }

        private readonly IContactService contactService;

        public EditContactViewModel()
        {
            contactService = DependencyService.Get<IContactService>();
            PageAppearingCommand = new Command(OnAppearing);
            EditContactCommand = new EditContactCommand(this, contactService);
        }

        private async void OnAppearing()
        {
            IsBusy = true;

            Id = long.Parse(await SecureStorage.GetAsync("contactId"));
            var contact = await contactService.GetContactByIdAsync(Id);
            DiscordName = contact.DiscordName;

            IsBusy = false;
        }
    }
}