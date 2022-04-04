using System.Threading.Tasks;
using TutoringSystemMobile.Models.Dtos.Authentication;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResposneDto> AuthenticateAsync(AuthenticationDto userModel);
    }
}