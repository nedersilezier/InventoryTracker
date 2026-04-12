using InventoryTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Shared.Extensions
{
    public static class TransactionTypeExtensions
    {
        public static string ToDisplayName(TransactionType type)
        {
            switch (type)
            {
                case TransactionType.TransferBetweenWarehouses:
                    return "Transfer";
                case TransactionType.IssueToClient:
                    return "Issue to client";
                case TransactionType.Adjustment:
                    return "Adjustment";
                case TransactionType.ReturnFromClient:
                    return "Return";
                default:
                    return type.ToString();
            }
        }
    }
}
