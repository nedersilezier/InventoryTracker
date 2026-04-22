using InventoryTracker.Application.Common.DTOs;
using InventoryTracker.Application.Features.Transactions.DTOs;
using MediatR;


namespace InventoryTracker.Application.Features.Transactions.Queries.GetTransactions
{
    public class GetTransactionsQuery: IRequest<PagedResult<TransactionListDTO>>
    {
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool IncludeAdjustments { get; set; } = true;
        public bool IncludeTransfers { get; set; } = true;
        public bool IncludeIssues { get; set; } = true;
        public bool IncludeReturns { get; set; } = true;
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
