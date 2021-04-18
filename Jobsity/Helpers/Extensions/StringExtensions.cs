using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jobsity.Helpers.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// removes the first instance of a substring inside a string. If substring is not found then returns the original string
        /// </summary>
        public static string RemoveFirstInstance (this string OgString, string subStr)
        {
            int index = OgString.IndexOf(subStr);
            string cleanStr = (index < 0)
                ? OgString
                : OgString.Remove(index, subStr.Length);

            return cleanStr;
        }
    }
}