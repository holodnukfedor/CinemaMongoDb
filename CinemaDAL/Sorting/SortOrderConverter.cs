using System;

namespace CinemaDAL.Sorting
{
    public class SortOrderConverter
    {
        public static SortOrder GetSortOrderFromString(String sortOrder)
        {
            if (sortOrder.ToLower() == "asc") return SortOrder.Asc;
            return SortOrder.Desc;
        }
    }
}
