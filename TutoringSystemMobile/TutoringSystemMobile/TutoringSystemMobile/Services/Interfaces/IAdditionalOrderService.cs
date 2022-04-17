using System.Threading.Tasks;
using TutoringSystemMobile.Models.Dtos.AdditionalOrder;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Parameters;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IAdditionalOrderService
    {
        Task<OrdersCollectionDto> GetOrdersAsync(AdditionalOrderParameters parameters);
        Task<OrderDetailsDto> GetOrderByIdAsync(long orderId);
        Task<long> AddOrderAsync(NewOrderDto newOrder);
        Task<bool> UpdateOrderAsync(UpdatedOrderDto updatedOrder);
        Task<bool> RemoveOrderAsync(long orderId);
        Task<bool> ChangeOrderStatusAsync(long orderId, AdditionalOrderStatus status);
        Task<bool> ChangePaymentStatusAsync(long orderId, bool isPaid);
    }
}
