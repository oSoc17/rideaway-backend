using Itinero.Navigation.Instructions;
using Itinero.Navigation.Directions;
using Itinero;

namespace rideaway_backend.Model{
    public class InstructionProperties {
        public Instruction Instruction {get; set;}

        public float CurrentDistance {get; set;}

        public float DistanceToNext {get;set;}

        public string Color {get; set;}

        public string RouteRef {get; set;}

        public DirectionEnum Direction {get; set;}

        public RelativeDirection Angle {get; set;}

        public InstructionProperties(Instruction Instruction, Instruction Next, Route route){
            this.Instruction = Instruction;
            Route.Meta meta =  route.ShapeMetaFor(Instruction.Shape);
            string ColorTemp;
            meta.Attributes.TryGetValue("color", out ColorTemp);
            this.Color = ColorTemp;
            string RefTemp;
            meta.Attributes.TryGetValue("ref", out RefTemp);
            this.RouteRef = RefTemp;
            float time;
            float dist;
            route.DistanceAndTimeAt(Instruction.Shape, out dist, out time);
            this.CurrentDistance = dist;

            if(Next != null){
                this.Direction = route.DirectionToNext(Instruction.Shape);
                if (Instruction.Type != "start" && Instruction.Type != "stop"){
                    this.Angle = route.RelativeDirectionAt(Instruction.Shape);
                }                
                float distanceNext;
                float timeNext;
                float distanceNow;
                float timeNow;
                route.DistanceAndTimeAt(Next.Shape, out distanceNext, out timeNext);
                route.DistanceAndTimeAt(Instruction.Shape, out distanceNow, out timeNow);
                this.DistanceToNext = distanceNext - distanceNow;
            }            
        }
    }
}