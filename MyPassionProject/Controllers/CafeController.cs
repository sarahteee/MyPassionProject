﻿using MyPassionProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyPassionProject.Controllers
{
    public class CafeController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();


        static CafeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44321/api/cafedata/ ");
        }

        // GET: Cafe/List
        public ActionResult List()
        {
            //objective: with our cafe data api to retrieve a list of cafes
            //curl https://localhost:44321/api/cafedata/listcafes

            string url = "listcafes";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CafeDto> cafes = response.Content.ReadAsAsync<IEnumerable<CafeDto>>().Result;

            //returns Views/Cafe/List.cshtml
            return View(cafes);

        }

        // GET: Cafe/Details/5
        public ActionResult Details(int id)
        {
            DetailsCafe ViewModel = new DetailsCafe();

            //objective: with our cafe data api to retrieve one cafe
            //curl https://localhost:44321/api/cafedata/findcafe/{id}

            string url = "findcafe/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CafeDto selectedcafe = response.Content.ReadAsAsync<CafeDto>().Result;

            ViewModel.selectedcafe = selectedcafe;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Cafe/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Cafe/Create
        [HttpPost]
        public ActionResult Create(Cafe cafe)
        {
            //objective: add a new cafe into the system using the API
            //curl -H "Content-Type:application/json" -d @cafe.json https://localhost:44321/api/cafedata/addcafe
            string url = "addcafe";

            string jsonpayload = jss.Serialize(cafe);


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
        public ActionResult Edit(int id)


            UpdateCafe ViewModel = new UpdateCafe();

            string url = "findcafe/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CafeDto selectedcafe = response.Content.ReadAsAsync<CafeDto>().Result;
            ViewModel.selectedcafe = selectedcafe;
            return View(selectedcafe);
        }

        // POST: Cafe/Update/5
        [HttpPost]
        public ActionResult Update(int id, Cafe cafe)
        {
            string url = "updatecafe/" + id;
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
            string url = "findcafe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CafeDto selectedcafe = response.Content.ReadAsAsync<CafeDto>().Result;
            return View(selectedcafe);
        }

        // POST: Cafe/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "deletecafe/" + id;
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
