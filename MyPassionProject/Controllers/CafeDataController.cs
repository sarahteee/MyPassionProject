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
using MyPassionProject.Models;

namespace MyPassionProject.Controllers
{
    public class CafeDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CafeData/ListCafes
        [HttpGet]
        public IEnumerable<CafeDto> ListCafes()
        {
            //sending query to database select * from cafes
            List<Cafe> Cafes = db.Cafes.ToList();
            List<CafeDto> CafeDtos = new List<CafeDto>();

            Cafes.ForEach(c => CafeDtos.Add(new CafeDto()
            {
                CafeId = c.CafeId,
                CafeName = c.Name,
            }
            ));

            return CafeDtos;
        }

        // GET: api/CafeData/FindCafe/5
        [ResponseType(typeof(Cafe))]
        [HttpGet]
        public IHttpActionResult FindCafe(int id)
        {
            Cafe cafe = db.Cafes.Find(id);
            if (cafe == null)
            {
                return NotFound();
            }

            return Ok(cafe);
        }

        // POST: api/CafeData/UpdateCafe/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCafe(int id, Cafe cafe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cafe.CafeId)
            {
                return BadRequest();
            }

            db.Entry(cafe).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CafeExists(id))
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

        // POST: api/CafeData/AddCafe
        [ResponseType(typeof(Cafe))]
        [HttpPost]
        public IHttpActionResult AddCafe(Cafe cafe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cafes.Add(cafe);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cafe.CafeId }, cafe);
        }

        // POST: api/CafeData/DeleteCafe/5
        [ResponseType(typeof(Cafe))]
        [HttpPost]
        public IHttpActionResult DeleteCafe(int id)
        {
            Cafe cafe = db.Cafes.Find(id);
            if (cafe == null)
            {
                return NotFound();
            }

            db.Cafes.Remove(cafe);
            db.SaveChanges();

            return Ok(cafe);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CafeExists(int id)
        {
            return db.Cafes.Count(e => e.CafeId == id) > 0;
        }
    }
}