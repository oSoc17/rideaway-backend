using System.Collections.Generic;
using Itinero;
using Itinero.Navigation.Instructions;
using rideaway_backend.Model;

namespace rideaway_backend.Extensions {
    /// <summary>
    /// Extensions for instructions to set and get attributes of the corresponding metadata.
    /// </summary>
    public static class InstructionExtensions {
        /// <summary>
        /// Get an attribute value from the metadata corresponding to the instruction.
        /// </summary>
        /// <param name="instruction">Instruction to get the attribute from.</param>
        /// <param name="key">The key for the attribute value.</param>
        /// <param name="route">The route object that has the metadata for all the instructions.</param>
        /// <returns></returns>
        public static string GetAttribute (this Instruction instruction, string key, Route route) {
            string value;
            route.ShapeMetaFor (instruction.Shape).Attributes.TryGetValue (key, out value);
            return value;
        }

        /// <summary>
        /// Set an attribute in the metadata corresponding to the instruction.
        /// </summary>
        /// <param name="instruction">Instruction to set the attribute of.</param>
        /// <param name="key">The key for the attribute.</param>
        /// <param name="value">The value to set for the attribute.</param>
        /// <param name="route">The route object that has the metadata for all the instructions</param>
        public static void SetAttribute (this Instruction instruction, string key, string value, Route route) {
            route.ShapeMetaFor (instruction.Shape).Attributes.AddOrReplace (key, value);
        }

        public static GeoJsonFeatureCollection ToGeoJsonCollection (this IList<Instruction> instructions, Route Route) {
            IList<InstructionProperties> InstructionProps = new List<InstructionProperties> ();
            Instruction Previous = null;
            foreach (Instruction instruction in instructions) {
                if (Previous == null) {
                    Previous = instruction;
                } else {
                    InstructionProps.Add (new InstructionProperties (Previous, instruction, Route));
                    Previous = instruction;
                }
            }
            InstructionProps.Add (new InstructionProperties (Previous, null, Route));

            return new GeoJsonFeatureCollection (InstructionProps);
        }

        public static IList<Instruction> makeContinuous (this IList<Instruction> instructions, Route Route) {
            IList<Instruction> continuous = new List<Instruction> ();
            continuous.Add (instructions[0]);
            instructions[1].Type = "enter";
            continuous.Add (instructions[1]);
            for (var i = 2; i < instructions.Count - 2; i++) {
                if (instructions[i].GetAttribute ("ref", Route) != null) {
                    continuous.Add (instructions[i]);
                }
            }
            if (instructions.Count >= 3) {
                if (instructions.Count >= 4) {
                    instructions[instructions.Count - 2].Type = "leave";
                    continuous.Add (instructions[instructions.Count - 2]);
                }
                continuous.Add (instructions[instructions.Count - 1]);
            }
            return continuous;
        }

        public static IList<Instruction> simplify (this IList<Instruction> instructions, Route Route) {
            IList<Instruction> simplified = new List<Instruction> ();
            string currentRef = null;
            string currentColour = null;
            simplified.Add (instructions[0]);
            simplified.Add (instructions[1]);
            Instruction previous = null;
            for (var i = 2; i < instructions.Count - 1; i++) {
                Instruction ins = instructions[i];
                if (currentRef == null) {
                    string refs = ins.GetAttribute ("ref", Route);
                    string colours = ins.GetAttribute ("colour", Route);
                    if (refs != null) {
                        currentRef = refs.Split (',')[0];
                        ins.SetAttribute ("ref", currentRef, Route);
                        if (colours != null) {
                            currentColour = colours.Split (',')[0];
                        }
                        ins.SetAttribute ("colour", currentColour, Route);
                        previous = ins;
                    }
                } else {
                    string refs = ins.GetAttribute ("ref", Route);
                    string colours = ins.GetAttribute ("colour", Route);
                    if (refs != null && !refs.Contains (currentRef)) {
                        previous.SetAttribute ("ref", currentRef, Route);
                        previous.SetAttribute ("colour", currentColour, Route);
                        currentRef = refs.Split (',')[0];
                        if (colours != null) {
                            currentColour = colours.Split (',')[0];
                        }

                        simplified.Add (previous);
                    }
                }
                previous = ins;
            }

            if (instructions.Count >= 3) {
                if (instructions.Count >= 4) {
                    previous.SetAttribute ("ref", currentRef, Route);
                    previous.SetAttribute ("colour", currentColour, Route);
                    simplified.Add (previous);
                }
                simplified.Add (instructions[instructions.Count - 1]);
            }

            return simplified;
        }
    }
}