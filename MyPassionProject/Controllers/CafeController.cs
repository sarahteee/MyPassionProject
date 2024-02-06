using MyPassionProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MyPassionProject.Controllers
{
    public class CafeController : Controller
    {
        // GET: Cafe/List
        public ActionResult List()
        {
            //objective: with our cafe data api to retrieve a list of cafes
            //curl https://localhost:44321/api/cafedata/listcafes

            HttpClient client = new HttpClient() { };
            string url = "https://localhost:44321/api/cafedata/listcafes";
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

            HttpClient client = new HttpClient() { };
            string url = "https://localhost:44321/api/cafedata/findcafe/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CafeDto selectedcafe = response.Content.ReadAsAsync<CafeDto>().Result;

            return View(selectedcafe);
        }

        // GET: Cafe/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cafe/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
