using System.Text.Json.Serialization;

namespace ICEDT.API.Middleware
{
    public record Response
    {
        [JsonPropertyName("result")]
        public object? Result { get; set; }
        [JsonPropertyName("isError")]
        public bool IsError { get; set; }
        [JsonPropertyName("error")]
        public Error? Error { get; set; }

        public Response() { }

        public Response(object? result, bool isError, Error? error)
        {
            Result = result;
            IsError = isError;
            Error = error;
        }
    }

    public record Error
    {
        [JsonPropertyName("title")]
        public string Title { get; private set; }
        [JsonPropertyName("details")]
        public string Details { get; private set; }
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; private set; }
        [JsonPropertyName("extensions")]
        public IDictionary<string, object?> Extensions { get; set; } = new Dictionary<string, object?>(StringComparer.Ordinal);

        public Error(string title, string details, int statusCode)
        {
            Title = title;
            Details = details;
            StatusCode = statusCode;
        }
    }
}