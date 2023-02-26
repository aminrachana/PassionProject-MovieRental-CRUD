using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject.Models
{
    public class Rental
    {
        [Key]
        public int RentalID { get; set; }

        public string RFName { get; set; }

        public string RLName { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime ReturnDate { get; set; }

        [ForeignKey("Movie")]
        public int MovieID { get; set; }
        public virtual Movie Movie { get; set;  }
    }

    public class RentalDto
    {
        public int RentalID { get; set; }

        public string RFName { get; set; }

        public string RLName { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public string MovieName { get; set; }
    }
}