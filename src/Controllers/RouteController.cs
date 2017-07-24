using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itinero;
using Itinero.LocalGeo;
using Itinero.Navigation.Instructions;
using Itinero.Profiles;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using rideaway_backend.Exceptions;
using rideaway_backend.Instance;
using rideaway_backend.Model;

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
                Coordinate from = ParseCoordinate (loc1);
                Coordinate to = ParseCoordinate (loc2);
                Route route = RouterInstance.Calculate (profile, from, to);

                if (profile == "brussels") {
                    return Json (new RouteResponse (route, true, instructions, lang));
                }
                return Json (new RouteResponse (route, false, instructions, lang));

            } catch (ResolveException re) {
                return NotFound (re.Message);
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        /// <summary>
        /// Turns a string consisting of a latitude and longitude value seperated 
        /// by a comma into a Coordinate.
        /// </summary>
        /// <param name="coord">String to convert.</param>
        /// <returns>A coordinate object.</returns>
        /// <exception name="CoordinateLengthException">
        /// Thrown when the string is null or doesn't consist of two parts.
        /// </exception>
        /// <exception name="CoordinateParseException">
        /// Thrown when one part of the string could not be parsed to a float.
        /// </exception>
        public Coordinate ParseCoordinate (string coord) {
            float lat;
            float lon;
            if (coord == null) {
                throw new CoordinateLengthException ("Location is not specified");
            }
            string[] parts = coord.Split (',');
            if (parts.Length != 2) {
                throw new CoordinateLengthException ("Location <" + coord + "> does not consist of two parts");
            }
            if (float.TryParse (parts[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out lat) &&
                float.TryParse (parts[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out lon)) {
                return new Coordinate (lat, lon);
            } else {
                throw new CoordinateParseException ("Location <" + coord + "> could not be parsed");
            }
        }
    }
}