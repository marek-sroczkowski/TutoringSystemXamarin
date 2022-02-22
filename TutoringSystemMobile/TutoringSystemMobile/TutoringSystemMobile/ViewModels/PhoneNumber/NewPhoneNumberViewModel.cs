using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Dtos.PhoneNumber;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.PhoneNumber
{
    public class NewPhoneNumberViewModel : BaseViewModel
    {
        private long contactId;
        private string owner;
        private string number;

        public long ContactId { get => contactId; set => SetValue(ref contactId, value); }
        public string Owner { get => owner; set => SetValue(ref owner, value); }
        public string Number { get => number; set => SetValue(ref number, value); }

        public Command AddPhoneNumberCommand { get; }

        public NewPhoneNumberViewModel(long contactId)
        {
            ContactId = contactId;
            AddPhoneNumberCommand = new Command(async () => await OnAddPhoneNumber(), CanAddPhoneNumber);
            PropertyChanged += (_, __) => AddPhoneNumberCommand.ChangeCanExecute();
        }

        public bool CanAddPhoneNumber()
        {
            return !Owner.IsEmpty() &&
                !Number.IsEmpty() &&
                !IsBusy;
        }

        private async Task OnAddPhoneNumber()
        {
            IsBusy = true;
            var added = await DependencyService.Get<IPhoneNumberService>()
                .AddPhoneNumberAsync(ContactId, new NewPhoneNumberDto(Owner, Number));
            IsBusy = false;

            if (added)
            {
                await PopupNavigation.Instance.PopAsync();
                DependencyService.Get<IReloadContactService>().ReloadContact();
            }
            else
            {
                DependencyService.Get<IToast>()?.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }
    }
}