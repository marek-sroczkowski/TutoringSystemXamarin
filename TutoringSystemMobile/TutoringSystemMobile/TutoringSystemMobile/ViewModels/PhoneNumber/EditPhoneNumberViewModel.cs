using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.PhoneNumber;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.PhoneNumber
{
    public class EditPhoneNumberViewModel : BaseViewModel
    {
        private long phoneNumberId;
        private string owner;
        private string number;
        private long contactId;

        public long ContactId { get => contactId; set => SetValue(ref contactId, value); }
        public long PhoneNumberId { get => phoneNumberId; set => SetValue(ref phoneNumberId, value); }
        public string Owner { get => owner; set => SetValue(ref owner, value); }
        public string Number { get => number; set => SetValue(ref number, value); }

        public Command PageAppearingCommand { get; }
        public Command EditPhoneNumberCommand { get; }

        private readonly IPhoneNumberService phoneNumberService = DependencyService.Get<IPhoneNumberService>();
        private readonly IReloadContactService reloadContactService = DependencyService.Get<IReloadContactService>();

        public EditPhoneNumberViewModel(long phoneNumberId)
        {
            PhoneNumberId = phoneNumberId;

            PageAppearingCommand = new Command(async () => await OnAppearing());

            EditPhoneNumberCommand = new Command(async () => await OnEditPhoneNumber(), CanEditPhoneNumber);
            PropertyChanged += (_, __) => EditPhoneNumberCommand.ChangeCanExecute();
        }

        public bool CanEditPhoneNumber()
        {
            return !Owner.IsEmpty()
                && !Number.IsEmpty()
                && !IsBusy;
        }

        private async Task OnEditPhoneNumber()
        {
            IsBusy = true;
            var updatedPhone = new UpdatedPhoneNumberDto(PhoneNumberId, Owner, Number);
            var updated = await phoneNumberService.UpdatePhoneNumberAsync(ContactId, updatedPhone);
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

            ContactId = long.Parse(await SecureStorage.GetAsync(SecureStorageConstans.ContactId));
            var phoneNumber = await phoneNumberService.GetPhoneNumberById(ContactId, PhoneNumberId);
            Owner = phoneNumber.Owner;
            Number = phoneNumber.Number;

            IsBusy = false;
        }
    }
}