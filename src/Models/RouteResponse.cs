using System.Collections.Generic;
using Itinero.Navigation.Instructions;
using Newtonsoft.Json.Linq;
using Itinero;

namespace rideaway_backend.Model {
    public class RouteResponse {
        private Route RouteObj;

        public JObject Route {get;set;}

        public IList<InstructionProperties> Instructions { get; set; }

        public RouteResponse(Route RouteObj){
            this.RouteObj = RouteObj;
            Route = JObject.Parse(RouteObj.ToGeoJson());
        }

        public RouteResponse(Route RouteObj, IList<Instruction> rawInstructions){
            this.RouteObj = RouteObj;
            Route = JObject.Parse(RouteObj.ToGeoJson());
            Instructions = new List<InstructionProperties>();
            foreach(Instruction instruction in rawInstructions){
                Instructions.Add(new InstructionProperties(instruction));
            }
        }
    }
}