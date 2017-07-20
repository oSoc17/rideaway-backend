using Itinero.Navigation.Instructions;
using Itinero;

namespace rideaway_backend.Extensions {
    public static class InstructionExtensions {
        public static string GetAttribute(this Instruction instruction, string key, Route route){
            string value;
            route.ShapeMetaFor(instruction.Shape).Attributes.TryGetValue("key", out value);
            return value;
        }
    }
}