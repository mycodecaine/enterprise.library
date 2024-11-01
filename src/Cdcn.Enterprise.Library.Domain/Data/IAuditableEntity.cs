using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Data
{
    public interface IAuditableEntity
    {
        /// <summary>
        /// Gets the created on date and time in UTC format.
        /// </summary>
        DateTime CreatedOnUtc { get; }

        /// <summary>
        /// Gets the modified on date and time in UTC format.
        /// </summary>
        DateTime? ModifiedOnUtc { get; }

        /// <summary>
        /// Created By
        /// </summary>
        Guid? CreatedBy { get; }

        /// <summary>
        /// ModifiedBy
        /// </summary>
        Guid? ModifiedBy { get; }

    }
}
