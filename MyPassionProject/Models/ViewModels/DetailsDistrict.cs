using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPassionProject.Models.ViewModels
{
    public class DetailsDistrict
    {
        //the district itself that we want to display
        public DistrictDto SelectedDistrict { get; set; }

        //all of the related cafes in that particular district
        public IEnumerable<CafeDto> RelatedCafes { get; set; }
    }
}