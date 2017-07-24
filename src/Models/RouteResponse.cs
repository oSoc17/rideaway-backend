using System.Collections.Generic;
using Itinero.Navigation.Instructions;
using Newtonsoft.Json.Linq;
using Itinero;
using rideaway_backend.Extensions;
using System;
using rideaway_backend.Instance;

namespace rideaway_backend.Model {
    public class RouteResponse {
        private Route RouteObj;
        private IList<Instruction> rawInstructions;

        public JObject Route {get;set;}

        public GeoJsonFeatureCollection Instructions {get; set;}

        public RouteResponse(Route RouteObj, bool colorCorrection, bool instructions,  string language="en"){
            this.RouteObj = RouteObj;
            if (colorCorrection){
                rawInstructions = RouteObj.GenerateInstructions(Languages.GetLanguage(language));
                rawInstructions = makeContinuous();
                rawInstructions = SimplifyInstructions();
                correctColours();
                if (instructions) {
                    IList<InstructionProperties> InstructionProps = new List<InstructionProperties>();{
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
            Route = JObject.Parse(RouteObj.ToGeoJson());        
        }      
        
        public IList<Instruction> SimplifyInstructions(){
            IList<Instruction> simplified = new List<Instruction>();
            string currentRef = null;
            string currentColour = null;
            simplified.Add(rawInstructions[0]);
            simplified.Add(rawInstructions[1]);
            Instruction previous = null;
            for (var i = 2; i < rawInstructions.Count - 1; i++){
                Instruction ins = rawInstructions[i];
                if (currentRef == null){
                    string refs = ins.GetAttribute("ref", RouteObj);
                    string colours = ins.GetAttribute("colour", RouteObj);
                    if (refs != null){
                        currentRef = refs.Split(',')[0];
                        ins.SetAttribute("ref", currentRef, RouteObj);
                        if (colours != null){
                            currentColour = colours.Split(',')[0];
                        }
                        ins.SetAttribute("colour", currentColour, RouteObj);     
                        previous = ins;
                    }                        
                }
                else {
                    string refs = ins.GetAttribute("ref", RouteObj);
                    string colours = ins.GetAttribute("colour", RouteObj);
                    if (colours == null){
                        Console.WriteLine("colours null");
                    }
                    if (refs != null && !refs.Contains(currentRef)){
                        previous.SetAttribute("ref", currentRef, RouteObj);
                        previous.SetAttribute("colour", currentColour, RouteObj);
                        currentRef = refs.Split(',')[0];
                        if (colours != null){
                            currentColour = colours.Split(',')[0];
                        }
                        
                        simplified.Add(previous); 
                    }
                }
                previous = ins;
            }

            if (rawInstructions.Count >= 3){
                if (rawInstructions.Count >=4){
                    previous.SetAttribute("ref", currentRef, RouteObj);
                    previous.SetAttribute("colour", currentColour, RouteObj);
                    simplified.Add(previous);
                }
                simplified.Add(rawInstructions[rawInstructions.Count-1]);
            }            

            return simplified;
        }

        public void correctColours(){
            int instructionIndex = 0;
            Instruction currentInstruction = rawInstructions[instructionIndex];
            
            for(var i = 0; i < RouteObj.ShapeMeta.Length; i++){
                int currentShape = RouteObj.ShapeMeta[i].Shape;
                if(currentShape == currentInstruction.Shape){
                   
                    instructionIndex++;
                    if(instructionIndex < rawInstructions.Count){
                        currentInstruction = rawInstructions[instructionIndex];
                    }
                }
                if (i < RouteObj.ShapeMeta.Length - 1){
                    RouteObj.ShapeMeta[i + 1].Attributes.AddOrReplace("colour", currentInstruction.GetAttribute("colour", RouteObj));
                }

            }
        }

        public IList<Instruction> makeContinuous(){
            IList<Instruction> continuous = new List<Instruction>();
            continuous.Add(rawInstructions[0]);
            rawInstructions[1].Type = "enter";
            continuous.Add(rawInstructions[1]);
            for (var i = 2; i < rawInstructions.Count - 2; i++){
                if(rawInstructions[i].GetAttribute("ref", RouteObj) != null){
                    continuous.Add(rawInstructions[i]);
                }
            }
            if (rawInstructions.Count >= 3){
                if (rawInstructions.Count >=4){
                    rawInstructions[rawInstructions.Count-2].Type = "leave";
                    continuous.Add(rawInstructions[rawInstructions.Count-2]);
                }
                continuous.Add(rawInstructions[rawInstructions.Count-1]);
            }
            return continuous;
        } 
    }

      
}