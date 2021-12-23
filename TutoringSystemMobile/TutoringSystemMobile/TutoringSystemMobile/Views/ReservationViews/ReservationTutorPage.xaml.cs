using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservationTutorPage : ContentPage
    {
        public ReservationTutorPage()
        {
            InitializeComponent();
            calendar.SelectedDate = System.DateTime.Now;
        }
    }
}