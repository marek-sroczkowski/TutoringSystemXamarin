using System.ComponentModel;
using TutoringSystemMobile.ViewModels;
using Xamarin.Forms;

namespace TutoringSystemMobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}