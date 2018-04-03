using System;
using CinemaBLL.DTO;
using System.Collections.Generic;
using CinemaDAL.Sorting;
using System.Threading.Tasks;

namespace CinemaBLL.Services.Interfaces
{
    public interface IMovieService
    {
        Task<Tuple<IList<MovieDTO>, PaginationInfo>> GetMovies(int page, int amount, string sortPropertyName, SortOrder sortOrder);
        Task AddMovie(MovieDTO movie);
        Task<long> Count();
    }
}
