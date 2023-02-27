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

        /// <summary>
        /// Returns a list of rentals in the rentals database
        /// </summary>
        /// <returns>
        /// A list of Rental Objects mapped to the database column values (RentalID, RFName, RLName, PurchaseDate, ReturnDate).
        /// </returns>
        /// <example>GET api/RentalData/ListRentals -> {Rental Object}</example>

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

        /// <summary>
        /// Finds an rental from the MySQL Database through an id. | Non-Deterministic.
        /// </summary>
        /// <param name="id">The Rental ID</param>
        /// <returns>Rental object containing information about the rental with a matching ID. Empty Rental Object if the ID does not match any rentals in the system.</returns>
        /// <example>api/RentalData/FindRental/5 -> {Rental Object}</example>
        
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

        /// <summary>
        /// Updates an Rental on the MySQL Database. 
        /// </summary>
        /// <param name="rental">An object with fields that map to the columns of the Rental's table.</param>
        /// <example>
        /// POST api/RentalData/UpdateRental/5
        
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

        /// <summary>
        /// Adds a Rental to the MySQL Database.
        /// </summary>
        /// <param name="rental">An object with fields that map to the columns of the rental's table.</param>
        /// <example>
        /// POST api/RentalData/AddRental
        
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

        /// <summary>
        /// Deletes a rental from the database if the ID of that rental exists.
        /// </summary>
        /// <param name="id">The ID of the rental.</param>
        /// <example> POST : /api/RentalData/DeleteRental/5</example>
        
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