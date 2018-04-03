using System;

namespace CinemaBLL.DTO
{
    public class MovieDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int Rating { get; set; }
    }
}
