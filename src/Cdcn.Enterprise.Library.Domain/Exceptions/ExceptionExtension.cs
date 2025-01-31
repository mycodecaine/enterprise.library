using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cdcn.Enterprise.Library.Domain.Exceptions
{
    /// <summary>
    /// Provides extension methods for the <see cref="Exception"/> class.
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// Converts the exception to a JSON string representation.
        /// </summary>
        /// <param name="ex">The exception to convert.</param>
        /// <returns>A JSON string representation of the exception.</returns>
        public static string ToJsonString(this Exception ex)
        {
            if (ex == null)
                return JsonSerializer.Serialize(new { Error = "No exception information provided." });

            var exceptionDetails = GetExceptionDetails(ex);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // for pretty-printing
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            return JsonSerializer.Serialize(exceptionDetails, options);
        }

        /// <summary>
        /// Recursively retrieves the details of the exception.
        /// </summary>
        /// <param name="ex">The exception to retrieve details from.</param>
        /// <returns>An object containing the details of the exception.</returns>
        private static object? GetExceptionDetails(Exception? ex)
        {
            if (ex == null) return null;

            return new
            {
                Type = ex.GetType().FullName,
                Message = ex.Message,
                Source = ex.Source,
                TargetSite = ex.TargetSite?.ToString(),
                StackTrace = ex.StackTrace,
                InnerException = ex.InnerException != null ? GetExceptionDetails(ex.InnerException) : null
            };
        }
    }

}
