using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaDAL.Repositories.Interfaces;
using CinemaDAL.Entities;
using CinemaDAL.Context;

namespace CinemaDAL.Repositories
{
    public class MongoUnitOfWork : IUnitOfWork
    {
        private CinemaDbContext _dbContext;

        private MovieRepository _movieRepository;

        public IAsyncRepository<Movie> Movies 
        {
            get { return _movieRepository ?? (_movieRepository = new MovieRepository(_dbContext)); } 
        }

        public MongoUnitOfWork(string connectionString)
        {
            _dbContext = new CinemaDbContext(connectionString);
        }
    }
}
