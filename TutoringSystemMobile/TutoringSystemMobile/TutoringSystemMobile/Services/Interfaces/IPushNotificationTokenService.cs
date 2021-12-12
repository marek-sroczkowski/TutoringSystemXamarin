using System.Threading.Tasks;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IPushNotificationTokenService
    {
        Task<bool> PutTokenAsync();
    }
}
