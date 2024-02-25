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
using Microsoft.Ajax.Utilities;
using MyPassionProject.Models;

namespace MyPassionProject.Controllers
{
    public class CafeDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all cafes in the system.
        /// </summary>
        /// <returns>
        /// All cafes in the database with names containing the seach key, including their associated districts and addresses.
        /// </returns>
        /// <example>
        /// GET: api/CafeData/ListCafes/creeds
        /// </example>
        [HttpGet]
        [Route("api/CafeData/ListCafes/{SearchKey?}")]
        public IEnumerable<CafeDto> ListCafes(string SearchKey = null)
        {
            List<Cafe> Cafes = new List<Cafe>();

            if(SearchKey == null)
            {
                Cafes = db.Cafes.ToList();
            } else
            {
                Cafes = db.Cafes.Where(c => c.Name.Contains(SearchKey)).ToList();
            }

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

            return CafeDtos;
        }

        /// <summary>
        /// Gathers information about all cafes related to a particular district ID
        /// </summary>
        /// <returns>
        ///  All cafes in the database, including their associated district matched with a particular district ID
        /// </returns>
        /// <param name="id">District Id.</param>
        /// <example>
        /// GET: api/CafeData/ListCafesForDistrict/3
        /// </example>
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

        /// <summary>
        /// Gathers information about cafes related to a particular amenity
        /// </summary>
        /// <returns>
        /// All cafes in the database, including their associated district that match to a particular amenity id
        /// </returns>
        /// <param name="id">Amenity Id.</param>
        /// <example>
        /// GET: api/CafeData/ListCafesWithAmenity/1
        /// </example>
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

        /// <summary>
        /// Associates a particular amenity with a particular cafe
        /// </summary>
        /// <param name="cafeid">The cafe ID primary key</param>
        /// <param name="amenityid">The amenity ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/CafeData/AssociateCafeWithAmenity/9/1
        /// </example>
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

            Debug.WriteLine("input cafe id is:" + cafeid);
            Debug.WriteLine("selected cafe name is:" + SelectedCafe.Name);


            SelectedCafe.Amenities.Add(SelectedAmenity);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular amenity and a particular cafe
        /// </summary>
        /// <param name="cafeid">The cafe ID primary key</param>
        /// <param name="amenityid">The amenity ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/CafeData/UnAssociateCafeWithAmenity/9/1
        /// </example>
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

            Debug.WriteLine("input cafe id is: " + cafeid);

            SelectedCafe.Amenities.Remove(SelectedAmenity);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Returns all cafes in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A cafe in the system matching up to the cafe ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the cafe</param>
        /// <example>
        /// GET: api/CafeData/FindCafe/5
        /// </example>
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

        /// <summary>
        /// Updates a particular cafe in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the cafe ID primary key</param>
        /// <param name="cafe">JSON FORM DATA of a cafe</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/CafeData/UpdateCafe/5
        /// FORM DATA: cafe JSON Object
        /// </example>
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

        /// <summary>
        /// Adds a cafe to the system
        /// </summary>
        /// <param name="cafe">JSON FORM DATA of a cafe</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Cafe ID, Cafe Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/CafeData/AddCafe
        /// FORM DATA: Cafe JSON Object
        /// </example>
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

        /// <summary>
        /// Deletes an cafe from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the cafe</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/CafeData/DeleteCafe/5
        /// FORM DATA: (empty)
        /// </example>
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