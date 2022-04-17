using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Models.Dtos.Authentication;
using TutoringSystemMobile.Models.Enums;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TutoringSystemMobile.Helpers
{
    public class AuthenticationHelper
    {
        public static async Task StoreTokensAsync(AuthenticationResposneDto authenticationResposne)
        {
            await SecureStorage.SetAsync(SecureStorageConstans.JwtToken, authenticationResposne.JwtToken.Token);
            await SecureStorage.SetAsync(SecureStorageConstans.JwtTokenExpirationDate, authenticationResposne.JwtToken.ExpirationDate.ToString());
            await SecureStorage.SetAsync(SecureStorageConstans.RefreshToken, authenticationResposne.RefreshToken.Token);
            await SecureStorage.SetAsync(SecureStorageConstans.RefreshTokenExpirationDate, authenticationResposne.RefreshToken.ExpirationDate.ToString());
        }

        public static async Task StoreJwtTokenAsync(TokenDto token)
        {
            await SecureStorage.SetAsync(SecureStorageConstans.JwtToken, token.Token);
            await SecureStorage.SetAsync(SecureStorageConstans.JwtTokenExpirationDate, token.ExpirationDate.ToString());
        }

        public static async Task StoreRefreshTokenAsync(TokenDto token)
        {
            await SecureStorage.SetAsync(SecureStorageConstans.RefreshToken, token.Token);
            await SecureStorage.SetAsync(SecureStorageConstans.RefreshTokenExpirationDate, token.ExpirationDate.ToString());
        }

        public static async Task<TokenDto> GetJwtToken()
        {
            string token = await SecureStorage.GetAsync(SecureStorageConstans.JwtToken);
            string expirationDate = await SecureStorage.GetAsync(SecureStorageConstans.JwtTokenExpirationDate);

            return new TokenDto
            {
                Token = token,
                ExpirationDate = expirationDate.ToDateTime()
            };
        }

        public static async Task<TokenDto> GetRefreshToken()
        {
            string token = await SecureStorage.GetAsync(SecureStorageConstans.RefreshToken);
            string expirationDate = await SecureStorage.GetAsync(SecureStorageConstans.RefreshTokenExpirationDate);

            return new TokenDto
            {
                Token = token,
                ExpirationDate = expirationDate.ToDateTime()
            };
        }

        public static void Logout()
        {
            SecureStorage.Remove(SecureStorageConstans.JwtToken);
            SecureStorage.Remove(SecureStorageConstans.JwtTokenExpirationDate);
            SecureStorage.Remove(SecureStorageConstans.RefreshToken);
            SecureStorage.Remove(SecureStorageConstans.RefreshTokenExpirationDate);
            SecureStorage.Remove(SecureStorageConstans.UserName);

            Settings.LoginStatus = AccountStatus.LoggedOut;
            (Application.Current as App).MainPage = new AppShell();
        }
    }
}