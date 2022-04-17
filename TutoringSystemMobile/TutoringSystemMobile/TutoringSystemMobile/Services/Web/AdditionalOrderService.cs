using Flurl.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Constans;
using TutoringSystemMobile.Extensions;
using TutoringSystemMobile.Helpers;
using TutoringSystemMobile.Models.Dtos.AdditionalOrder;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Pagination;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Services.Interfaces;
using TutoringSystemMobile.Services.Web;
using Xamarin.Forms;

[assembly: Dependency(typeof(AdditionalOrderService))]
namespace TutoringSystemMobile.Services.Web
{
    public class AdditionalOrderService : IAdditionalOrderService
    {
        private readonly string baseUrl;

        public AdditionalOrderService()
        {
            baseUrl = Settings.BaseApiUrl + ServicesConstans.Order;
        }

        public async Task<long> AddOrderAsync(NewOrderDto newOrder)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .PostJsonAsync(newOrder);

            string location = response.Headers.FirstOrDefault(ServicesConstans.Location);

            return location is null ? -1 : location.GetIdByLocation();
        }

        public async Task<bool> RemoveOrderAsync(long orderId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(orderId)
                .DeleteAsync();

            return response.StatusCode == 204;
        }

        public async Task<OrderDetailsDto> GetOrderByIdAsync(long orderId)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegment(orderId)
                .GetAsync();

            return response.StatusCode == 200 ? await response.GetJsonAsync<OrderDetailsDto>() : new OrderDetailsDto();
        }

        public async Task<OrdersCollectionDto> GetOrdersAsync(AdditionalOrderParameters parameters)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .SetQueryParams(parameters)
                .GetAsync();

            return await GetOrdersAsync(response);
        }

        private static async Task<OrdersCollectionDto> GetOrdersAsync(IFlurlResponse response)
        {
            var orders = response.StatusCode == 200
                ? await response.GetJsonAsync<IEnumerable<OrderDto>>()
                : new List<OrderDto>();

            var pagination = response.StatusCode == 200
                ? JsonConvert.DeserializeObject<PaginationMetadata>(response.Headers.FirstOrDefault("X-Pagination"))
                : new PaginationMetadata();

            return new OrdersCollectionDto { Orders = orders, Pagination = pagination };
        }

        public async Task<bool> ChangeOrderStatusAsync(long orderId, AdditionalOrderStatus status)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegments(ServicesConstans.Status, orderId)
                .SetQueryParam(ServicesConstans.Status, status.ToString())
                .PatchAsync();

            return response.StatusCode == 204;
        }

        public async Task<bool> ChangePaymentStatusAsync(long orderId, bool isPaid)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .AppendPathSegments(ServicesConstans.Payment, orderId)
                .SetQueryParam(ServicesConstans.IsPaid, isPaid.ToString())
                .PatchAsync();

            return response.StatusCode == 204;
        }

        public async Task<bool> UpdateOrderAsync(UpdatedOrderDto updatedOrder)
        {
            var baseRequest = await baseUrl.BaseRequest();
            var response = await baseRequest
                .PutJsonAsync(updatedOrder);

            return response.StatusCode == 204;
        }
    }
}