using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Contracts.Requests.Transactions
{
    public class GetTransactionsRequest
    {
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool IncludeAdjustments { get; set; } = true;
        public bool IncludeTransfers { get; set; } = true;
        public bool IncludeIssues { get; set; } = true;
        public bool IncludeReturns { get; set; } = true;
    }
}
