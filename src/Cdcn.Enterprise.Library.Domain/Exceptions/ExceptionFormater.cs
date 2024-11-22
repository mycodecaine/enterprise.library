using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Exceptions
{
    public static class ExceptionFormatter
    {
        public static string FormatExceptionMessage(this Exception ex)
        {
            if (ex == null)
                return "No exception information provided.";

            var builder = new StringBuilder();

            builder.AppendLine($"Exception Type: {ex.GetType().FullName}");
            builder.AppendLine($"Message       : {ex.Message}");
            builder.AppendLine($"Source        : {ex.Source}");
            builder.AppendLine($"Target Site   : {ex.TargetSite}");
            builder.AppendLine("Stack Trace   :");
            builder.AppendLine(ex.StackTrace);

            // Include inner exception details, if any
            var inner = ex.InnerException;
            while (inner != null)
            {
                builder.AppendLine();
                builder.AppendLine("Caused By:");
                builder.AppendLine($"  Exception Type: {inner.GetType().FullName}");
                builder.AppendLine($"  Message       : {inner.Message}");
                builder.AppendLine($"  Source        : {inner.Source}");
                builder.AppendLine($"  Target Site   : {inner.TargetSite}");
                builder.AppendLine("  Stack Trace   :");
                builder.AppendLine(inner.StackTrace);

                inner = inner.InnerException;
            }

            return builder.ToString();
        }
    }

}
