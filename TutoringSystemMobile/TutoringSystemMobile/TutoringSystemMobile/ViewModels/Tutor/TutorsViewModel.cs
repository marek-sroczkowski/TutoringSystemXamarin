﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Dtos.Tutor;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Tutor
{
    public class TutorsViewModel : BaseViewModel
    {
        public ObservableCollection<DisplayedTutorDto> Tutors { get; }

        public Command LoadTutorsCommand { get; }
        public Command AddTutorCommand { get; }
        public Command<DisplayedTutorDto> TutorTapped { get; }
        public Command PageAppearingCommand { get; }

        public readonly ITutorService tutorService;

        public TutorsViewModel()
        {
            Tutors = new ObservableCollection<DisplayedTutorDto>();
            tutorService = DependencyService.Get<ITutorService>();
            LoadTutorsCommand = new Command(async () => await LoadTutorsAsync());
            AddTutorCommand = new Command(async () => await OnAddTutor());
            TutorTapped = new Command<DisplayedTutorDto>(async (tutor) => await OnTutorSelected(tutor));
            PageAppearingCommand = new Command(OnAppearing);
        }

        private void OnAppearing()
        {
            IsBusy = true;
        }

        private async Task LoadTutorsAsync()
        {
            IsBusy = true;

            var tutors = await tutorService.GetTutorsByStudentAsync();
            var displayedTutors = tutors.Select(t => new DisplayedTutorDto(t));
            Tutors.Clear();
            foreach (var tutor in displayedTutors)
            {
                Tutors.Add(tutor);
            }

            IsBusy = false;
        }

        private async Task OnTutorSelected(DisplayedTutorDto tutor)
        {
            if (tutor == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(TutorDetailsStudentPage)}?{nameof(TutorDetailsViewModel.Id)}={tutor.Id}");
        }

        private async Task OnAddTutor()
        {
            await Shell.Current.GoToAsync($"{nameof(SearchTutorStudentPage)}");
        }
    }
}
