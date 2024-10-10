using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Data
{
    public interface ISoftDeletableEntity
    {
        /// <summary>
        /// Gets the date and time in UTC format the entity was deleted on.
        /// </summary>
        DateTime? DeletedOnUtc { get; }

        /// <summary>
        /// Gets a value indicating whether the entity has been deleted.
        /// </summary>
        bool Deleted { get; }
    }
}
