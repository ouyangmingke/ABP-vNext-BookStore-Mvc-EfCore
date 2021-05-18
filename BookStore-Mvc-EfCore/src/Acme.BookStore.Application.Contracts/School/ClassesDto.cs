using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.School
{
    public class ClassesDto:AuditedEntityDto<int>
    {
        public string CLassNo { get; set; }
        public string CLassName { get; set; }
        public Guid? TenantId { get; }
    }
}
