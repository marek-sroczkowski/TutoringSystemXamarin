﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.AdditionalOrderDtos;
using TutoringSystemMobile.Models.Enums;
using TutoringSystemMobile.Models.Parameters;

namespace TutoringSystemMobile.Services.Interfaces
{
    public interface IAdditionalOrderService
    {
        Task<IEnumerable<OrderDto>> GetAdditionalOrdersAsync(AdditionalOrderParameters parameters);
        Task<OrderDetailsDto> GetAdditionalOrderByIdAsync(long orderId);
        Task<long> AddAdditionalOrderAsync(NewOrderDto newOrder);
        Task<bool> UpdateAdditionalOrderAsync(UpdatedOrderDto updatedOrder);
        Task<bool> DeleteAdditionalOrderAsync(long orderId);
        Task<bool> ChangeOrderStatusAsync(long orderId, AdditionalOrderStatus status);
        Task<bool> ChangePaymentStatusAsync(long orderId, bool isPaid);
    }
}
