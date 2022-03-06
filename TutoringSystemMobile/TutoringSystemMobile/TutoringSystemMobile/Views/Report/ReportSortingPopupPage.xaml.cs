using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using TutoringSystemMobile.ViewModels.Report;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutoringSystemMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportSortingPopupPage
    {
        public ReportSortingPopupPage(string sortBy)
        {
            InitializeComponent();
            BindingContext = new ReportSortingViewModel(sortBy);
        }

        private async void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    MovePopup(e);
                    break;
                case GestureStatus.Completed:
                    await SetFinishPopupPosition();
                    break;
            }
        }

        private async Task SetFinishPopupPosition()
        {
            if (BottomPopup.TranslationY > BottomPopup.Height - BottomPopup.Height / 2)
                await PopupNavigation.Instance.PopAsync();
            else
                BottomPopup.TranslationY = 0;
        }

        private void MovePopup(PanUpdatedEventArgs e)
        {
            if (e.TotalY > 0)
                BottomPopup.TranslationY = BottomPopup.TranslationY + e.TotalY;
            else if (BottomPopup.TranslationY > 0)
                BottomPopup.TranslationY = BottomPopup.TranslationY + e.TotalY;
        }
    }
}