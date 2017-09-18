using System.IO;
using System;
using Itinero.LocalGeo;
using System.Globalization;

namespace rideaway_backend.Instance {
    public static class RequestLogger {
        public static void LogRequest(Coordinate from, Coordinate to){
            string row = DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:sszzz") + "," 
            + from.Latitude.ToString(new CultureInfo ("en-US")) + "," 
            + from.Longitude.ToString(new CultureInfo ("en-US")) + "," 
            + to.Latitude.ToString(new CultureInfo ("en-US")) + "," 
            + to.Longitude.ToString(new CultureInfo ("en-US")) + "\n";

            File.AppendAllText(@"wwwroot/requests/data" + DateTime.Now.ToString("dd-MM-yyyy") + ".csv", row);
        }
    }
}