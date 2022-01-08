//Create all the params for setting up pagination

namespace Application.Core
{
    public class PagingParams
    {
        private const int MaxPageSize = 5;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 5;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}