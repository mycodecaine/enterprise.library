using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Exceptions
{
    public static class ExceptionExtension
    {
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

        private static object GetExceptionDetails(Exception? ex)
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
