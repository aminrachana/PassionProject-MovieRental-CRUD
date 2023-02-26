using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProject.Models;
using System.Diagnostics;

namespace PassionProject.Controllers
{
    public class RentalDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/RentalData/ListRentals
        [HttpGet]
        public IEnumerable<RentalDto> ListRentals()
        {
            List<Rental> Rentals = db.Rentals.ToList();
            List<RentalDto> RentalDtos = new List<RentalDto>();

            Rentals.ForEach(Rental => RentalDtos.Add(new RentalDto()
            {
                RentalID = Rental.RentalID,
                RFName = Rental.RFName,
                RLName = Rental.RLName,
                PurchaseDate = Rental.PurchaseDate,
                ReturnDate = Rental.ReturnDate,
                MovieName = Rental.Movie.MovieName
            }));

            return RentalDtos;
        }

        // GET: api/RentalData/ListRentalsForMovie/3
        [HttpGet]
        public IEnumerable<RentalDto> ListRentalsForMovie(int id)
        {
            List<Rental> Rentals = db.Rentals.Where(a=> a.MovieID == id).ToList();
            List<RentalDto> RentalDtos = new List<RentalDto>();

            Rentals.ForEach(Rental => RentalDtos.Add(new RentalDto()
            {
                RentalID = Rental.RentalID,
                RFName = Rental.RFName,
                RLName = Rental.RLName,
                PurchaseDate = Rental.PurchaseDate,
                ReturnDate = Rental.ReturnDate,
                MovieName = Rental.Movie.MovieName
            }));

            return RentalDtos;
        }

        // GET: api/RentalData/FindRental/5
        [ResponseType(typeof(Rental))]
        [HttpGet]
        public IHttpActionResult FindRental(int id)
        {
            Rental Rental = db.Rentals.Find(id);
            RentalDto RentalDto = new RentalDto()
            {
                RentalID = Rental.RentalID,
                RFName = Rental.RFName,
                RLName = Rental.RLName,
                PurchaseDate = Rental.PurchaseDate,
                ReturnDate = Rental.ReturnDate,
                MovieName = Rental.Movie.MovieName
            };
            if (Rental == null)
            {
                return NotFound();
            }

            return Ok(RentalDto);
        }

        // PUT: api/RentalData/UpdateRental/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateRental(int id, Rental rental)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rental.RentalID)
            {
                return BadRequest();
            }

            db.Entry(rental).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/RentalData/AddRental
        [ResponseType(typeof(Rental))]
        [HttpPost]
        public IHttpActionResult AddRental(Rental rental)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rentals.Add(rental);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = rental.RentalID }, rental);
        }

        // DELETE: api/RentalData/DeleteRental/5
        [ResponseType(typeof(Rental))]
        [HttpPost]
        public IHttpActionResult DeleteRental(int id)
        {
            Rental rental = db.Rentals.Find(id);
            if (rental == null)
            {
                return NotFound();
            }

            db.Rentals.Remove(rental);
            db.SaveChanges();

            return Ok(rental);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RentalExists(int id)
        {
            return db.Rentals.Count(e => e.RentalID == id) > 0;
        }
    }
}