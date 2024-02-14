using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using MyPassionProject.Models.ViewModels;
using System.Web.Script.Serialization;
using MyPassionProject.Models;

namespace MyPassionProject.Controllers
{
    public class AmenityController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AmenityController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44321/api/");
        }

            // GET: Amenity/List
            public ActionResult List()
        {
            //objective: communicate with our Amnity data api to retrieve a list of amenities
            //curl https://localhost:44321/api/amenitydata/listamenities


            string url = "amenitydata/listamenities";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<AmenityDto> Amenities = response.Content.ReadAsAsync<IEnumerable<AmenityDto>>().Result;


            return View(Amenities);
        }

        // GET: Amenity/Details/5
        public ActionResult Details(int id)
        {
            DetailsAmenity ViewModel = new DetailsAmenity();

            //objective: communicate with our amenity data api to retrieve one one amenity
            //curl https://localhost:44321/api/amenitydata/findamenity/{id}

            string url = "amenitydata/findamenity/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            AmenityDto SelectedAmenity = response.Content.ReadAsAsync<AmenityDto>().Result;

            ViewModel.SelectedAmenity = SelectedAmenity;

            //show all amenities available in single cafe
            url = "cafedata/listcafesforamenity/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CafeDto> RelevantCafes = response.Content.ReadAsAsync<IEnumerable<CafeDto>>().Result;

            ViewModel.RelevantCafes = RelevantCafes;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Keeper/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Amenity/Create
        [HttpPost]
        public ActionResult Create(Amenity Amenity )
        {

            //objective: add a new amenity into our system using the API
            //curl -H "Content-Type:application/json" -d @Amenity.json https://localhost:44321/api/amenitydata/addamenity 
            string url = "amenitydata/addamenity";


            string jsonpayload = jss.Serialize(Amenity);

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


        // GET: Amenity/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "amenitydata/findamenity/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AmenityDto SelectedAmenity = response.Content.ReadAsAsync<AmenityDto>().Result;
            return View(SelectedAmenity);
        }

        // POST: Amenity/Update/5
        [HttpPost]
        public ActionResult Update(int id, Amenity Amenity)
        {
            string url = "amenitydata/updateamenity/" + id;
            string jsonpayload = jss.Serialize(Amenity);
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

        // GET: Amenity/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "amenitydata/findamenity/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AmenityDto SelectedAmenity = response.Content.ReadAsAsync<AmenityDto>().Result;
            return View(SelectedAmenity);
        }

        // POST: Amenity/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "amenitydata/deleteamenity/" + id;
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
