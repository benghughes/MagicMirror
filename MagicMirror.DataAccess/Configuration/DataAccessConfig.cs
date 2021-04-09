using System;
using System.Collections.Generic;
using System.Text;

namespace MagicMirror.DataAccess.Configuration
{
    public static class DataAccessConfig
    {
        public static string OpenWeatherMapId
        {
            get
            {
                return "5f635dc16ce86b4f3b7adc938977feab";
            }
        }
        public static string OpenWeatherMapUrl
        {
            get
            {
                return "http://api.openweathermap.org/data/2.5";
            }
        }

        public static string TrafficApiUrl
        {
            get
            {
                return "https://maps.googleapis.com/maps/api/distancematrix/json";
            }
        }

        public static string TrafficApiId
        {
            get
            {
                return "AIzaSyCCgjUuaVPwaC92yAkk17FUi0kjcbv8Swk";
            }
        }
    }
}
