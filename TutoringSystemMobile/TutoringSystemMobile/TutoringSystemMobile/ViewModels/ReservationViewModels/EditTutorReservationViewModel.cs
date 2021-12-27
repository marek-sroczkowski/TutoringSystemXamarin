using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.ReservationViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class EditTutorReservationViewModel : BaseViewModel
    {
        private long id;

        public long Id
        {
            get => id;
            set
            {
                id = value;
                LoadReservationById(id);
            }
        }

        public EditTutorReservationViewModel()
        {

        }

        private void LoadReservationById(long id)
        {
            throw new NotImplementedException();
        }
    }
}
