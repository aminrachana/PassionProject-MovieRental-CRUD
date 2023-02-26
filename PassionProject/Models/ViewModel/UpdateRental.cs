using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModel
{
    public class UpdateRental
    {
        public RentalDto SelectedRental { get; set; }

        public IEnumerable<MovieDto> MoviesOptions { get; set; }
    }
}