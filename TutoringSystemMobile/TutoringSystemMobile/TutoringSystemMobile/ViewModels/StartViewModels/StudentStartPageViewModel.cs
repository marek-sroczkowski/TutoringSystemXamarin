﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TutoringSystemMobile.Services.Synchronization;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.StartViewModels
{
    public class StudentStartPageViewModel : BaseViewModel
    {
        public Command PageAppearingCommand { get; }

        public StudentStartPageViewModel()
        {
            PageAppearingCommand = new Command(async () => await OnAppearing());
        }

        private async Task OnAppearing()
        {
            IsBusy = true;

            await SynchronizationService.Instance.StartSynchronization();
        }
    }
}