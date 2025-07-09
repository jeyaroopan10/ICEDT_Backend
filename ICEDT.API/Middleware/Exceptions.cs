namespace ICEDT.API.Middleware
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(string message, IDictionary<string, string[]> errors = null)
            : base(message)
        {
            Errors = errors ?? new Dictionary<string, string[]>();
        }
    }
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class ConflictException : Exception
    {
        public ConflictException() { }

        public ConflictException(string message) : base(message) { }

        public ConflictException(string message, Exception inner) : base(message, inner) { }
    }

}