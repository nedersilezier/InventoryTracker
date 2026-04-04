using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Domain.Common
{
    public abstract class SoftDeletableEntity : AuditableEntity
    {
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
