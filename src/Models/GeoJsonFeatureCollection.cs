using System.Collections.Generic;

namespace rideaway_backend.Model {
    public class GeoJsonFeatureCollection {
        public string type {get; set;}
        public IList<InstructionProperties> features {get; set;}

        public GeoJsonFeatureCollection (IList<InstructionProperties> features){
            this.type = "FeatureCollection";
            this.features = features;
        }

    }
}