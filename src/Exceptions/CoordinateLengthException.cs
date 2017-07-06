namespace rideaway_backend.Exceptions
{
    public class CoordinateLengthException: System.Exception 
    {
        public CoordinateLengthException(): base() { }
        public CoordinateLengthException(string message): base(message) { }
        public CoordinateLengthException(string message, System.Exception inner): base(message, inner) { }

    }

}