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
using MyPassionProject.Migrations;
using MyPassionProject.Models;

namespace MyPassionProject.Controllers
{
    public class DistrictDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DistrictDatal/ListDistricts
        [HttpGet]
        [ResponseType(typeof(DistrictDto))]
        public IHttpActionResult ListDistricts()
        {
            List<District> Districts = db.Districts.ToList();
            List<DistrictDto> DistrictDtos = new List<DistrictDto>();

            Districts.ForEach(d => DistrictDtos.Add(new DistrictDto()
            {
                DistrictId = d.DistrictId,
                DistrictName = d.DistrictName
            }));

            return Ok(DistrictDtos);
        }

        // GET: api/DistrictData/FindDistrict/5
        [ResponseType(typeof(DistrictDto))]
        [HttpGet]
        public IHttpActionResult FindDistrict(int id)
        {
            District District = db.Districts.Find(id);
            DistrictDto DistrictDto = new DistrictDto();
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

        // PUT: api/DistrictData/UpdateDistrict/5
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

        // POST: api/DistrictData/Add
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

        // DELETE: api/DistrictData/DeleteDistrict/5
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