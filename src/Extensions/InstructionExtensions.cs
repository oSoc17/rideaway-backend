using Itinero.Navigation.Instructions;
using Itinero;

namespace rideaway_backend.Extensions {
    public static class InstructionExtensions {
        public static string GetAttribute(this Instruction instruction, string key, Route route){
            string value;
            route.ShapeMetaFor(instruction.Shape).Attributes.TryGetValue(key, out value);
            return value;
        }

        public static void SetAttribute(this Instruction instruction, string key, string value, Route route){
            route.ShapeMetaFor(instruction.Shape).Attributes.AddOrReplace(key, value);
        }
    }
}