using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaDAL.Repositories.Interfaces;
using CinemaDAL.Repositories;
using System.Configuration;
using CinemaDAL.Sorting;
using MongoDB.Bson;
using MongoDB.Driver;
using CinemaDAL.Entities;

namespace CinemaDALTest
{
    class Program
    {
        private static async void DoWork()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CinemaDb"].ConnectionString;

            IUnitOfWork unitOfWork = new MongoUnitOfWork(connectionString);

            var movies = await unitOfWork.Movies.GetAmount(0, 20, "Name", SortOrder.Asc);
            foreach (var movie in movies)
            {
                Console.WriteLine(String.Format("Name : {0}, Release date : {1}", movie.Name, movie.ReleaseDate.ToShortDateString()));
            }
        }

        static void Main(string[] args)
        {
            DoWork();
            Console.ReadLine();
        }
    }
}
