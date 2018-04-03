using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ninject.Modules;
using CinemaBLL.Services;
using CinemaBLL.Services.Interfaces;
using CinemaMongoDb.Infrastructure;
using CinemaMongoDb.Models;
using CinemaDAL.Sorting;
using System.Threading.Tasks;
using CinemaBLL.DTO;

namespace CinemaMongoDb.Controllers
{
    public class MovieController : ApiController
    {
        private IMovieService _service;

        [HttpGet]
        public async Task<MoviesWithPagination> GetMovies(int page, int rows, string orderProperty = "Name", string order = "asc")
        {
            var orderEnum = SortOrderConverter.GetSortOrderFromString(order);
            Tuple<IList<MovieDTO>, PaginationInfo> tupleMovieAndPagination = await _service.GetMovies(page, rows, orderProperty, orderEnum);
            MoviesWithPagination response = new MoviesWithPagination();
            response.Movies = PLMapperConfigurer.Mapper.Map<IList<MovieDTO>, List<MoviePL>>(tupleMovieAndPagination.Item1);
            response.PaginationInfo = tupleMovieAndPagination.Item2;
            return response;
        }

        public MovieController(IMovieService service)
        {
            _service = service;
        }
     
    }
}
