
namespace Telemetry.Application.Exceptions
{

    public class ApplicationTelemetryException : Exception
    {
        public ApplicationTelemetryException(string message) : base(message) { }
    }
}
