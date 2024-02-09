using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyPassionProject.Models
{
    public class Cafe
    {
        //different aspects of a cafe 
        [Key]
        public int CafeId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public byte[] Image { get; set; }


        //a cafe has a district id
        //a district has many cafes
        [ForeignKey("District")]
        public int DistrictId { get; set; }
        public virtual District District { get; set; }


        //many amenities in a cafe
        public ICollection<Amenity> Amenities { get; set; }
    }

    public class CafeDto
    {
        public int CafeId { get; set; }
        public string CafeName { get; set; }
        public string CafeAddress { get; set; }
        public string CafePhone { get; set; }
        public string CafeDescription { get; set; }
        public string CafeWebsite { get; set; }
        public byte[] CafeImage { get; set; }

        public int DistrictId { get; set; }
        public string DistrictName { get; set; }


    }
}