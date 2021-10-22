namespace TutoringSystemMobile.Models.Enums
{
    public enum WrongPasswordStatus
    {
        DatabaseError = -1,
        PasswordsVary,
        TooShort,
        DuplicateOfOld,
        InvalidOldPassword
    }
}
