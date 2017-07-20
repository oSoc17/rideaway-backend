using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Itinero.LocalGeo;
using Itinero.Profiles;
using Itinero;
using Itinero.Navigation.Instructions;
using rideaway_backend.Exceptions;
using rideaway_backend.Instance;
using rideaway_backend.Model;
using Microsoft.AspNetCore.Cors;


namespace rideaway_backend.Controllers
{
    [Route("[controller]")]
    public class RouteController : Controller
    {
        // GET api/values
        [HttpGet]
        [EnableCors("AllowAnyOrigin")]
        public ActionResult Get(string loc1, string loc2, string profile = "brussels", bool instructions = false, string lang = "en")
        {
            try{
                Coordinate from = ParseCoordinate(loc1);
                Coordinate to = ParseCoordinate(loc2);
                Route route = RouterInstance.Calculate(profile, from, to);
  
                if (profile == "brussels"){
                    return Json(new RouteResponse(route, true, instructions, lang));
                }
                return Json(new RouteResponse(route, false, instructions, lang));
                
            }
            catch(ResolveException re){
                return NotFound(re.Message);
            }
            catch(Exception e){
                return BadRequest(e.Message);
            }
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
