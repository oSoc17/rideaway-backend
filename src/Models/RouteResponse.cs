using System.Collections.Generic;
using Itinero.Navigation.Instructions;

namespace rideaway_backend.Model {
    public class RouteResponse {
        private string Route;
        private IList<InstructionProperties> Instructions;

        public RouteResponse(string Route){
            this.Route = Route;
        }

        public RouteResponse(string Route, IList<Instruction> rawInstructions){
            this.Route = Route;
            Instructions = new List<InstructionProperties>();
            foreach(Instruction instruction in rawInstructions){
                Instructions.Add(new InstructionProperties(instruction));
            }
        }
    }
}