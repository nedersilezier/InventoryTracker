using InventoryTracker.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Domain.Entities
{
    public class Country : AuditableEntity
    {
        public Guid CountryId { get; set; }
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
