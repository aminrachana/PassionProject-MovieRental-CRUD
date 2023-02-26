using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using PassionProject.Models;
using PassionProject.Models.ViewModel;
using System.Web.Script.Serialization;

namespace PassionProject.Controllers
{
    public class MovieController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static MovieController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        // GET: Movie/List
        public ActionResult List()
        {
            string url = "MovieData/ListMovies";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<MovieDto> movies = response.Content.ReadAsAsync<IEnumerable<MovieDto>>().Result;

            return View(movies);
        }

        // GET: Movie/Details/5
        public ActionResult Details(int id)
        {
            DetailsMovie ViewModel = new DetailsMovie();

            string url = "MovieData/FindMovie/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            MovieDto Selectedmovie = response.Content.ReadAsAsync<MovieDto>().Result;

            ViewModel.SelectedMovie = Selectedmovie;

            url = "RentalData/ListRentalsForMovie/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<RentalDto> RelatedRentals = response.Content.ReadAsAsync<IEnumerable<RentalDto>>().Result;

            ViewModel.RelatedRentals = RelatedRentals;

            return View(ViewModel);
        }
        
        public ActionResult Error()
        {
            return View();
        }

        // GET: Movie/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            string url = "MovieData/AddMovie";
            string jsonpayload = jss.Serialize(movie);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Movie/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "MovieData/FindMovie/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MovieDto selectedMovie = response.Content.ReadAsAsync<MovieDto>().Result;
            return View(selectedMovie);
        }

        // POST: Movie/Update/5
        [HttpPost]
        public ActionResult Update(int id, Movie movie)
        {
            string url = "MovieData/UpdateMovie/" + id;
            string jsonpayload = jss.Serialize(movie);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Movie/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "MovieData/FindMovie/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MovieDto selectedMovie = response.Content.ReadAsAsync<MovieDto>().Result;
            return View(selectedMovie);
        }

        // POST: Movie/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "MovieData/DeleteMovie/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
