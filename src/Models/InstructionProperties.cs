using Itinero.Navigation.Instructions;
using Itinero.Navigation.Directions;
using System.Collections.Generic;
using Itinero;
using System.Globalization;
using Itinero.LocalGeo;

namespace rideaway_backend.Model{
    public class InstructionProperties {

        public string type {get; set;}

        public Dictionary<string, string> properties {get; set;}
        public GeoJsonPoint geometry {get; set;}

        private Instruction Instruction {get; set;}

        private DirectionEnum Direction {get; set;}

        private RelativeDirection Angle {get; set;}

        public InstructionProperties(Instruction Instruction, Instruction Next, Route route){
            this.type = "Feature";
            this.properties = new Dictionary<string, string>();
            this.geometry =  new GeoJsonPoint(route.Shape[Instruction.Shape]);
            //this.geometry =  new GeoJsonPoint((Coordinate)route.Shape.GetValue(Instruction.Shape));
            //this.geometry = new GeoJsonPoint(route.Branches[Instruction.Shape].Coordinate);
            this.Instruction = Instruction;
            properties.Add("instruction", Instruction.Text);
            Route.Meta meta =  route.ShapeMetaFor(Instruction.Shape);
            string ColourTemp;
            meta.Attributes.TryGetValue("colour", out ColourTemp);
            properties.Add("colour", ColourTemp);
            
            string RefTemp;
            meta.Attributes.TryGetValue("ref", out RefTemp);
            properties.Add("ref", RefTemp);
            
            float time;
            float dist;
            route.DistanceAndTimeAt(Instruction.Shape, out dist, out time);
            properties.Add("distance", dist.ToString(new CultureInfo("en-US")));

            if(Next != null){
                this.Direction = route.DirectionToNext(Instruction.Shape);
                properties.Add("direction", Direction.ToString());
                if (Instruction.Type != "start" && Instruction.Type != "stop"){
                    this.Angle = route.RelativeDirectionAt(Instruction.Shape);
                    properties.Add("angle", Angle.Angle.ToString(new CultureInfo("en-US")));
                }                
                float distanceNext;
                float timeNext;
                float distanceNow;
                float timeNow;
                route.DistanceAndTimeAt(Next.Shape, out distanceNext, out timeNext);
                route.DistanceAndTimeAt(Instruction.Shape, out distanceNow, out timeNow);
                properties.Add("distanceToNext", (distanceNext -distanceNow).ToString(new CultureInfo("en-US")));
            }            
        }
    }
}