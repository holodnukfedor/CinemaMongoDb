using System;

namespace CinemaBLL.DTO
{
    public class PaginationInfo
    {
        public int PagesCount { set; get; }
        public int PageNumber { set; get; }
        public int RowsCount { set; get; }
        public int AmountOnPage { set; get; }
    }
}
