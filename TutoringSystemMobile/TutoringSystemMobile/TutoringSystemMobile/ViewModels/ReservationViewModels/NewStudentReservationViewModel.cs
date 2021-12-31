using System;
using System.Collections.Generic;
using System.Text;

namespace TutoringSystemMobile.ViewModels.ReservationViewModels
{
    public class NewStudentReservationViewModel : BaseViewModel
    {
        private DateTime startDate;

        public DateTime StartDate { get => startDate; set => SetValue(ref startDate, value); }
    }
}