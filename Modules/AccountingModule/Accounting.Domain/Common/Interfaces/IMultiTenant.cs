using System;

namespace Accounting.Domain.Common.Interfaces
{
    public interface IMultiTenant
    {
        Guid TenantId { get; set; }
    }
}
