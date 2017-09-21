using rideaway_backend.Exceptions;
using Itinero.LocalGeo;

namespace rideaway_backend.Util {
    public class Utility {
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
        public static Coordinate ParseCoordinate (string coord) {
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