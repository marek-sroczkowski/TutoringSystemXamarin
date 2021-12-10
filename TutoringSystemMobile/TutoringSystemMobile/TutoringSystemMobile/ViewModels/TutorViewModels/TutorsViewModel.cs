using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.StudentDtos;
using TutoringSystemMobile.Models.TutorDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Utils;
using TutoringSystemMobile.ViewModels.StudentViewModels;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.TutorViewModels
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

            var tutors = await tutorService.GetTutorsAsync();
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
            //var result = await Shell.Current.DisplayActionSheet(AlertConstans.NewStudent, GeneralConstans.Cancel, null, AlertConstans.NotExistingStudent, AlertConstans.ExistingStudent);
            //if (result == AlertConstans.ExistingStudent)
            //{
            //    await Shell.Current.GoToAsync($"{nameof(NewExistingStudentTutorPage)}");
            //}
            //else if (result == AlertConstans.NotExistingStudent)
            //{
            //    await Shell.Current.GoToAsync($"{nameof(CreatingNewStudentTutorPage)}");
            //}
        }
    }
}
