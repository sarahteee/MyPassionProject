using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPassionProject.Models
{
    public class Amenity
    {

        [Key]
        public int AmenityId { get; set; }
        public string AmenityName { get; set; }

        //multiple cafes associated with an amenity
        public ICollection<Cafe> Cafes { get; set; }

    }
}