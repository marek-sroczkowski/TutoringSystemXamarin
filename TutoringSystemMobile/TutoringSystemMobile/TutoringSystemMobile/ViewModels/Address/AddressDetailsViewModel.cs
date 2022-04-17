using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Services.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Address
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
        public string Street
        {
            get => string.IsNullOrEmpty(street) ? GeneralConstans.NoValue : street;
            set => SetValue(ref street, value);
        }
        public string HouseAndFlatNumber
        {
            get => string.IsNullOrEmpty(houseAndFlatNumber) ? GeneralConstans.NoValue : houseAndFlatNumber;
            set => SetValue(ref houseAndFlatNumber, value);
        }
        public string City
        {
            get => string.IsNullOrEmpty(city) ? GeneralConstans.NoValue : city;
            set => SetValue(ref city, value);
        }
        public string PostalCode
        {
            get => string.IsNullOrEmpty(postalCode) ? GeneralConstans.NoValue : postalCode;
            set => SetValue(ref postalCode, value);
        }
        public string Description { get => description; set => SetValue(ref description, value); }
        public string Owner { get => owner; set => SetValue(ref owner, value); }

        public Command PageAppearingCommand { get; }
        public Command NavigateToStudentCommand { get; }

        private readonly IAddressService addressService = DependencyService.Get<IAddressService>();

        public AddressDetailsViewModel()
        {
            PageAppearingCommand = new Command(async () => await OnAppearing());

            NavigateToStudentCommand = new Command(async () => await OnNavigateToStudent(), CanNavigateToStudent);
            PropertyChanged += (_, __) => NavigateToStudentCommand.ChangeCanExecute();
        }

        public bool CanNavigateToStudent()
        {
            return !Street.IsEmpty()
                && !HouseAndFlatNumber.IsEmpty()
                && !City.IsEmpty()
                && !PostalCode.IsEmpty();
        }

        private async Task OnNavigateToStudent()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                await Launcher.OpenAsync($"http://maps.apple.com/?daddr={GetDestination()},+CA&saddr=");
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                await Launcher.OpenAsync($"http://maps.google.com/?daddr={GetDestination()},+CA&saddr=");
            }
        }

        private string GetDestination()
        {
            string houseNumber = HouseAndFlatNumber;
            if (HouseAndFlatNumber.Contains("\\"))
            {
                int endIndex = HouseAndFlatNumber.IndexOf("\\");
                houseNumber = HouseAndFlatNumber[..endIndex];
            }
            else if (HouseAndFlatNumber.Contains("/"))
            {
                int endIndex = HouseAndFlatNumber.IndexOf("/");
                houseNumber = HouseAndFlatNumber[..endIndex];
            }

            return $"{houseNumber}+{Street}+{PostalCode}+{City}";
        }

        private async Task OnAppearing()
        {
            IsBusy = true;

            await SecureStorage.SetAsync(SecureStorageConstans.CurrentPage, SecureStorageConstans.Address);
            long addressId = long.Parse(await SecureStorage.GetAsync(SecureStorageConstans.AddressId));
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