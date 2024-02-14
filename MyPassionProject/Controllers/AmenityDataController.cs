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

        /// <summary>
        /// Returns all Amenities in the system.
        /// </summary>
        /// <returns>
        /// All amenities in the database.
        /// </returns>
        /// <example>
        /// GET: api/AmenityData/ListAmenities
        /// </example>
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

        /// <summary>
        /// Returns all amenities in the system associated with a particular cafe.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all amenities in the database associated with a particular cafe
        /// </returns>
        /// <param name="id">cafe Primary Key</param>
        /// <example>
        /// GET: api/AmenityData/ListAmenitiesForCafe/5
        /// </example>
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

        /// <summary>
        /// Returns amenities in the system not available at a cafe.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all amenities in the database available at a cafe
        /// </returns>
        /// <param name="id">cafe Primary Key</param>
        /// <example>
        /// GET: api/AmenityData/ListAmenitiesNotInCafe/5
        /// </example>
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

        /// <summary>
        /// Returns all amenities in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An amenity in the system matching up to the amenity ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the amenity</param>
        /// <example>
        /// GET: api/AmenityData/FindAmenity/5
        /// </example>
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

        /// <summary>
        /// Updates a particular amenity in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the amenity ID primary key</param>
        /// <param name="Amenity">JSON FORM DATA of an Amenity</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/AmenityData/UpdateAmenity/5
        /// FORM DATA: Amenity JSON Object
        /// </example>
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

        /// <summary>
        /// Adds an amenity to the system
        /// </summary>
        /// <param name="Amenity">JSON FORM DATA of an Amenity</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Amenity ID, Amenity Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/AmenityData/AddAmenity
        /// FORM DATA: Amenity JSON Object
        /// </example>
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

        /// <summary>
        /// Deletes an amenity from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the amenity</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/AmenityData/DeleteAmenity/5
        /// FORM DATA: (empty)
        /// </example>
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