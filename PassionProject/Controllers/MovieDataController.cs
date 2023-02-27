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
    public class MovieDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns a list of movies in the movies database
        /// </summary>
        /// <returns>
        /// A list of Movie Objects mapped to the database column values (movie name, movie genre, date of release, description, cost of renting).
        /// </returns>
        /// <example>GET api/MovieData/ListMovies -> {Movie Object, Movie Object, Movie Object...}</example>
        
        // GET: api/MovieData/ListMovies
        [HttpGet]
        public IEnumerable<MovieDto> ListMovies()
        {
            List<Movie> Movies = db.Movies.ToList();
            List<MovieDto> MovieDtos = new List<MovieDto>();

            Movies.ForEach(Movie => MovieDtos.Add(new MovieDto(){
                MovieID = Movie.MovieID,
                MovieName = Movie.MovieName,
                MovieGenre = Movie.MovieGenre,
                MovieYear = Movie.MovieYear,
                MovieCost = Movie.MovieCost,
                MovieDescription = Movie.MovieDescription
            }));

            return MovieDtos;
        }

        /// <summary>
        /// Finds an movie from the MySQL Database through an id. | Non-Deterministic.
        /// </summary>
        /// <param name="id">The Movie ID</param>
        /// <returns>Movie object containing information about the movie with a matching ID. Empty Movie Object if the ID does not match any movies in the system.</returns>
        /// <example>api/MovieData/FindMovie/5 -> {Movie Object}</example>
        
        // GET: api/MovieData/FindMovie/5
        [HttpGet]
        [ResponseType(typeof(Movie))]
        public IHttpActionResult FindMovie(int id)
        {
            Movie Movie = db.Movies.Find(id);
            MovieDto MovieDto = new MovieDto()
            {
                MovieID = Movie.MovieID,
                MovieName = Movie.MovieName,
                MovieGenre = Movie.MovieGenre,
                MovieYear = Movie.MovieYear,
                MovieCost = Movie.MovieCost,
                MovieDescription = Movie.MovieDescription
            };
            if (Movie == null)
            {
                return NotFound();
            }

            return Ok(MovieDto);
        }

        /// <summary>
        /// Updates an Movie on the MySQL Database. 
        /// </summary>
        /// <param name="movie">An object with fields that map to the columns of the Movie's table.</param>
        /// <example>
        /// POST api/MovieData/UpdateMovie/5
        
        // POST: api/MovieData/UpdateMovie/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateMovie(int id, Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movie.MovieID)
            {
                return BadRequest();
            }

            db.Entry(movie).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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
        /// Adds a Movie to the MySQL Database.
        /// </summary>
        /// <param name="movie">An object with fields that map to the columns of the movie's table.</param>
        /// <example>
        /// POST api/MovieData/AddMovie

        // POST: api/MovieData/AddMovie
        [ResponseType(typeof(Movie))]
        [HttpPost]
        public IHttpActionResult AddMovie(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Movies.Add(movie);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = movie.MovieID }, movie);
        }

        /// <summary>
        /// Deletes a movie from the database if the ID of that movie exists.
        /// </summary>
        /// <param name="id">The ID of the movie.</param>
        /// <example> POST : /api/MovieData/DeleteMovie/5</example>

        // POST: api/MovieData/DeleteMovie/5
        [ResponseType(typeof(Movie))]
        [HttpPost]
        public IHttpActionResult DeleteMovie(int id)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }

            db.Movies.Remove(movie);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MovieExists(int id)
        {
            return db.Movies.Count(e => e.MovieID == id) > 0;
        }
    }
}