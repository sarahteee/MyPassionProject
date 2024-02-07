using MyPassionProject.Models;
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
            //objective: with our cafe data api to retrieve one cafe
            //curl https://localhost:44321/api/cafedata/findcafe/{id}

            string url = "findcafe/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CafeDto selectedcafe = response.Content.ReadAsAsync<CafeDto>().Result;

            return View(selectedcafe);
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

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(cafe);


            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            client.PostAsync(url, content);

            return RedirectToAction("List");
        }

        // GET: Cafe/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cafe/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cafe/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cafe/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
