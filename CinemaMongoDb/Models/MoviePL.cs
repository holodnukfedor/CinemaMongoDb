using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemaMongoDb.Models
{
    public class MoviePL
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ReleaseDate { get; set; }

        public int Rating { get; set; }
    }
}