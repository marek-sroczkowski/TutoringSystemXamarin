using System;
using TutoringSystemMobile.Models.Enums;

namespace TutoringSystemMobile.Models.AdditionalOrderDtos
{
    public class NewOrderDto
    {
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public bool IsPaid { get; set; }
        public AdditionalOrderStatus Status { get; set; }

        public NewOrderDto()
        {
        }

        public NewOrderDto(string name, DateTime deadline, string description, double cost, bool isPaid)
        {
            Name = name;
            Deadline = deadline;
            Description = description;
            Cost = cost;
            IsPaid = isPaid;
            Status = AdditionalOrderStatus.Pending;
        }

        public NewOrderDto(string name, DateTime deadline, string description, double cost, bool isPaid, AdditionalOrderStatus status) : this(name, deadline, description, cost, isPaid)
        {
            Status = status;
        }
    }
}
