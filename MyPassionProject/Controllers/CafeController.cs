using MyPassionProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using MyPassionProject.Models.ViewModels;
using System.Web.Script.Serialization;
using MyPassionProject.Migrations;

namespace MyPassionProject.Controllers
{
    public class CafeController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();


        static CafeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44321/api/");
        }

        // GET: Cafe/List
        public ActionResult List(string SearchKey = null)
        {
            //objective: with our cafe data api to retrieve a list of cafes
            //curl https://localhost:44321/api/cafedata/listcafes

            string url = "cafedata/listcafes/" + SearchKey;
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CafeDto> Cafes = response.Content.ReadAsAsync<IEnumerable<CafeDto>>().Result;

            //returns Views/Cafe/List.cshtml
            return View(Cafes);

        }

        // GET: Cafe/Details/5
        public ActionResult Details(int id)
        {
            DetailsCafe ViewModel = new DetailsCafe();

            //objective: with our cafe data api to retrieve one cafe
            //curl https://localhost:44321/api/cafedata/findcafe/{id}

            string url = "cafedata/findcafe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            CafeDto SelectedCafe = response.Content.ReadAsAsync<CafeDto>().Result;
            ViewModel.SelectedCafe = SelectedCafe;

            Debug.WriteLine("The cafe recieved is: " + SelectedCafe.CafeId);
            Debug.WriteLine(SelectedCafe.CafeName);

            //show associated amenities with this cafe
            url = "amenitydata/listamenitiesforcafe/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<AmenityDto> AvailableAmenities = response.Content.ReadAsAsync<IEnumerable<AmenityDto>>().Result;
            ViewModel.AvailableAmenities = AvailableAmenities;
            

            url = "amenitydata/listamenitiesnotincafe/" + id;
            response= client.GetAsync(url).Result;
            IEnumerable<AmenityDto> CurrentAmenities = response.Content.ReadAsAsync<IEnumerable<AmenityDto>>().Result;
            ViewModel.CurrentAmenities = CurrentAmenities;

            return View(ViewModel);
        }

        //POST: Cafe/Associate/{CafeId}/{AmenityId}
        [HttpPost]
        public ActionResult Associate (int id, int AmenityId)
        {
            Debug.WriteLine("Attempting to associate cafe :" + id + "with amenity" + AmenityId);
            
            //call api to associate cafe with amenity
            string url = "cafedata/associatecafewithamenity/" + id + "/" + AmenityId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        //Get:Cafe/Unassociate/{id}?AmenityId={amenityId}
        [HttpGet]
        public ActionResult UnAssociate(int id, int AmenityId)
        {
            Debug.WriteLine("Attempting to unassociate cafe :" + id + "with amenity" + AmenityId);

            //call api to unassociate cafe with amenity
            string url = "cafedata/unassociatecafewithamenity/" + id+ "/" +AmenityId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync (url, content).Result;

            return RedirectToAction("Details/" + id);
        }
        public ActionResult Error()
        {
            return View();
        }

        // GET: Cafe/New
        public ActionResult New()
        {
            //information about all districts in the system
            //GET api/districtdata/listdistricts

            string url = "districtdata/listdistricts/";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DistrictDto> DistrictOptions = response.Content.ReadAsAsync<IEnumerable<DistrictDto>>().Result;


            return View(DistrictOptions);
        }

        // POST: Cafe/Create
        [HttpPost]
        public ActionResult Create(Cafe Cafe)
        {
            //objective: add a new cafe into the system using the API
            //curl -H "Content-Type:application/json" -d @cafe.json https://localhost:44321/api/cafedata/addcafe
            string url = "cafedata/addcafe";

            string jsonpayload = jss.Serialize(Cafe);
            //Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode) 
            {
                return RedirectToAction("List");
            }else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Cafe/Edit/5
        public ActionResult Edit(int id) { 


            UpdateCafe ViewModel = new UpdateCafe();

            //existing cafe info
            string url = "cafedata/findcafe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CafeDto selectedcafe = response.Content.ReadAsAsync<CafeDto>().Result;
            ViewModel.selectedcafe = selectedcafe;

            //all districts to choose from when updating cafe
            //existing cafe info
            url = "districtdata/listdistricts/";
            response = client.GetAsync(url).Result;
            IEnumerable<DistrictDto> DistrictOptions = response.Content.ReadAsAsync<IEnumerable<DistrictDto>>().Result;

            ViewModel.DistrictOptions = DistrictOptions;
            return View(ViewModel);
        }

        // POST: Cafe/Update/5
        [HttpPost]
        public ActionResult Update(int id, Cafe cafe)
        {
            string url = "cafedata/updatecafe/" + id;
            string jsonpayload = jss.Serialize(cafe);
            HttpContent content= new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType= "application/json";
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

        // GET: Cafe/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "cafedata/findcafe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CafeDto selectedcafe = response.Content.ReadAsAsync<CafeDto>().Result;
            return View(selectedcafe);
        }

        // POST: Cafe/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "cafedata/deletecafe/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType= "application/json";
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
