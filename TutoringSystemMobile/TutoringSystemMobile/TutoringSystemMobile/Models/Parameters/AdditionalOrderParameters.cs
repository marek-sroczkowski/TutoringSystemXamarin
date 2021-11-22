using System;

namespace TutoringSystemMobile.Models.Parameters
{
    public class AdditionalOrderParameters : QueryStringParameters
    {
        public bool IsPaid { get; set; }
        public bool IsNotPaid { get; set; }
        public bool IsInProgress { get; set; }
        public bool IsPending { get; set; }
        public bool IsRealized { get; set; }
        public DateTime ReceiptStartDate { get; set; }
        public DateTime ReceiptEndDate { get; set; }
        public DateTime DeadlineStart { get; set; }
        public DateTime DeadlineEnd { get; set; }

        public AdditionalOrderParameters()
        {
            IsPaid = true;
            IsNotPaid = true;
            IsInProgress = true;
            IsPending = true;
            IsRealized = true;
            ReceiptStartDate = DateTime.Now.AddMonths(-1);
            ReceiptEndDate = DateTime.Now;
            DeadlineStart = DateTime.Now.AddDays(-14);
            DeadlineEnd = DateTime.Now.AddMonths(1);
        }

        public AdditionalOrderParameters(bool isPaid, bool isNotPaid, bool isInProgress, bool isPending, bool isRealized, DateTime receiptStartDate, DateTime receiptEndDate, DateTime deadlineStart, DateTime deadlineEnd, int pageSize, int currentPage) : base(currentPage, pageSize)
        {
            IsPaid = isPaid;
            IsNotPaid = isNotPaid;
            IsInProgress = isInProgress;
            IsPending = isPending;
            IsRealized = isRealized;
            ReceiptStartDate = receiptStartDate;
            ReceiptEndDate = receiptEndDate;
            DeadlineStart = deadlineStart;
            DeadlineEnd = deadlineEnd;
        }
    }
}