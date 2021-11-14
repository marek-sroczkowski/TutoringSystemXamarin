using System.Windows.Input;
using TutoringSystemMobile.Commands.AddressCommands;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.AddressViewModels
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

        public ICommand EditAddressCommand { get; }

        private readonly IAddressService addressService;

        public EditAddressViewModel()
        {
            addressService = DependencyService.Get<IAddressService>();
            EditAddressCommand = new EditAddressCommand(this, addressService);
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