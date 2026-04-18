using InventoryTracker.Contracts.Responses.Transactions;
using InventoryTracker.Shared.Enums;
using InventoryTracker.WebOperator.ViewModels;
using InventoryTracker.WebOperator.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc;


namespace InventoryTracker.WebOperator.Controllers
{
    public class TransactionsController : Controller
    {
        public IActionResult Index(TransactionType? type, TransactionStatus? status, int PageNumber = 1, int pageLength = 4)
        {
            var testTransactions = new List<TransactionCardViewModel>
                {
                    new TransactionCardViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX001", FromDisplay = "Warehouse 1", ToDisplay = "Warehouse 2", TransactionDate = DateTime.Now, Type = TransactionType.TransferBetweenWarehouses, TypeDisplay = "Transfer", Status = TransactionStatus.Approved },
                    new TransactionCardViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX002", FromDisplay = "Warehouse 2", ToDisplay = "Warehouse 3", TransactionDate = DateTime.Now, Type = TransactionType.TransferBetweenWarehouses, TypeDisplay = "Transfer", Status = TransactionStatus.Approved },
                    new TransactionCardViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX003", FromDisplay = "Warehouse 3", ToDisplay = "Warehouse 4", TransactionDate = DateTime.Now, Type = TransactionType.TransferBetweenWarehouses, TypeDisplay = "Transfer", Status = TransactionStatus.Cancelled },
                    new TransactionCardViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX004", FromDisplay = "Warehouse 4", ToDisplay = "Warehouse 5", TransactionDate = DateTime.Now, Type = TransactionType.TransferBetweenWarehouses, TypeDisplay = "Transfer", Status = TransactionStatus.Approved },
                    new TransactionCardViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX005", FromDisplay = "Warehouse 5", ToDisplay = "Warehouse 6", TransactionDate = DateTime.Now, Type = TransactionType.TransferBetweenWarehouses, TypeDisplay = "Transfer", Status = TransactionStatus.Draft },
                    new TransactionCardViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX006", FromDisplay = "", ToDisplay = "Warehouse 1", TransactionDate = DateTime.Now, Type = TransactionType.Adjustment, TypeDisplay = "Adjustment", Status = TransactionStatus.Draft },
                    new TransactionCardViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX007", FromDisplay = "Client 11", ToDisplay = "Warehouse 3", TransactionDate = DateTime.Now, Type = TransactionType.ReturnFromClient, TypeDisplay = "Return from client", Status = TransactionStatus.Approved },
                    new TransactionCardViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX008", FromDisplay = "Warehouse 3", ToDisplay = "Client", TransactionDate = DateTime.Now, Type = TransactionType.IssueToClient, TypeDisplay = "Issue to client", Status = TransactionStatus.Draft },
                    new TransactionCardViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX008", FromDisplay = "Warehouse 3", ToDisplay = "Client", TransactionDate = DateTime.Now, Type = TransactionType.IssueToClient, TypeDisplay = "Issue to client", Status = TransactionStatus.Draft },
                    new TransactionCardViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX008", FromDisplay = "Warehouse 3", ToDisplay = "Client", TransactionDate = DateTime.Now, Type = TransactionType.IssueToClient, TypeDisplay = "Issue to client", Status = TransactionStatus.Draft },
                    new TransactionCardViewModel {TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX008", FromDisplay = "Warehouse 3", ToDisplay = "Client", TransactionDate = DateTime.Now, Type = TransactionType.IssueToClient, TypeDisplay = "Issue to client", Status = TransactionStatus.Draft}
                };
            var testQuery = testTransactions.AsQueryable();

            if (type != null || status != null)
                testQuery = testQuery.Where(t => t.Type == type || t.Status == status);
            var totalCount = testQuery.Count();
            var testVm = new OperatorTransactionsIndexViewModel
            {
                TypeFilter = type,
                StatusFilter = status,
                PageTitle = BuildPageTitle(type, status),
                PageDescription = $"Showing {totalCount} transaction records.",
                Pagination = new PaginationViewModel
                {
                    CurrentPage = PageNumber,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageLength),
                    PageSize = pageLength,
                    Controller = "Transactions",
                    Action = "Index",
                    RouteValues = new Dictionary<string, string?>
                    {
                        ["type"] = type?.ToString(),
                        ["status"] = status?.ToString()
                    }
                }
            };
            testVm.Transactions = new List<TransactionCardViewModel>(testQuery.Skip((PageNumber - 1) * pageLength).Take(pageLength).ToList());
            return View(testVm);
        }
        private static string BuildPageTitle(TransactionType? type, TransactionStatus? status)
        {
            if (status == TransactionStatus.Draft)
                return "Draft Transactions";

            return type switch
            {
                TransactionType.ReturnFromClient => "Return Transactions",
                TransactionType.IssueToClient => "Issue Transactions",
                TransactionType.TransferBetweenWarehouses => "Transfer Transactions",
                TransactionType.Adjustment => "Adjustment Transactions",
                _ => "All Transactions"
            };
        }
    }
}
