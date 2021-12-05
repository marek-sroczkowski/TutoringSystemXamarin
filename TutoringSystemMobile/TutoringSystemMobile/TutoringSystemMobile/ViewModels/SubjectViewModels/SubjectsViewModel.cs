using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.SubjectDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.SubjectViewModels
{
    public class SubjectsViewModel : BaseViewModel
    {
        public ObservableCollection<SubjectDto> Subjects { get; }

        public Command LoadSubjectsCommand { get; }
        public Command NewSubjectFormCommand { get; }
        public Command<SubjectDto> SubjectTapped { get; }
        public Command PageAppearingCommand { get; }

        public SubjectsViewModel()
        {
            Subjects = new ObservableCollection<SubjectDto>();
            LoadSubjectsCommand = new Command(async () => await OnLoadSubject());
            NewSubjectFormCommand = new Command(async () => await OnNewSubjectClick());
            SubjectTapped = new Command<SubjectDto>(async (subject) => await OnSubjectSelected(subject));
            PageAppearingCommand = new Command(OnAppearing);
        }

        private async Task OnLoadSubject()
        {
            IsBusy = true;

            Subjects.Clear();
            var subjects = await DependencyService.Get<ISubjectService>().GetSubjectsAsync();
            foreach (var subject in subjects)
                Subjects.Add(subject);

            IsBusy = false;
        }

        private void OnAppearing()
        {
            IsBusy = true;
        }

        private async Task OnSubjectSelected(SubjectDto subject)
        {
            if (subject == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(SubjectDetailsTutorPage)}?{nameof(SubjectDetailsViewModel.Id)}={subject.Id}");
        }

        private async Task OnNewSubjectClick()
        {
            await Shell.Current.GoToAsync($"{nameof(NewSubjectTutorPage)}");
        }
    }
}
