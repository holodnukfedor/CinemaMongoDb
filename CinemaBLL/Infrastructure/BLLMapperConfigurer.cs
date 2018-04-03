using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CinemaDAL.Entities;
using CinemaBLL.DTO;

namespace CinemaBLL.Infrastructure
{
    public class BLLMapperConfigurer
    {
        private static MapperConfiguration _mapConfig;

        public static IMapper Mapper { get; private set; }

        static BLLMapperConfigurer()
        {
            _mapConfig = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Movie, MovieDTO>();
                    cfg.CreateMap<MovieDTO, Movie>();
                }
            );

            Mapper = _mapConfig.CreateMapper();
        }
    }
}
