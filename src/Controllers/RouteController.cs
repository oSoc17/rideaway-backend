using System;
using Itinero;
using Itinero.LocalGeo;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using rideaway_backend.Exceptions;
using rideaway_backend.Instance;
using rideaway_backend.Model;
using rideaway_backend.Util;

namespace rideaway_backend.Controllers {
    /// <summary>
    /// Controller for the routing endpoint.
    /// </summary>
    [Route ("[controller]")]
    public class RouteController : Controller {
        /// <summary>
        /// Main endpoint for the application, is invoked by a GET-request to <c>hostname/route</c>.
        /// </summary>
        /// <param name="loc1">The starting point of the route.</param>
        /// <param name="loc2">The ending point of the route.</param>
        /// <param name="profile">The routing profile to use.</param>
        /// <param name="instructions">Return instructions or not.</param>
        /// <param name="lang">Language of the instructions.</param>
        /// <returns>JSON result with geoJSON featurecollection representing the route.</returns>
        [HttpGet]
        [EnableCors ("AllowAnyOrigin")]
        public ActionResult Get (string loc1, string loc2, string profile = "brussels", bool instructions = false, string lang = "en") {
            try {
                Coordinate from = Utility.ParseCoordinate (loc1);
                Coordinate to = Utility.ParseCoordinate (loc2);
                Route route = RouterInstance.Calculate (profile, from, to);

                if (profile == "brussels") {
                    RequestLogger.LogRequest (from, to);
                    return Json (new RouteResponse (route, true, instructions, lang));
                }
                return Json (new RouteResponse (route, false, instructions, lang));

            } catch (ResolveException re) {
                return NotFound (re.Message);
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

    }
}