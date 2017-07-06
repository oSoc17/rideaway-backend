namespace rideaway_backend.Exceptions
{
    public class CoordinateParseException : System.Exception
    {
        public CoordinateParseException() : base() { }
        public CoordinateParseException(string message) : base(message) { }
        public CoordinateParseException(string message, System.Exception inner) : base(message, inner) { }
    }
}
