using System.Collections.Generic;
using Itinero.Navigation.Instructions;
using Newtonsoft.Json.Linq;
using Itinero;
using rideaway_backend.Extensions;
using System;

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
            IList<Instruction> simplified = SimplifyInstructions(rawInstructions, RouteObj);
            Instruction Previous = null;
            foreach(Instruction instruction in simplified){
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
        
        public IList<Instruction> SimplifyInstructions(IList<Instruction> instructions, Route Route){
            IList<Instruction> simplified = new List<Instruction>();
            string currentRef = null;
            simplified.Add(instructions[0]);
            instructions[1].Type = "enter";
            simplified.Add(instructions[1]);
            for (var i = 2; i < instructions.Count; i++){
                Instruction ins = instructions[i];
                if (currentRef == null){
                    string refs = ins.GetAttribute("ref", Route);
                    if (refs != null){
                        currentRef = refs.Split(',')[0];
                        ins.SetAttribute("ref", currentRef, Route);
                        string colours = ins.GetAttribute("colour", Route);
                        string currentColour = colours.Split(',')[0];
                        ins.SetAttribute("colour", currentColour, Route);
                        simplified.Add(ins);
                    }                        
                }
                else {
                    string refs = ins.GetAttribute("ref", Route);
                    if (refs != null && !refs.Contains(currentRef)){
                        currentRef = refs.Split(',')[0];
                        ins.SetAttribute("ref", currentRef, Route);
                        string colours = ins.GetAttribute("colour", Route);
                        string currentColour = colours.Split(',')[0];
                        ins.SetAttribute("colour", currentColour, Route);
                        simplified.Add(ins);
                    }
                }
            }

            if (instructions.Count >= 3){
                if (instructions.Count >=4){
                    instructions[instructions.Count-2].Type = "leave";
                    simplified.Add(instructions[instructions.Count-2]);
                }
                simplified.Add(instructions[instructions.Count-1]);
            }            

            return simplified;
        } 
    }

      
}