using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaBLL.DTO;
using CinemaDAL.Sorting;
using CinemaDAL.Repositories.Interfaces;
using CinemaDAL.Repositories;
using CinemaBLL.Services.Interfaces;
using CinemaDAL.Entities;
using CinemaBLL.Infrastructure;

namespace CinemaBLL.Services
{
    public class MovieService : IMovieService
    {
        public const int DefaultCountOnPage = 10;

        public const string DefaultOrderProperty = "Name";

        public const SortOrder DefaultSortOrder = SortOrder.Asc;

        private IUnitOfWork _database { get; set; }

        public async Task<Tuple<IList<MovieDTO>, PaginationInfo>> GetMovies(int page, int amount = DefaultCountOnPage, string sortPropertyName = DefaultOrderProperty, SortOrder sortOrder = DefaultSortOrder)
        {
            PaginationInfo paginationInfo;
            List<MovieDTO> moviesDTO = new List<MovieDTO>();
            List<Movie> movieList;
            paginationInfo = new PaginationInfo();

            paginationInfo.RowsCount = (int) await _database.Movies.Count();

            if (paginationInfo.RowsCount == 0)
            {
                paginationInfo.PageNumber = 1;
                paginationInfo.PagesCount = 1;
                return new Tuple<IList<MovieDTO>, PaginationInfo>(moviesDTO, paginationInfo);
            }

            if (amount <= 0)
                amount = DefaultCountOnPage;

            if (amount > paginationInfo.RowsCount)
                amount = paginationInfo.RowsCount;

            paginationInfo.AmountOnPage = amount;
            paginationInfo.PagesCount = paginationInfo.RowsCount / amount;
            if (paginationInfo.RowsCount % amount > 0)
                ++paginationInfo.PagesCount;

            paginationInfo.PageNumber = page;
            if (paginationInfo.PageNumber > paginationInfo.PagesCount)
                paginationInfo.PageNumber = paginationInfo.PagesCount;

            if (paginationInfo.PageNumber <= 0)
                paginationInfo.PageNumber = 1;

            movieList = new List<Movie> (await _database.Movies.GetAmount((paginationInfo.PageNumber - 1) * amount, amount, sortPropertyName, sortOrder));
            moviesDTO = BLLMapperConfigurer.Mapper.Map<List<Movie>, List<MovieDTO>>(movieList);
            return new Tuple<IList<MovieDTO>, PaginationInfo>(moviesDTO, paginationInfo);;
        }

        public async Task AddMovie(MovieDTO movieDto)
        {
            var movie = BLLMapperConfigurer.Mapper.Map<MovieDTO, Movie>(movieDto);
            await _database.Movies.Create(movie);
        }

        public async Task<long> Count()
        {
            return await _database.Movies.Count();
        }

        public MovieService(IUnitOfWork unitOfWork)
        {
            _database = unitOfWork;
        }
    }
}
