using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TutoringSystemMobile.Models.AdditionalOrderDtos;
using TutoringSystemMobile.Models.Parameters;
using TutoringSystemMobile.Services.Interfaces;

namespace TutoringSystemMobile.Services.Web
{
    public class AdditionalOrderService : IAdditionalOrderService
    {
        public Task<OrderDto> AddAdditionalOrderAsync(NewOrderDto newOrder)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAdditionalOrderAsync(long orderId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDetailsDto> GetAdditionalOrderByIdAsync(long orderId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<OrderDto>> GetAdditionalOrdersAsync(AdditionalOrderParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetInProgressStatusAsync(long orderId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetRealizedStatusAsync(long orderId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAdditionalOrderAsync(UpdatedOrderDto updatedOrder)
        {
            throw new NotImplementedException();
        }
    }
}
