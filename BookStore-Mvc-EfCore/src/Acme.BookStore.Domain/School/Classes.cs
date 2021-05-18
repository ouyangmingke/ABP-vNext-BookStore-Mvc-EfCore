using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.School
{
    public class Classes : AuditedAggregateRoot<int>,IMultiTenant
    {
        public string CLassNo { get; set; }
        public string CLassName { get; set; }

        public Guid? TenantId { get; }
    }
}
