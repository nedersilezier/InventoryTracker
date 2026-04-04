using InventoryTracker.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Domain.Enums
{
    public enum TransactionType
    {
        IssueToClient = 1,
        ReturnFromClient = 2,
        TransferBetweenWarehouses = 3,
        Adjustment = 4
    }
}
