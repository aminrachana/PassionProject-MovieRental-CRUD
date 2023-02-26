using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PassionProject.Models
{
    public class Movie
    {
        [Key]
        public int MovieID { get; set; }

        public string MovieName { get; set; }

        public string MovieGenre { get; set; }

        public int MovieYear { get; set; }

        public int MovieCost { get; set; }

        public string MovieDescription { get; set; }
    }

    public class MovieDto
    {
        public int MovieID { get; set; }

        public string MovieName { get; set; }

        public string MovieGenre { get; set; }

        public int MovieYear { get; set; }

        public int MovieCost { get; set; }

        public string MovieDescription { get; set; }
    }
}