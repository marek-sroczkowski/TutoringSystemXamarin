using System.Windows.Input;
using TutoringSystemMobile.Commands.StudentCommands;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.AddressViewModels
{
    public class AddressDetailsViewModel : BaseViewModel
    {
        private long id;
        private string street;
        private string houseAndFlatNumber;
        private string city;
        private string postalCode;
        private string description;
        private string owner;

        public long Id { get => id; set => SetValue(ref id, value); }
        public string Street { get => street; set => SetValue(ref street, value); }
        public string HouseAndFlatNumber { get => houseAndFlatNumber; set => SetValue(ref houseAndFlatNumber, value); }
        public string City { get => city; set => SetValue(ref city, value); }
        public string PostalCode { get => postalCode; set => SetValue(ref postalCode, value); }
        public string Description { get => description; set => SetValue(ref description, value); }
        public string Owner { get => owner; set => SetValue(ref owner, value); }

        public Command PageAppearingCommand { get; }
        public ICommand NavigateToStudentCommand { get; }

        private readonly IAddressService addressService;

        public AddressDetailsViewModel()
        {
            addressService = DependencyService.Get<IAddressService>();
            PageAppearingCommand = new Command(OnAppearing);
            NavigateToStudentCommand = new NavigateToStudentCommand(this);
        }

        private async void OnAppearing()
        {
            IsBusy = true;

            long addressId = long.Parse(await SecureStorage.GetAsync("addressId"));
            var address = await addressService.GetAddressByIdAsync(addressId);

            Street = address.Street;
            HouseAndFlatNumber = address.HouseAndFlatNumber;
            City = address.City;
            PostalCode = address.PostalCode;
            Description = address.Description;
            Owner = address.Owner;

            IsBusy = false;
        }
    }
}
