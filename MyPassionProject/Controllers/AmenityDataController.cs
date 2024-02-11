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
    public class AmenityDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AmenityData/ListAmenities
        [HttpGet]
        [ResponseType(typeof(AmenityDto))]
        public IHttpActionResult ListAmenities()
        {
            List<Amenity> Amenities = db.Amenities.ToList();
            List<AmenityDto> AmenityDtos = new List<AmenityDto>();

            Amenities.ForEach(a => AmenityDtos.Add(new AmenityDto()
            {
                AmenityId = a.AmenityId,
                AmenityName = a.AmenityName
            }));

            return Ok(AmenityDtos);
        }

        // GET: api/AmenityData/ListAmenitiesForCafe/5
        [HttpGet]
        [ResponseType(typeof(AmenityDto))]
        public IHttpActionResult ListAmenitiesForCafe(int id)
        {
            List<Amenity> Amenities = db.Amenities.Where(
                a=>a.Cafes.Any(
                    c=>c.CafeId==id)
                ).ToList();
            List<AmenityDto> AmenityDtos = new List<AmenityDto>();

            Amenities.ForEach(a => AmenityDtos.Add(new AmenityDto()
            {
                AmenityId = a.AmenityId,
                AmenityName = a.AmenityName
            }));

            return Ok(AmenityDtos);
        }

        // GET: api/AmenityData/ListAmenitiesNotInCafe/5
        [HttpGet]
        [ResponseType(typeof(AmenityDto))]
        public IHttpActionResult ListAmenitiesNotInCafe(int id)
        {
            List<Amenity> Amenities = db.Amenities.Where(
                a => !a.Cafes.Any(
                    c => c.CafeId == id)
                ).ToList();
            List<AmenityDto> AmenityDtos = new List<AmenityDto>();

            Amenities.ForEach(a => AmenityDtos.Add(new AmenityDto()
            {
                AmenityId = a.AmenityId,
                AmenityName = a.AmenityName
            }));

            return Ok(AmenityDtos);
        }

        // GET: api/AmenityData/FindAmenity/5
        [HttpGet]
        [ResponseType(typeof(AmenityDto))]
        public IHttpActionResult FindAmenity(int id)
        {
            Amenity Amenity = db.Amenities.Find(id);
            AmenityDto AmenityDto = new AmenityDto()
            {
                AmenityId = Amenity.AmenityId,
                AmenityName = Amenity.AmenityName
            };

            if (Amenity == null)
            {
                return NotFound();
            }

            return Ok(AmenityDto);
        }

        // PUT: api/AmenityData/UpdateAmenity/5
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateAmenity(int id, Amenity Amenity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Amenity.AmenityId)
            {
                return BadRequest();
            }

            db.Entry(Amenity).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AmenityExists(id))
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

        // POST: api/AmenityData/AddAmenity
        [ResponseType(typeof(Amenity))]
        [HttpPost]
        public IHttpActionResult AddAmenity(Amenity Amenity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Amenities.Add(Amenity);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Amenity.AmenityId }, Amenity);
        }

        // DELETE: api/AmenityData/DeleteAmenity/5
        [ResponseType(typeof(Amenity))]
        [HttpPost]
        public IHttpActionResult DeleteAmenity(int id)
        {
            Amenity Amenity = db.Amenities.Find(id);
            if (Amenity == null)
            {
                return NotFound();
            }

            db.Amenities.Remove(Amenity);
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

        private bool AmenityExists(int id)
        {
            return db.Amenities.Count(e => e.AmenityId == id) > 0;
        }
    }
}