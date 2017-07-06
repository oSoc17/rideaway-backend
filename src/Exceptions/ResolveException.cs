namespace rideaway_backend.Exceptions
{
    public class ResolveException : System.Exception {
        public ResolveException(): base() { }
        public ResolveException(string message): base(message) { }
        public ResolveException(string message, System.Exception inner): base(message, inner) { }
    }
}