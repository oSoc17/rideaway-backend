using System.Collections.Generic;
using Itinero.Navigation.Instructions;
using Newtonsoft.Json.Linq;
using Itinero;

namespace rideaway_backend.Model {
    public class RouteResponse {
        private Route RouteObj;

        public JObject Route {get;set;}

        public GeoJsonFeatureCollection Instructions {get; set;}

        public RouteResponse(Route RouteObj){
            this.RouteObj = RouteObj;
            Route = JObject.Parse(RouteObj.ToGeoJson());
        }

        public RouteResponse(Route RouteObj, IList<Instruction> rawInstructions){
            this.RouteObj = RouteObj;
            Route = JObject.Parse(RouteObj.ToGeoJson());
            IList<InstructionProperties> InstructionProps = new List<InstructionProperties>();
            Instruction Previous = null;
            foreach(Instruction instruction in rawInstructions){
                if (Previous == null){
                    Previous = instruction;
                }
                else {
                    InstructionProps.Add(new InstructionProperties(Previous, instruction, RouteObj));
                    Previous = instruction;
                }                
            }
            InstructionProps.Add(new InstructionProperties(Previous, null, RouteObj));
            Instructions = new GeoJsonFeatureCollection(InstructionProps);
        }
    }
}