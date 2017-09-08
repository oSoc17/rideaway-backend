using System.IO;
using System;
using Itinero.LocalGeo;

namespace rideaway_backend.Instance {
    public static class RequestLogger {
        public static void LogRequest(Coordinate from, Coordinate to){

            File.AppendAllText(@"wwwroot/requests/" + DateTime.Now.ToString("dd-MM-yyyy") + ".csv", "test");
            
        }
    }
}