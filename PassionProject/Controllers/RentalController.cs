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
    public class RentalController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static RentalController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        // GET: Rental/List
        public ActionResult List()
        {
            string url = "RentalData/ListRentals";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<RentalDto> rentals = response.Content.ReadAsAsync<IEnumerable<RentalDto>>().Result;

            return View(rentals);
        }

        // GET: Rental/Details/5
        public ActionResult Details(int id)
        {
            string url = "RentalData/FindRental/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            RentalDto selectedRental = response.Content.ReadAsAsync<RentalDto>().Result;

            return View(selectedRental);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Rental/New
        public ActionResult New()
        {
            string url = "MovieData/ListMovies";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<MovieDto> MoviesOptions = response.Content.ReadAsAsync<IEnumerable<MovieDto>>().Result;

            return View(MoviesOptions);
        }

        // POST: Rental/Create
        [HttpPost]
        public ActionResult Create(Rental rental)
        {
            string url = "RentalData/AddRental";
            string jsonpayload = jss.Serialize(rental);

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

        // GET: Rental/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateRental ViewModel = new UpdateRental();

            string url = "RentalData/FindRental/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RentalDto SelectedRental = response.Content.ReadAsAsync<RentalDto>().Result;
            ViewModel.SelectedRental = SelectedRental;

            url = "MovieData/ListMovies";
            response = client.GetAsync(url).Result;
            IEnumerable<MovieDto> MoviesOptions = response.Content.ReadAsAsync<IEnumerable<MovieDto>>().Result;

            ViewModel.MoviesOptions = MoviesOptions;

            return View(ViewModel);
        }

        // POST: Rental/Update/5
        [HttpPost]
        public ActionResult Update(int id, Rental rental)
        {
            string url = "RentalData/UpdateRental/" + id;
            string jsonpayload = jss.Serialize(rental);
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

        // GET: Rental/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "RentalData/FindRental/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RentalDto SelectedRental = response.Content.ReadAsAsync<RentalDto>().Result;

            return View(SelectedRental);
        }

        // POST: Rental/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "RentalData/DeleteRental/" + id;
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
