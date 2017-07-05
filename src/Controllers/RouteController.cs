using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Itinero.LocalGeo;
using rideaway_backend.Exceptions;

namespace rideaway_backend.Controllers
{
    [Route("[controller]")]
    public class RouteController : Controller
    {
        // GET api/values
        [HttpGet]
        public Coordinate Get(string loc1, string loc2)
        {
            Coordinate from = ParseCoordinate(loc1);
            Coordinate to = ParseCoordinate(loc2);
            return from;
        }

        public Coordinate ParseCoordinate(string coord){
            float lat;
            float lon;
            if (coord == null){
                throw new CoordinateLengthException("Location is not specified");
            }
            string[] parts = coord.Split(',');
            if(parts.Length != 2){
                throw new CoordinateLengthException("Location <" + coord + "> does not consist of two parts");
            }
            if(float.TryParse(parts[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out lat) &&
            float.TryParse(parts[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out lon)){
                return new Coordinate(lat, lon);
            }
            else {
                throw new CoordinateParseException("Location <" +coord + "> could not be parsed");
            }
        }
    }
}
