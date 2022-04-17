using Flurl.Http;
using System;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Extensions
{
    public static class FlurlExtensions
    {
        public static async Task<AddStudentToTutorStatus> GetAddedStudentStatusAsync(this IFlurlResponse response)
        {
            var status = await response.GetStringAsync();

            return (AddStudentToTutorStatus)Enum.Parse(typeof(AddStudentToTutorStatus), status.Trim('\"'));
        }

        public static async Task<AddTutorToStudentStatus> GetAddedTutorStatusAsync(this IFlurlResponse response)
        {
            var status = await response.GetStringAsync();

            return (AddTutorToStudentStatus)Enum.Parse(typeof(AddTutorToStudentStatus), status.Trim('\"'));
        }

        public static async Task<Role> GetUserRole(this IFlurlResponse response)
        {
            var role = await response.GetStringAsync();

            return (Role)Enum.Parse(typeof(Role), role.Trim('\"'));
        }
    }
}