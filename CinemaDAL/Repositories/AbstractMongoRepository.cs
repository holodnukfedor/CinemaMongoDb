using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaDAL.Sorting;
using MongoDB.Bson;
using MongoDB.Driver;
using CinemaDAL.Repositories.Interfaces;
using CinemaDAL.Entities.Interfaces;

namespace CinemaDAL.Repositories
{
    public abstract class AbstractMongoRepository<T> : IAsyncRepository<T> where T : class, IEntityMongoId
    {
        protected abstract string DefaultOrderProperty { get; }

        protected abstract IMongoCollection<T> MongoCollection { get; }

        public async Task Create(T entity)
        {
            await MongoCollection.InsertOneAsync(entity);
        }

        public async Task Update(T entity)
        {
            await MongoCollection.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(entity.Id)), entity);
        }

        public async Task Delete(string id)
        {
            await MongoCollection.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }

        public async Task<T> Get(string id)
        {
            return await MongoCollection.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }

        public async Task<long> Count()
        {
            return await MongoCollection.CountAsync(new BsonDocument());
        }

        public async Task<IEnumerable<T>> GetAmount(int fromRow, int amount, string orderPropertyName = null, SortOrder sortOrder = SortOrder.Asc)
        {
            if (orderPropertyName == null)
                orderPropertyName = DefaultOrderProperty;

            SortDefinition<T> sortDefinition;

            if (sortOrder == SortOrder.Asc)
                sortDefinition = Builders<T>.Sort.Ascending(orderPropertyName);
            else
                sortDefinition = Builders<T>.Sort.Descending(orderPropertyName);

            return await MongoCollection.Find(new BsonDocument()).Sort(sortDefinition).Skip(fromRow).Limit(amount).ToListAsync();
        }

        public async Task<long> DeleteAll()
        {
            var deleteResult = await MongoCollection.DeleteManyAsync(new BsonDocument());
            return deleteResult.DeletedCount;
        }
        
    }
}
