using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
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
        [ResponseType(typeof(CafeDto))]
        public IHttpActionResult ListCafes()
        {
            //sending query to database select * from cafes
            List<Cafe> Cafes = db.Cafes.ToList();
            List<CafeDto> CafeDtos = new List<CafeDto>();

            Cafes.ForEach(c => CafeDtos.Add(new CafeDto()
            {
                CafeId = c.CafeId,
                CafeName = c.Name,
                CafeAddress = c.Address,
                CafePhone = c.Phone,
                CafeDescription = c.Description,
                CafeWebsite = c.Website,
                CafeImage = c.Image,
                DistrictId = c.DistrictId,
                DistrictName = c.District.DistrictName
            }
            ));

            return Ok(CafeDtos);
        }

        [HttpGet]
        [ResponseType(typeof(CafeDto))]
        public IHttpActionResult ListCafesForDistrict(int id)
        {
            //SQL Equivalent:
            //Select * from cafes where cafes.districtid = {id}
            List<Cafe> Cafes = db.Cafes.Where(c => c.DistrictId == id).ToList();
            List<CafeDto> CafeDtos = new List<CafeDto>();

            Cafes.ForEach(c => CafeDtos.Add(new CafeDto()
            {
                CafeId = c.CafeId,
                CafeName = c.Name,
                CafeAddress = c.Address,
                CafePhone = c.Phone,
                CafeDescription = c.Description,
                CafeWebsite = c.Website,
                CafeImage = c.Image,
                DistrictId = c.DistrictId,
                DistrictName = c.District.DistrictName
            }));

            return Ok(CafeDtos);
        }

        [HttpGet]
        [ResponseType(typeof(CafeDto))]
        public IHttpActionResult ListCafesWithAmenity(int id)
        {

            List<Cafe> Cafes = db.Cafes.Where(
                c => c.Amenities.Any(
                    a=>a.AmenityId==id
                    )).ToList();
            List<CafeDto> CafeDtos = new List<CafeDto>();

            Cafes.ForEach(c => CafeDtos.Add(new CafeDto()
            {
                CafeId = c.CafeId,
                CafeName = c.Name,
                CafeAddress = c.Address,
                CafePhone = c.Phone,
                CafeDescription = c.Description,
                CafeWebsite = c.Website,
                CafeImage = c.Image,
                DistrictId = c.DistrictId,
                DistrictName = c.District.DistrictName
            }));

            return Ok(CafeDtos);
        }

        [HttpPost]
        [Route("api/CafeData/AssociateCafeWithAmenity/{cafeid}/{amenityid}")]
        public IHttpActionResult AssociateCafeWithAmenity(int cafeid, int amenityid)
        {

            Cafe SelectedCafe = db.Cafes.Include(c => c.Amenities).Where(c => c.CafeId == cafeid).FirstOrDefault();
            Amenity SelectedAmenity = db.Amenities.Find(amenityid);

            if (SelectedCafe == null || SelectedAmenity == null)
            {
                return NotFound();
            }

            SelectedCafe.Amenities.Add(SelectedAmenity);
            db.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("api/CafeData/UnAssociateCafeWithAmenity/{cafeid}/{amenityid}")]
        public IHttpActionResult UnAssociateCafeWithAmenity(int cafeid, int amenityid)
        {

            Cafe SelectedCafe = db.Cafes.Include(c => c.Amenities).Where(c => c.CafeId == cafeid).FirstOrDefault();
            Amenity SelectedAmenity = db.Amenities.Find(amenityid);

            if (SelectedCafe == null || SelectedAmenity == null)
            {
                return NotFound();
            }

            SelectedCafe.Amenities.Remove(SelectedAmenity);
            db.SaveChanges();

            return Ok();
        }


        // GET: api/CafeData/FindCafe/5
        [ResponseType(typeof(CafeDto))]
        [HttpGet]
        public IHttpActionResult FindCafe(int id)
        {
            Cafe Cafe = db.Cafes.Find(id);
            CafeDto CafeDto = new CafeDto()
            {
                CafeId = Cafe.CafeId,
                CafeName = Cafe.Name,
                CafeAddress = Cafe.Address,
                CafePhone = Cafe.Phone,
                CafeDescription = Cafe.Description,
                CafeWebsite = Cafe.Website,
                CafeImage = Cafe.Image,
                DistrictName = Cafe.District.DistrictName
            };
            if (Cafe == null)
            {
                return NotFound();
            }

            return Ok(CafeDto);
        }

        // POST: api/CafeData/UpdateCafe/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCafe(int id, Cafe Cafe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Cafe.CafeId)
            {
                return BadRequest();
            }

            db.Entry(Cafe).State = EntityState.Modified;

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
        public IHttpActionResult AddCafe(Cafe Cafe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cafes.Add(Cafe);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Cafe.CafeId }, Cafe);
        }

        // POST: api/CafeData/DeleteCafe/5
        [ResponseType(typeof(Cafe))]
        [HttpPost]
        public IHttpActionResult DeleteCafe(int id)
        {
            Cafe Cafe = db.Cafes.Find(id);
            if (Cafe == null)
            {
                return NotFound();
            }

            db.Cafes.Remove(Cafe);
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

        private bool CafeExists(int id)
        {
            return db.Cafes.Count(c => c.CafeId == id) > 0;
        }
    }
}