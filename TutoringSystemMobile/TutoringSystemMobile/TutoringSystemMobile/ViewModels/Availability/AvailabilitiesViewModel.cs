using System;
using System.Collections.Generic;
using System.Text;

namespace TutoringSystemMobile.ViewModels.Availability
{
    public class AvailabilitiesViewModel : BaseViewModel
    {
        private DateTime startDate;
        public DateTime StartDate { get => startDate; set => SetValue(ref startDate, value); }
    }
}
