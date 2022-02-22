using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Helpers;
using Xamarin.Forms;

namespace TutoringSystemMobile.ViewModels.Subject
{
    public class SubjectPlaceViewModel : BaseViewModel
    {
        private bool isTaughtOnline;
        private bool isTaughtAtTutor;
        private bool isTaughtAtStudent;
        private SubjectPlace? place;

        public bool IsTaughtOnline { get => isTaughtOnline; set => SetValue(ref isTaughtOnline, value); }
        public bool IsTaughtAtTutor { get => isTaughtAtTutor; set => SetValue(ref isTaughtAtTutor, value); }
        public bool IsTaughtAtStudent { get => isTaughtAtStudent; set => SetValue(ref isTaughtAtStudent, value); }
        public SubjectPlace? Place
        {
            get => place;
            set
            {
                SetValue(ref place, value);
                if (!place.HasValue)
                    return;

                switch (place)
                {
                    case SubjectPlace.All:
                        SetCheckBoxActivity(new SubjectPlaceCheckBoxActivity(true, true, true));
                        break;
                    case SubjectPlace.AtTutor:
                        SetCheckBoxActivity(new SubjectPlaceCheckBoxActivity(false, true, false));
                        break;
                    case SubjectPlace.AtStudent:
                        SetCheckBoxActivity(new SubjectPlaceCheckBoxActivity(false, false, true));
                        break;
                    case SubjectPlace.Online:
                        SetCheckBoxActivity(new SubjectPlaceCheckBoxActivity(true, false, false));
                        break;
                    case SubjectPlace.AtTutorAndAtStudent:
                        SetCheckBoxActivity(new SubjectPlaceCheckBoxActivity(false, true, true));
                        break;
                    case SubjectPlace.AtTutorAndOnline:
                        SetCheckBoxActivity(new SubjectPlaceCheckBoxActivity(true, true, false));
                        break;
                    case SubjectPlace.AtStudentAndOnline:
                        SetCheckBoxActivity(new SubjectPlaceCheckBoxActivity(true, false, true));
                        break;
                }
            }
        }

        public Command TaughtAtTutorCommand { get; }
        public Command TaughtAtStudentCommand { get; }
        public Command TaughtOnlineCommand { get; }
        public Command SetSubjectPlaceCommand { get; }

        public SubjectPlaceViewModel(SubjectPlace? place)
        {
            Place = place;
            TaughtAtTutorCommand = new Command(OnTaughtAtTutor);
            TaughtAtStudentCommand = new Command(OnTaughtAtStudent);
            TaughtOnlineCommand = new Command(OnTaughtOnline);
            SetSubjectPlaceCommand = new Command(async () => await OnSetSubjectPlace(), CanSetSubject);
            PropertyChanged += (_, __) => SetSubjectPlaceCommand.ChangeCanExecute();
        }

        private bool CanSetSubject()
        {
            return IsTaughtAtStudent || IsTaughtAtTutor || IsTaughtOnline;
        }

        private async Task OnSetSubjectPlace()
        {
            if (PopupNavigation.Instance.PopupStack.Count == 0)
                return;

            SetPlace();
            MessagingCenter.Send(this, MessagingCenterConstans.SubjectPlaceSelected);

            await PopupNavigation.Instance.PopAsync();
        }

        private void SetPlace()
        {
            if (IsTaughtAtTutor && IsTaughtAtStudent && IsTaughtOnline)
                Place = SubjectPlace.All;
            else if (!IsTaughtAtTutor && IsTaughtAtStudent && IsTaughtOnline)
                Place = SubjectPlace.AtStudentAndOnline;
            else if (IsTaughtAtTutor && !IsTaughtAtStudent && IsTaughtOnline)
                Place = SubjectPlace.AtTutorAndOnline;
            else if (IsTaughtAtTutor && IsTaughtAtStudent && !IsTaughtOnline)
                Place = SubjectPlace.AtTutorAndAtStudent;
            else if (!IsTaughtAtTutor && !IsTaughtAtStudent && IsTaughtOnline)
                Place = SubjectPlace.Online;
            else if (IsTaughtAtTutor && !IsTaughtAtStudent && !IsTaughtOnline)
                Place = SubjectPlace.AtTutor;
            else if (!IsTaughtAtTutor && IsTaughtAtStudent && !IsTaughtOnline)
                Place = SubjectPlace.AtStudent;
        }

        private void OnTaughtOnline()
        {
            SetCheckBoxActivity(new SubjectPlaceCheckBoxActivity(true, false, false));
        }

        private void OnTaughtAtStudent()
        {
            SetCheckBoxActivity(new SubjectPlaceCheckBoxActivity(false, false, true));
        }

        private void OnTaughtAtTutor()
        {
            SetCheckBoxActivity(new SubjectPlaceCheckBoxActivity(false, true, false));
        }

        private void SetCheckBoxActivity(SubjectPlaceCheckBoxActivity activity)
        {
            IsTaughtOnline = activity.IsTaughtOnline;
            IsTaughtAtTutor = activity.IsTaughtAtTutor;
            IsTaughtAtStudent = activity.IsTaughtAtStudent;
        }
    }
}
