namespace Patreon.Net
{
    /// <summary>
    /// The exception that is thrown when a request to a Patreon API endpoint returns an unexpected response or an error.
    /// </summary>
    public class PatreonApiException : System.Exception
    {
        public PatreonApiException(string message) : base(message) { }
    }
}
