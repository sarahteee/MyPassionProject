using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPassionProject.Models.ViewModels
{
    public class DetailsAmenity
    {
        public AmenityDto SelectedAmenity { get; set; }
        public IEnumerable<CafeDto> RelevantCafes { get; set; }
    }
}