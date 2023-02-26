using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModel
{
    public class DetailsMovie
    {
        public MovieDto SelectedMovie { get; set; }

        public IEnumerable<RentalDto> RelatedRentals { get; set; }
    }
}