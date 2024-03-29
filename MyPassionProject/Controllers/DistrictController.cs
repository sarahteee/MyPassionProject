﻿using System;
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
    public class DistrictController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DistrictController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44321/api/");
        }

        // GET: District/List
        public ActionResult List(string SearchKey = null)
        {
            //objective: communicate with our district data api to retrieve a list of districts
            //curl https://localhost:44321/api/districtdata/listdistricts

            //DetailsDistrict ViewModel = new DetailsDistrict();

            string url = "districtdata/listdistricts/" + SearchKey;
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DistrictDto> Districts = response.Content.ReadAsAsync<IEnumerable<DistrictDto>>().Result;
                        

            return View(Districts);
        }

        // GET: District/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our district data api to retrieve one district
            //curl https://localhost:44321/api/districtdata/finddistricts/{id}

            DetailsDistrict ViewModel = new DetailsDistrict();

            string url = "districtdata/finddistrict/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DistrictDto SelectedDistrict = response.Content.ReadAsAsync<DistrictDto>().Result;

            ViewModel.SelectedDistrict = SelectedDistrict;

            url = "cafedata/ListCafesForDistrict/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CafeDto> RelatedCafes = response.Content.ReadAsAsync<IEnumerable<CafeDto>>().Result;

            ViewModel.RelatedCafes = RelatedCafes;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: District/New
        public ActionResult New()
        {
            return View();
        }

        // POST: District/Create
        [HttpPost]
        public ActionResult Create(District District)
        {
            //objective: add a new district into our system using the API
            //curl -H "Content-Type:application/json" -d @District.json https://localhost:44324/api/districtdata/addDistrict 
            string url = "districtdata/adddistrict";


            string jsonpayload = jss.Serialize(District);

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

        // POST: District/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "districtdata/finddistrict/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DistrictDto SelectedDistrict = response.Content.ReadAsAsync<DistrictDto>().Result;
            return View(SelectedDistrict);
        }

        // POST: District/Update/5
        [HttpPost]
        public ActionResult Update(int id, District District)
        {

            string url = "districtdata/updatedistrict/" + id;
            string jsonpayload = jss.Serialize(District);
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

        // GET: District/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "districtdata/finddistrict/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DistrictDto SelectedDistrict = response.Content.ReadAsAsync<DistrictDto>().Result;
            return View(SelectedDistrict);
        }

        // POST: District/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "districtdata/deletedistrict/" + id;
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
