using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.AdditionalOrderDtos;
using TutoringSystemMobile.Models.Parameters;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IAdditionalOrderService
    {
        Task<IEnumerable<OrderDto>> GetAdditionalOrdersAsync(AdditionalOrderParameters parameters);
        Task<OrderDetailsDto> GetAdditionalOrderByIdAsync(long orderId);
        Task<bool> AddAdditionalOrderAsync(NewOrderDto newOrder);
        Task<bool> UpdateAdditionalOrderAsync(UpdatedOrderDto updatedOrder);
        Task<bool> DeleteAdditionalOrderAsync(long orderId);
        Task<bool> SetInProgressStatusAsync(long orderId);
        Task<bool> SetRealizedStatusAsync(long orderId);
    }
}
