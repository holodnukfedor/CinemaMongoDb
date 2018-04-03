using System;
using AutoMapper;
using CinemaBLL.DTO;
using CinemaMongoDb.Models;

namespace CinemaMongoDb.Infrastructure
{
    public class PLMapperConfigurer
    {
        private static MapperConfiguration _mapConfig;

        public static IMapper Mapper { get; private set; }

        public static string DateAndTimeFormat { get; set; }

        static PLMapperConfigurer()
        {
            DateAndTimeFormat = "dd.MM.yyyy";

            _mapConfig = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<MovieDTO, MoviePL>()
                        .ForMember(destinationMember => destinationMember.ReleaseDate, opt => opt.MapFrom(p => p.ReleaseDate.ToString(DateAndTimeFormat)));
                }
                );

            Mapper = _mapConfig.CreateMapper();
        }
    }
}