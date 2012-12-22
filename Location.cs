using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace arunes
{
    public class Location
    {
        public string Country;
        public string RegionName;
        public string City;
        public double Latitude;
        public double Longitude;

        public Location(string ipInfoDbApiKey)
        {
            SetLocationData(ipInfoDbApiKey, Functions.GetIPAddress());
        }

        public Location(string ipInfoDbApiKey, string ipAddress)
        {
            SetLocationData(ipInfoDbApiKey, ipAddress);
        }

        private void SetLocationData(string ipInfoDbApiKey, string ipAddress)
        {
            try
            {
                var url = "http://api.ipinfodb.com/v2/ip_query.php?key={0}&ip={1}&timezone=false";
                XDocument locationXml = XDocument.Load(string.Format(url, ipInfoDbApiKey, ipAddress));
                Country = locationXml.Root.Element("CountryName").Value;
                RegionName = locationXml.Root.Element("RegionName").Value;
                City = locationXml.Root.Element("City").Value;
                Latitude = Convert.ToDouble(locationXml.Root.Element("Latitude").Value);
                Longitude = Convert.ToDouble(locationXml.Root.Element("Longitude").Value);
            }
            catch
            { }
        }
    }
}
