using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TutoringSystemMobile.Commands.SubjectCommands;
using TutoringSystemMobile.Models.SubjectDtos;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Views;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.SubjectViewModels
{
    public class SubjectsViewModel : BaseViewModel
    {
        public ObservableCollection<SubjectDto> Subjects { get; }

        public ICommand LoadSubjectsCommand { get; }
        public Command NewSubjectFormCommand { get; }
        public Command<SubjectDto> SubjectTapped { get; }
        public Command PageAppearingCommand { get; }

        private readonly ISubjectService subjectService;

        public SubjectsViewModel()
        {
            Subjects = new ObservableCollection<SubjectDto>();
            subjectService = DependencyService.Get<ISubjectService>();
            LoadSubjectsCommand = new LoadSubjectsCommand(this, subjectService);
            NewSubjectFormCommand = new Command(async () => await OnNewSubjectClick());
            SubjectTapped = new Command<SubjectDto>(async (subject) => await OnSubjectSelected(subject));
            PageAppearingCommand = new Command(OnAppearing);
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
