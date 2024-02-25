using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using System.Web.Http.Description;
using MyPassionProject.Migrations;
using MyPassionProject.Models;

namespace MyPassionProject.Controllers
{
    public class DistrictDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all districts in the system.
        /// </summary>
        /// <returns>
        /// All districts in the database, including their associated districts.
        /// </returns>
        /// <example>
        /// GET: api/DistrictData/ListDistricts
        /// </example>
        [HttpGet]
        [Route("api/DistrictData/ListDistricts/{SearchKey?}")]
        public IEnumerable<DistrictDto> ListDistricts(string SearchKey = null)
        {
            //List<District> Districts = db.Districts.ToList();
            List<District> Districts = new List<District>();

            if (SearchKey == null)
            {
                Districts = db.Districts.ToList();
            }
            else
            {
                Districts = db.Districts.Where(d => d.DistrictName.Contains(SearchKey)).ToList();
            }
            List<DistrictDto> DistrictDtos = new List<DistrictDto>();
            Districts.ForEach(d => DistrictDtos.Add(new DistrictDto()
            {
                DistrictId = d.DistrictId,
                DistrictName = d.DistrictName
            }));

            return DistrictDtos;
        }

        /// <summary>
        /// Returns all districts in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A district in the system matching up to the district ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the district</param>
        /// <example>
        /// GET: api/DistrictData/FindDistrict/5
        /// </example>
        [ResponseType(typeof(DistrictDto))]
        [HttpGet]
        public IHttpActionResult FindDistrict(int id)
        {
            District District = db.Districts.Find(id);
            DistrictDto DistrictDto = new DistrictDto()
            {
                DistrictId = District.DistrictId,
                DistrictName = District.DistrictName
            };
            if (District == null)
            {
                return NotFound();
            }

            return Ok(DistrictDto);
        }

        /// <summary>
        /// Updates a particular district in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the district ID primary key</param>
        /// <param name="District">JSON FORM DATA of a district</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/DistrictData/UpdateDistrict/5
        /// FORM DATA: District JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDistrict(int id, District District)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != District.DistrictId)
            {
                return BadRequest();
            }

            db.Entry(District).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistrictExists(id))
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
        /// Adds a district to the system
        /// </summary>
        /// <param name="District">JSON FORM DATA of an district</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: District ID, District Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/DistrictData/AddDistrict
        /// FORM DATA: District JSON Object
        /// </example>
        [ResponseType(typeof(District))]
        [HttpPost]
        public IHttpActionResult AddDistrict(District District)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Districts.Add(District);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = District.DistrictId }, District);
        }

        /// <summary>
        /// Deletes a district from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the district</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/DistrctData/DeleteDistrict/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(District))]
        [HttpPost]
        public IHttpActionResult DeleteDistrict(int id)
        {
            District District = db.Districts.Find(id);
            if (District == null)
            {
                return NotFound();
            }

            db.Districts.Remove(District);
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

        private bool DistrictExists(int id)
        {
            return db.Districts.Count(e => e.DistrictId == id) > 0;
        }
    }
}