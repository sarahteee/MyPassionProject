using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPassionProject.Models.ViewModels
{
    public class DetailsCafe
    {
        public CafeDto SelectedCafe { get; set; }
        public IEnumerable<AmenityDto> AvailableAmenities { get; set; }

        public IEnumerable<AmenityDto> CurrentAmenities { get; set; }
    }
}