using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaBLL.Services;
using CinemaBLL.Services.Interfaces;
using CinemaDAL.Repositories;
using System.Configuration;
using System.IO;
using CinemaDAL.Entities;
using System.Collections.Generic;
using MongoDB;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using CinemaBLL.Infrastructure;
using CinemaBLL.DTO;
using CinemaDAL.Repositories.Interfaces;
using CinemaMongoDb.Controllers;
using CinemaMongoDb.Models;
using CinemaMongoDb.Infrastructure;
using System.Linq;

namespace CinemaBLLTest
{
    [TestClass]
    public class MoviePaginationTest
    {
        private IUnitOfWork _unitOfWork;

        private IMovieService _service;

        private List<MoviePL> _movies = new List<MoviePL>();

        /// <summary>
        /// Заполнить БД тестовыми данными
        /// Предполагаетя для тестирования БД без данных чтобы заполнить ее и тестировать уже на известном наборе данных
        /// </summary>
        [TestInitialize()]
        public void SeedDbByTestData()
        {
            var countTask = _service.Count();
            countTask.Wait();

            if (countTask.Result > 0)
                throw new ArgumentException("Предполагаетя для тестирования БД без данных чтобы заполнить ее и тестировать уже на известном наборе данных. Очистите БД, вопользуйтесть неосновной рабочей БД. После выполнения тестов БД будет очищена");

            string bsonText = System.IO.File.ReadAllText("../../TestData/Movies.json");
            List<BsonDocument> documents = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<List<BsonDocument>>(bsonText);
            foreach (var doc in documents)
            {
                var movie = BsonSerializer.Deserialize<Movie>(doc);
                var movieDTO = BLLMapperConfigurer.Mapper.Map<Movie, MovieDTO>(movie);
                var moviePL = PLMapperConfigurer.Mapper.Map<MovieDTO, MoviePL>(movieDTO);
                _movies.Add(moviePL);
                _unitOfWork.Movies.Create(movie).Wait();
            }
        }

        [TestCleanup()]
        public void ClearDb()
        {
            _unitOfWork.Movies.DeleteAll().Wait();
        }

        [TestMethod]
        public void Page1Amount1()
        {
            var movieController = new MovieController(_service);
            var moviesTask = movieController.GetMovies(1, 1);
            moviesTask.Wait();
            var movie = moviesTask.Result.Movies[0];
            var pagination = moviesTask.Result.PaginationInfo;
            Assert.AreEqual(movie.Name, _movies.OrderBy(x => x.Name).First().Name);
            Assert.AreEqual(pagination.PageNumber, 1);
            Assert.AreEqual(pagination.PagesCount, _movies.Count);
            Assert.AreEqual(pagination.RowsCount, _movies.Count);
            Assert.AreEqual(pagination.AmountOnPage, 1);
        }

        [TestMethod]
        public void NegativePageNumberTest()
        {
            var movieController = new MovieController(_service);
            var moviesTask = movieController.GetMovies(1, 1);
            moviesTask.Wait();
            var movie = moviesTask.Result.Movies[0];
            var pagination = moviesTask.Result.PaginationInfo;
            Assert.AreEqual(movie.Name, _movies.OrderBy(x => x.Name).First().Name);
            Assert.AreEqual(pagination.PageNumber, 1);
            Assert.AreEqual(pagination.PagesCount, _movies.Count);
            Assert.AreEqual(pagination.RowsCount, _movies.Count);
            Assert.AreEqual(pagination.AmountOnPage, 1);
        }

        [TestMethod]
        public void GreaterPageThenCountInDbPageTest()
        {
            var movieController = new MovieController(_service);
            var moviesTask = movieController.GetMovies(50, 1);
            moviesTask.Wait();
            var movie = moviesTask.Result.Movies.Last();
            var pagination = moviesTask.Result.PaginationInfo;
            Assert.AreEqual(movie.Name, _movies.OrderBy(x => x.Name).Last().Name);
            Assert.AreEqual(pagination.PageNumber, _movies.Count);
            Assert.AreEqual(pagination.PagesCount, _movies.Count);
            Assert.AreEqual(pagination.RowsCount, _movies.Count);
            Assert.AreEqual(pagination.AmountOnPage, 1);
        }

         [TestMethod]
        public void GreaterAmountOnPageThenInDb()
        {
            var movieController = new MovieController(_service);
            var moviesTask = movieController.GetMovies(1, 100);
            moviesTask.Wait();
            var movie = moviesTask.Result.Movies.First();
            var pagination = moviesTask.Result.PaginationInfo;
            Assert.AreEqual(moviesTask.Result.Movies.Count, _movies.Count);
            Assert.AreEqual(movie.Name, _movies.OrderBy(x => x.Name).First().Name);
            Assert.AreEqual(pagination.PageNumber, 1);
            Assert.AreEqual(pagination.PagesCount, 1);
            Assert.AreEqual(pagination.RowsCount, _movies.Count);
            Assert.AreEqual(pagination.AmountOnPage, _movies.Count);
        }

        [TestMethod]
        public void NegativeAmountOnPage()
         {
             var movieController = new MovieController(_service);
             var moviesTask = movieController.GetMovies(1, -100);
             moviesTask.Wait();
             var movie = moviesTask.Result.Movies.First();
             var pagination = moviesTask.Result.PaginationInfo;
             Assert.AreEqual(moviesTask.Result.Movies.Count, MovieService.DefaultCountOnPage);
             Assert.AreEqual(movie.Name, _movies.OrderBy(x => x.Name).First().Name);
             Assert.AreEqual(pagination.PageNumber, 1);
             Assert.AreEqual(pagination.RowsCount, _movies.Count);
             Assert.AreEqual(pagination.AmountOnPage, MovieService.DefaultCountOnPage);
         }

         [TestMethod]
        public void Page0Amount100()
        {
            var movieController = new MovieController(_service);
            var moviesTask = movieController.GetMovies(0, 100);
            moviesTask.Wait();
            var movie = moviesTask.Result.Movies.First();
            var pagination = moviesTask.Result.PaginationInfo;
            Assert.AreEqual(moviesTask.Result.Movies.Count, _movies.Count);
            Assert.AreEqual(movie.Name, _movies.OrderBy(x => x.Name).First().Name);
            Assert.AreEqual(pagination.PageNumber, 1);
            Assert.AreEqual(pagination.RowsCount, _movies.Count);
            Assert.AreEqual(pagination.AmountOnPage, _movies.Count);
        }

         [TestMethod]
        public void Page0Amount0()
        {
            var movieController = new MovieController(_service);
            var moviesTask = movieController.GetMovies(0, 0);
            moviesTask.Wait();
            var movie = moviesTask.Result.Movies.First();
            var pagination = moviesTask.Result.PaginationInfo;
            Assert.AreEqual(moviesTask.Result.Movies.Count, MovieService.DefaultCountOnPage);
            Assert.AreEqual(movie.Name, _movies.OrderBy(x => x.Name).First().Name);
            Assert.AreEqual(pagination.PageNumber, 1);
            Assert.AreEqual(pagination.RowsCount, _movies.Count);
            Assert.AreEqual(pagination.AmountOnPage, MovieService.DefaultCountOnPage);
        }

        [TestMethod]
        public void Page3Amount5()
        {
            const int amount = 5;
            const int pageNumber = 3;
            var movieController = new MovieController(_service);
            var moviesTask = movieController.GetMovies(pageNumber, amount);
            moviesTask.Wait();
            var movie = moviesTask.Result.Movies.First();
            var pagination = moviesTask.Result.PaginationInfo;
            Assert.AreEqual(moviesTask.Result.Movies.Count, amount);
            Assert.AreEqual(movie.Name, _movies.OrderBy(x => x.Name).ToList()[10].Name);
            Assert.AreEqual(pagination.PageNumber, pageNumber);
            Assert.AreEqual(pagination.RowsCount, _movies.Count);
            Assert.AreEqual(pagination.AmountOnPage, amount);
        }

        public MoviePaginationTest()
        {
            _unitOfWork = new MongoUnitOfWork(ConfigurationManager.ConnectionStrings["CinemaDbTest"].ToString());
            _service = new MovieService(_unitOfWork);
        }
    }
}
