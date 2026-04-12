using InventoryTracker.Shared.Enums;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Transactions;
using InventoryTracker.WebAdmin.ViewModels.Warehouses;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class TransactionsController : Controller
    {
        public IActionResult Index(int page = 1, string? searchTerm = null, int pageLenght = 80)
        {
            var testTransactions = new List<TransactionListItemViewModel>
                {
                    new TransactionListItemViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX001", Quantity = 100, QuantityDisplay = "100 pcs", TotalCreditValue = 1000, TotalCreditValueDisplay = "$1000", FromDisplay = "Warehouse 1", ToDisplay = "Warehouse 2", TransactionDate = DateTime.Now, Type = TransactionType.TransferBetweenWarehouses, TypeDisplay = "Transfer", Status = TransactionStatus.Approved },
                    new TransactionListItemViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX002", Quantity = 200, QuantityDisplay = "200 pcs", TotalCreditValue = 2000, TotalCreditValueDisplay = "$2000", FromDisplay = "Warehouse 2", ToDisplay = "Warehouse 3", TransactionDate = DateTime.Now, Type = TransactionType.TransferBetweenWarehouses, TypeDisplay = "Transfer", Status = TransactionStatus.Approved },
                    new TransactionListItemViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX003", Quantity = 300, QuantityDisplay = "300 pcs", TotalCreditValue = 3000, TotalCreditValueDisplay = "$3000", FromDisplay = "Warehouse 3", ToDisplay = "Warehouse 4", TransactionDate = DateTime.Now, Type = TransactionType.TransferBetweenWarehouses, TypeDisplay = "Transfer", Status = TransactionStatus.Cancelled },
                    new TransactionListItemViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX004", Quantity = 400, QuantityDisplay = "400 pcs", TotalCreditValue = 4000, TotalCreditValueDisplay = "$4000", FromDisplay = "Warehouse 4", ToDisplay = "Warehouse 5", TransactionDate = DateTime.Now, Type = TransactionType.TransferBetweenWarehouses, TypeDisplay = "Transfer", Status = TransactionStatus.Approved },
                    new TransactionListItemViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX005", Quantity = 500, QuantityDisplay = "500 pcs", TotalCreditValue = 5000, TotalCreditValueDisplay = "$5000", FromDisplay = "Warehouse 5", ToDisplay = "Warehouse 6", TransactionDate = DateTime.Now, Type = TransactionType.TransferBetweenWarehouses, TypeDisplay = "Transfer", Status = TransactionStatus.Draft },
                    new TransactionListItemViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX006", Quantity = 500, QuantityDisplay = "500 pcs", TotalCreditValue = 5000, TotalCreditValueDisplay = "$5000", FromDisplay = "", ToDisplay = "Warehouse 1", TransactionDate = DateTime.Now, Type = TransactionType.Adjustment, TypeDisplay = "Adjustment", Status = TransactionStatus.Draft },
                    new TransactionListItemViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX007", Quantity = 500, QuantityDisplay = "500 pcs", TotalCreditValue = 5000, TotalCreditValueDisplay = "$5000", FromDisplay = "Client 11", ToDisplay = "Warehouse 3", TransactionDate = DateTime.Now, Type = TransactionType.ReturnFromClient, TypeDisplay = "Return from client", Status = TransactionStatus.Approved },
                    new TransactionListItemViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX008", Quantity = 500, QuantityDisplay = "500 pcs", TotalCreditValue = 5000, TotalCreditValueDisplay = "$5000", FromDisplay = "Warehouse 3", ToDisplay = "Client", TransactionDate = DateTime.Now, Type = TransactionType.IssueToClient, TypeDisplay = "Issue to client", Status = TransactionStatus.Draft },
                    new TransactionListItemViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX008", Quantity = 500, QuantityDisplay = "500 pcs", TotalCreditValue = 5000, TotalCreditValueDisplay = "$5000", FromDisplay = "Warehouse 3", ToDisplay = "Client", TransactionDate = DateTime.Now, Type = TransactionType.IssueToClient, TypeDisplay = "Issue to client", Status = TransactionStatus.Draft },
                    new TransactionListItemViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX008", Quantity = 500, QuantityDisplay = "500 pcs", TotalCreditValue = 5000, TotalCreditValueDisplay = "$5000", FromDisplay = "Warehouse 3", ToDisplay = "Client", TransactionDate = DateTime.Now, Type = TransactionType.IssueToClient, TypeDisplay = "Issue to client", Status = TransactionStatus.Draft },
                    new TransactionListItemViewModel { TransactionId = Guid.NewGuid(), ReferenceNumber = "TRX008", Quantity = 500, QuantityDisplay = "500 pcs", TotalCreditValue = 5000, TotalCreditValueDisplay = "$5000", FromDisplay = "Warehouse 3", ToDisplay = "Client", TransactionDate = DateTime.Now, Type = TransactionType.IssueToClient, TypeDisplay = "Issue to client", Status = TransactionStatus.Draft }
                };

            var testQuery = testTransactions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                testQuery = testQuery.Where(item => item.ReferenceNumber.Contains(searchTerm) || item.FromDisplay.Contains(searchTerm) || item.ToDisplay.Contains(searchTerm) || item.TypeDisplay.Contains(searchTerm) || item.StatusDisplay.Contains(searchTerm));

            var testViewModel = new TransactionsIndexViewModel
            {
                TotalCount = testTransactions.Count,
                Filters = new TransactionFiltersViewModel { SearchTerm = searchTerm },
                Pagination = new PaginationViewModel
                {
                    CurrentPage = page,
                    Controller = "Transactions",
                    TotalPages = (int)Math.Ceiling((double)testTransactions.Count / pageLenght),
                    RouteValues = new Dictionary<string, string?> { { "searchTerm", searchTerm } }
                }
            };
            testViewModel.Transactions = new List<TransactionListItemViewModel>(testQuery.Skip((page - 1) * pageLenght).Take(pageLenght).ToList());
            return View(testViewModel);
        }
    }
}
