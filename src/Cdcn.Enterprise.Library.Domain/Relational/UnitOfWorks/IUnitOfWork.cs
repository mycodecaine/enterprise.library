﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Relational.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(Guid saveBy, CancellationToken cancellationToken = default);

    }
}