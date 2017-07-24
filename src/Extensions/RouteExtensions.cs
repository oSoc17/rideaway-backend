using System.Collections.Generic;
using Itinero;
using Itinero.Navigation.Instructions;

namespace rideaway_backend.Extensions {
    public static class RouteExtensions {
        public static void correctColours (this Route Route, IList<Instruction> instructions) {
            int instructionIndex = 0;
            Instruction currentInstruction = instructions[instructionIndex];

            for (var i = 0; i < Route.ShapeMeta.Length; i++) {
                int currentShape = Route.ShapeMeta[i].Shape;
                if (currentShape == currentInstruction.Shape) {

                    instructionIndex++;
                    if (instructionIndex < instructions.Count) {
                        currentInstruction = instructions[instructionIndex];
                    }
                }
                if (i < Route.ShapeMeta.Length - 1) {
                    Route.ShapeMeta[i + 1].Attributes.AddOrReplace ("colour", currentInstruction.GetAttribute ("colour", Route));
                }

            }
        }
    }
}