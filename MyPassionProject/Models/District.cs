using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyPassionProject.Models
{
    public class District
    {
        [Key]
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
    }

    public class DistrictDto
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
    }
}