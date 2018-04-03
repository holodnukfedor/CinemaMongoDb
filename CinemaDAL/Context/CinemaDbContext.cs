using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using CinemaDAL.Entities;
using MongoDB.Bson.Serialization;
using System.Configuration;

namespace CinemaDAL.Context
{
    public class CinemaDbContext
    {
        IMongoDatabase _database;

        private void SeedMoviesByTestData()
        {
            string pathToDbInitializeData = ConfigurationManager.AppSettings["pathToDbInitializeData"];

            if (String.IsNullOrWhiteSpace(pathToDbInitializeData))
                return;

            string bsonText = System.IO.File.ReadAllText(pathToDbInitializeData);
            List<BsonDocument> documents = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<List<BsonDocument>>(bsonText);
            List<Movie> movies = new List<Movie>(documents.Count);
            foreach (var doc in documents)
            {
                var movie = BsonSerializer.Deserialize<Movie>(doc);
                movies.Add(movie);
            }
            Movies.InsertMany(movies);
        }

        public IMongoCollection<Movie> Movies
        {
            get { return _database.GetCollection<Movie>("Movies"); }
        }

        public CinemaDbContext(string connectionString)
        {
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            _database = client.GetDatabase(connection.DatabaseName);

            if (Movies.Count(new BsonDocument()) == 0)
            {
                SeedMoviesByTestData();
            }

        }
    }
}
