using Itinero.Navigation.Instructions;

namespace rideaway_backend.Model{
    public class InstructionProperties {
        public Instruction Instruction {get; set;}

        public InstructionProperties(Instruction Instruction){
            this.Instruction = Instruction;
        }
    }
}