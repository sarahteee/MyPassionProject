using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPassionProject.Models.ViewModels
{
    public class UpdateCafe
    {
        //This viewmodel is a class which stores information that we need to present to /Cafe/Update/{}

        //the existing cafe information

        public CafeDto selectedcafe { get; set; }

        // all districts to choose from when updating this cafe

        public IEnumerable<DistrictDto> DistrictOptions { get; set; }
    }
}