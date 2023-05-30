using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace task5withGridviewfixed
{
    [Serializable]
    public class City
    {
        public string CityCode    { get; set; }
        public string CityName    { get; set; }
        public string CountryCode { get; set; }
        public bool ExistsOrNotinDB { get; set; }
    } 


}