using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using rideaway_backend.Util;
using Itinero.LocalGeo;
using rideaway_backend.Exceptions;
using System;

namespace rideaway_backend.Controllers {
    [Route ("[controller]")]
    public class ParkingController : Controller {
        [HttpGet]
        [EnableCors ("AllowAnyOrigin")]
        public ActionResult Get(string loc, int radius = 500){
            try {
                Coordinate location = Utility.ParseCoordinate (loc);
                
                return null;

            } catch (ResolveException re) {
                return NotFound (re.Message);
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }
    }
}