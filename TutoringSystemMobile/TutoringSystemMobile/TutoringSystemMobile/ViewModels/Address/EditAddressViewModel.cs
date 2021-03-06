using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.Address;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Address
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class EditAddressViewModel : BaseViewModel
    {
        private long id;
        private string street;
        private string houseAndFlatNumber;
        private string city;
        private string postalCode;
        private string description;

        public long Id
        {
            get => id;
            set
            {
                id = value;
                LoadAddressById(id);
            }
        }
        public string Street { get => street; set => SetValue(ref street, value); }
        public string HouseAndFlatNumber { get => houseAndFlatNumber; set => SetValue(ref houseAndFlatNumber, value); }
        public string City { get => city; set => SetValue(ref city, value); }
        public string PostalCode { get => postalCode; set => SetValue(ref postalCode, value); }
        public string Description { get => description; set => SetValue(ref description, value); }

        public Command EditAddressCommand { get; }

        private readonly IAddressService addressService = DependencyService.Get<IAddressService>();

        public EditAddressViewModel()
        {
            EditAddressCommand = new Command(async () => await OnEditAddress(), CanEditAddress);
            PropertyChanged += (_, __) => EditAddressCommand.ChangeCanExecute();
        }

        public bool CanEditAddress()
        {
            return !IsBusy;
        }

        private async Task OnEditAddress()
        {
            IsBusy = true;
            var updatedAddress = new UpdatedAddressDto(Id, Street, HouseAndFlatNumber, City, PostalCode, Description);
            var updated = await addressService.UpdateAddressAsync(updatedAddress);
            IsBusy = false;

            if (updated)
            {
                ToastHelper.MakeLongToast(ToastConstans.Updated);
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                ToastHelper.MakeLongToast(ToastConstans.ErrorTryAgainLater);
            }
        }

        private async void LoadAddressById(long addressId)
        {
            IsBusy = true;

            var address = await addressService.GetAddressByIdAsync(addressId);

            Street = address.Street;
            HouseAndFlatNumber = address.HouseAndFlatNumber;
            City = address.City;
            PostalCode = address.PostalCode;
            Description = address.Description;

            IsBusy = false;
        }
    }
}