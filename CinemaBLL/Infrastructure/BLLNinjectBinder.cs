using System;
using Ninject;
using CinemaDAL.Repositories;
using CinemaDAL.Repositories.Interfaces;

namespace CinemaBLL.Infrastructure
{
    public class BLLNinjectBinder
    {
        public static void Init(IKernel kernel, string connectionString)
        {
            kernel.Bind<IUnitOfWork>().To<MongoUnitOfWork>().WithConstructorArgument(connectionString);
        }
    }
}
