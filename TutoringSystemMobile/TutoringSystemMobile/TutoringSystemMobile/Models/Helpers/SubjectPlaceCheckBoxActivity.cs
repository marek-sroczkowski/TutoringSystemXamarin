namespace TutoringSystemMobile.Models.Helpers
{
    public class SubjectPlaceCheckBoxActivity
    {
        public bool IsTaughtOnline { get; set; }
        public bool IsTaughtAtTutor { get; set; }
        public bool IsTaughtAtStudent { get; set; }

        public SubjectPlaceCheckBoxActivity()
        {
        }

        public SubjectPlaceCheckBoxActivity(bool isTaughtOnline, bool isTaughtAtTutor, bool isTaughtAtStudent)
        {
            IsTaughtOnline = isTaughtOnline;
            IsTaughtAtTutor = isTaughtAtTutor;
            IsTaughtAtStudent = isTaughtAtStudent;
        }
    }
}
