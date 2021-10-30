using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.AdditionalOrderDtos;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(AdditionalOrderService))]
namespace TutoringSystemMobile.Services.Web
{
    public class AdditionalOrderService : IAdditionalOrderService
    {
        public async Task<bool> AddAdditionalOrderAsync(NewOrderDto newOrder)
        {
            string token = await SecureStorage.GetAsync("token");
            string url = AppSettingsManager.Settings["BaseApiUrl"] + "order";
            var response = await url
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .PostJsonAsync(newOrder);

            return response.StatusCode == 200;
        }

        public async Task<bool> DeleteAdditionalOrderAsync(long orderId)
        {
            string token = await SecureStorage.GetAsync("token");
            string url = AppSettingsManager.Settings["BaseApiUrl"] + "order";
            var response = await url
                .AllowAnyHttpStatus()
                .SetQueryParam("orderId", orderId)
                .WithOAuthBearerToken(token)
                .DeleteAsync();

            return response.StatusCode == 200;
        }

        public async Task<OrderDetailsDto> GetAdditionalOrderByIdAsync(long orderId)
        {
            string token = await SecureStorage.GetAsync("token");
            string url = AppSettingsManager.Settings["BaseApiUrl"] + "order";
            var response = await url
                .AllowAnyHttpStatus()
                .SetQueryParam("orderId", orderId)
                .WithOAuthBearerToken(token)
                .GetAsync();

            var orders = response.StatusCode == 200 ? await response.GetJsonAsync<ICollection<OrderDetailsDto>>() : new List<OrderDetailsDto>();
            return orders.Count > 0 ? orders.First() : new OrderDetailsDto();
        }

        public async Task<IEnumerable<OrderDto>> GetAdditionalOrdersAsync(AdditionalOrderParameters parameters)
        {
            string token = await SecureStorage.GetAsync("token");
            string url = AppSettingsManager.Settings["BaseApiUrl"] + "order";
            var response = await url
                .AllowAnyHttpStatus()
                .SetQueryParams(parameters)
                .WithOAuthBearerToken(token)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<IEnumerable<OrderDto>>() : new List<OrderDto>();
        }

        public async Task<bool> SetInProgressStatusAsync(long orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SetRealizedStatusAsync(long orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAdditionalOrderAsync(UpdatedOrderDto updatedOrder)
        {
            string token = await SecureStorage.GetAsync("token");
            string url = AppSettingsManager.Settings["BaseApiUrl"] + "order";
            var response = await url
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .PutJsonAsync(updatedOrder);

            return response.StatusCode == 200;
        }
    }
}
