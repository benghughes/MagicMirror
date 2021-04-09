using Acme.Generic.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.Generic.Extensions
{
    public static class StringExtensions
    {
        public static string ConvertUnixTime(this string valueToConvert)
        {
            string result = "";

            if (int.TryParse(valueToConvert, out int parsed))
            {
                result = DateTimeHelper.ConvertUnixTimeToGMTDateTime(parsed)
                    .ToLocalTime()
                    .ToShortTimeString();
            }
            return result;
        }
    }
}
