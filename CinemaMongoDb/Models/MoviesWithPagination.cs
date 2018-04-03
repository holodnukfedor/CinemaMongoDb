using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CinemaBLL.DTO;

namespace CinemaMongoDb.Models
{
    public class MoviesWithPagination
    {
        public List<MoviePL> Movies { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}