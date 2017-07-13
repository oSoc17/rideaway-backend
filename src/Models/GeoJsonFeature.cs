using System.Collections.Generic;

namespace rideaway_backend.Model {
    public class GeoJsonFeature{
        public string type {get; set;}

        public Dictionary<string, string> properties {get; set;}
        public GeoJsonPoint geometry {get; set;}

        public GeoJsonFeature (Dictionary<string, string> properties, GeoJsonPoint geometry){
            this.type = "Feature";
            this.properties = properties;
            this.geometry = geometry;
        }
    }
    
}