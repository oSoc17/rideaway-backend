using Itinero.LocalGeo;

namespace rideaway_backend.Model {
    public class GeoJsonPoint {
        public string type {get; set;}
        public float[] Coordinates {get; set;}
       

        public GeoJsonPoint (Coordinate Coordinate){
            this.type = "Point";
            this.Coordinates = new float[]{Coordinate.Longitude,Coordinate.Latitude};
        } 
    }
}