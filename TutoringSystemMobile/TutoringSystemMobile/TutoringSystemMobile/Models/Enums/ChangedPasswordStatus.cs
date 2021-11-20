namespace TutoringSystemMobile.Models.Enums
{
    public enum WrongPasswordStatus
    {
        InternalError = -1,
        PasswordsVary,
        TooShort,
        DuplicateOfOld,
        InvalidOldPassword
    }
}
