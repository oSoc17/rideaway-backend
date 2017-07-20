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
/*
        public RouteResponse(Route RouteObj){
            this.RouteObj = RouteObj;
            Route = JObject.Parse(RouteObj.ToGeoJson());
        }*/

        public RouteResponse(Route RouteObj, bool colorCorrection, bool instructions,  string language="en"){
            this.RouteObj = RouteObj;
            if (colorCorrection){
                IList<Instruction> simplified = SimplifyInstructions(language);
                correctColours(simplified);
                if (instructions) {
                    IList<InstructionProperties> InstructionProps = new List<InstructionProperties>();{
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
                }
            }
            Route = JObject.Parse(RouteObj.ToGeoJson());        
        }

/*
        public RouteResponse(Route RouteObj, IList<Instruction> rawInstructions){
            this.RouteObj = RouteObj;
            
            IList<InstructionProperties> InstructionProps = new List<InstructionProperties>();
            IList<Instruction> simplified = SimplifyInstructions(rawInstructions, RouteObj);
            correctColours(RouteObj, simplified);
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
            Route = JObject.Parse(RouteObj.ToGeoJson());
        }*/
        
        public IList<Instruction> SimplifyInstructions(string language){
            IList<Instruction> instructions = RouteObj.GenerateInstructions();
            IList<Instruction> simplified = new List<Instruction>();
            string currentRef = null;
            simplified.Add(instructions[0]);
            instructions[1].Type = "enter";
            simplified.Add(instructions[1]);
            for (var i = 2; i < instructions.Count; i++){
                Instruction ins = instructions[i];
                if (currentRef == null){
                    string refs = ins.GetAttribute("ref", RouteObj);
                    if (refs != null){
                        currentRef = refs.Split(',')[0];
                        ins.SetAttribute("ref", currentRef, RouteObj);
                        string colours = ins.GetAttribute("colour", RouteObj);
                        string currentColour = colours.Split(',')[0];
                        ins.SetAttribute("colour", currentColour, RouteObj);
                        simplified.Add(ins);
                    }                        
                }
                else {
                    string refs = ins.GetAttribute("ref", RouteObj);
                    if (refs != null && !refs.Contains(currentRef)){
                        currentRef = refs.Split(',')[0];
                        ins.SetAttribute("ref", currentRef, RouteObj);
                        string colours = ins.GetAttribute("colour", RouteObj);
                        string currentColour = colours.Split(',')[0];
                        ins.SetAttribute("colour", currentColour, RouteObj);
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

        public void correctColours(IList<Instruction> instructions){
            int instructionIndex = 0;
            Instruction currentInstruction = instructions[instructionIndex + 1];
            
            for(var i = 0; i < RouteObj.ShapeMeta.Length; i++){
                int currentShape = RouteObj.ShapeMeta[i].Shape;
                if(currentShape == currentInstruction.Shape){
                    instructionIndex++;
                    if(instructionIndex < instructions.Count - 1){
                        currentInstruction = instructions[instructionIndex + 1];
                    }
                }
                RouteObj.ShapeMeta[i].Attributes.AddOrReplace("colour", currentInstruction.GetAttribute("colour", RouteObj));

            }
        } 
    }

      
}