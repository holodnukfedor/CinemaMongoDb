using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaDAL.Sorting;

namespace CinemaDAL.Repositories.Interfaces
{
    public interface IAsyncRepository<T> where T : class
    {
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(string id);
        Task<IEnumerable<T>> GetAmount(int fromRow, int amount, string orderPropertyName = null, SortOrder sortOrder = SortOrder.Asc);
        Task<T> Get(string id);
        Task<long> Count();
        Task<long> DeleteAll();
    }
}
