namespace TutoringSystemMobile.Models.Parameters
{
    public abstract class QueryStringParameters
    {
        private const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int pageSize = 10;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        public string OrderBy { get; set; }

        public QueryStringParameters()
        {
        }

        protected QueryStringParameters(int pageNumber, int pageSize, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            OrderBy = orderBy;
        }
    }
}
