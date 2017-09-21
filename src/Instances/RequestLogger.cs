using System.IO;
using System;
using Itinero.LocalGeo;
using System.Globalization;

namespace rideaway_backend.Instance {
    /// <summary>
    /// Logger for requests of routes over the cyclenetwork.
    /// </summary>
    public static class RequestLogger {
        /// <summary>
        /// Log request in file of current day with current timestamp.
        /// </summary>
        /// <param name="from">starting coordinate</param>
        /// <param name="to">ending coordinate</param>
        public static void LogRequest(Coordinate from, Coordinate to){
            string row = DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:sszzz") + "," 
            + from.Latitude.ToString(new CultureInfo ("en-US")) + "," 
            + from.Longitude.ToString(new CultureInfo ("en-US")) + "," 
            + to.Latitude.ToString(new CultureInfo ("en-US")) + "," 
            + to.Longitude.ToString(new CultureInfo ("en-US")) + "\n";

            File.AppendAllText(@"wwwroot/requests/data/" + DateTime.Now.ToString("dd-MM-yyyy") + ".csv", row);
        }
    }
}