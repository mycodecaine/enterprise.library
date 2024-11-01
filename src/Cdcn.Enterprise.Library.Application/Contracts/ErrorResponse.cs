using Cdcn.Enterprise.Library.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Contracts
{
    public class ErrorResponse
    {
        public ErrorResponse(IReadOnlyCollection<Error> errors) => Errors = errors;

        /// <summary>
        /// Gets the errors.
        /// </summary>
        public IReadOnlyCollection<Error> Errors { get; }
    }
}
