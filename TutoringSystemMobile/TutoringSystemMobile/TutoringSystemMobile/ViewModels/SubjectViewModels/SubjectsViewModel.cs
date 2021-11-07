using System.Collections.ObjectModel;
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
        private SubjectDto selectedSubject;
        public ObservableCollection<SubjectDto> Subjects { get; }

        public SubjectDto SelectedSubject
        {
            get => selectedSubject;
            set
            {
                SetValue(ref selectedSubject, value);
                OnSubjectSelected(value);
            }
        }

        public ICommand LoadSubjectsCommand { get; }
        public Command NewSubjectFormCommand { get; }
        public Command<SubjectDto> SubjectTapped { get; }
        public Command PageAppearingCommand { get; }

        public readonly ISubjectService subjectService;

        public SubjectsViewModel()
        {
            Subjects = new ObservableCollection<SubjectDto>();
            subjectService = DependencyService.Get<ISubjectService>();
            LoadSubjectsCommand = new LoadSubjectsCommand(this, subjectService);
            NewSubjectFormCommand = new Command(OnNewSubjectClick);
            SubjectTapped = new Command<SubjectDto>(OnSubjectSelected);
            PageAppearingCommand = new Command(OnAppearing);
        }

        private void OnAppearing()
        {
            IsBusy = true;
            selectedSubject = null;
        }

        private async void OnSubjectSelected(SubjectDto subject)
        {
            if (subject == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(SubjectDetailsTutorPage)}?{nameof(SubjectDetailsViewModel.Id)}={subject.Id}");
        }

        private async void OnNewSubjectClick()
        {
            await Shell.Current.GoToAsync($"{nameof(NewSubjectTutorPage)}");
        }
    }
}
