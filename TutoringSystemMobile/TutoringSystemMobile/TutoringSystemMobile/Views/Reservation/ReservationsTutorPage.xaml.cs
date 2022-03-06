using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservationsTutorPage : ContentPage
    {
        public ReservationsTutorPage()
        {
            InitializeComponent();
            calendar.SelectedDate = System.DateTime.Now;
        }
    }
}