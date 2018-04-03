using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaDAL.Entities;

namespace CinemaDAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IAsyncRepository<Movie> Movies { get; }
    }
}
