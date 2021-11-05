using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystemMobile.Extensions;
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
        private readonly string baseUrl;

        public AdditionalOrderService()
        {
            baseUrl = AppSettingsManager.Settings["BaseApiUrl"] + "order";
        }

        public async Task<long> AddAdditionalOrderAsync(NewOrderDto newOrder)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .PostJsonAsync(newOrder);

            if (response.StatusCode != 201)
                return -1;

            string location = response.Headers.FirstOrDefault("location");

            return location is null ? -1 : location.GetIdByLocation();
        }

        public async Task<bool> DeleteAdditionalOrderAsync(long orderId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment(orderId)
                .WithOAuthBearerToken(token)
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        public async Task<OrderDetailsDto> GetAdditionalOrderByIdAsync(long orderId)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment(orderId)
                .WithOAuthBearerToken(token)
                .GetAsync();

            var order = response.StatusCode == 200 ? await response.GetJsonAsync<OrderDetailsDto>() : new OrderDetailsDto();
            return order;
        }

        public async Task<IEnumerable<OrderDto>> GetAdditionalOrdersAsync(AdditionalOrderParameters parameters)
        {
            string token = await SecureStorage.GetAsync("token");
            var response = await baseUrl
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
            var response = await baseUrl
                .AllowAnyHttpStatus()
                .WithOAuthBearerToken(token)
                .PutJsonAsync(updatedOrder);

            return response.StatusCode == 200;
        }
    }
}
