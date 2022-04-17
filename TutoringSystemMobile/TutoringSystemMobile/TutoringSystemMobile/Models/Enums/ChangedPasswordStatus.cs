namespace TutoringSystemMobile.Models.Enums
{
    public enum WrongPasswordStatus
    {
        InternalError = -1,
        PasswordsVary,
        NotMeetRequirements,
        DuplicateOfOld,
        InvalidOldPassword
    }
}