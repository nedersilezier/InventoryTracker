using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Features.Transactions.DTOs
{
    public class GetUsersDraftsParameters : GetTransactionsParameters
    {
        public string UserName { get; set; } = default!;
    }
}
