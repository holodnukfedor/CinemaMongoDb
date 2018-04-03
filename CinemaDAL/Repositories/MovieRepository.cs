using System.Text;
using System.Threading.Tasks;
using CinemaDAL.Sorting;
using MongoDB.Bson;
using MongoDB.Driver;
using CinemaDAL.Repositories.Interfaces;
using CinemaDAL.Entities.Interfaces;
using CinemaDAL.Entities;
using CinemaDAL.Context;

namespace CinemaDAL.Repositories
{
    public class MovieRepository : AbstractMongoRepository<Movie>
    {
        private CinemaDbContext _dbContext;

        protected override string DefaultOrderProperty { get { return "Name"; } }

        protected override IMongoCollection<Movie> MongoCollection { get { return _dbContext.Movies; } }

        public MovieRepository(CinemaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
