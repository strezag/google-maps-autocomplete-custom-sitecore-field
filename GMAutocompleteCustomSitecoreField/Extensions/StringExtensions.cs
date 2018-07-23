using System;
using System.Collections.Generic;
using System.Linq;using System.Web;

namespace GMAutocompleteCustomSitecoreField.Extensions
{
    // ReSharper disable once UnusedMember.Global
    public static class StringExtensions
    {
        /// <summary>
        /// This method is used to Get Address, Latitude or Longitude from the source which is a NameValueCollection.
        /// A valid Coordinate Name is: Address, Latitude and Longitude
        /// </summary>
        /// <param name="source">It is NameValueCollection string</param>
        /// <param name="coordinateName">It is a string type of parameter that holds:  Address, Latitude or Longitude</param>
        /// <returns>It returns the data of required Coordinate</returns>
        public static string GetMapCoordinates(this string source, string coordinateName)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(coordinateName))
            {
                return coordinateName;
            }

            source = source.Replace("&amp;", "&");
            var data = HttpUtility.ParseQueryString(source);
            if (data != null)
            {
                return Convert.ToString(data[coordinateName]);
            }

            return coordinateName;
        }
    }
}