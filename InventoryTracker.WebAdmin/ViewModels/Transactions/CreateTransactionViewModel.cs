using InventoryTracker.Shared.Enums;
using InventoryTracker.WebAdmin.ViewModels.Items;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InventoryTracker.WebAdmin.ViewModels.Transactions
{
    public class CreateTransactionViewModel
    {
        [Required(ErrorMessage = "Transaction type is required.")]
        public TransactionType Type { get; set; } = TransactionType.Adjustment;
        public Guid? ClientId { get; set; }
        public Guid? SourceWarehouseId { get; set; }
        public Guid? DestinationWarehouseId { get; set; }

        [Required(ErrorMessage = "Transaction date is required.")]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public List<SelectListItem> AvailableTypes = new List<SelectListItem>
        {
            new SelectListItem { Text = "Adjustment", Value = TransactionType.Adjustment.ToString() },
            new SelectListItem { Text = "Issue To Client", Value = TransactionType.IssueToClient.ToString() },
            new SelectListItem { Text = "Return From Client", Value = TransactionType.ReturnFromClient.ToString() },
            new SelectListItem { Text = "Transfer Between Warehouses", Value = TransactionType.TransferBetweenWarehouses.ToString() },
        };
        public List<ItemSelectOption> AvailableItems { get; set; } = new();
        public List<SelectListItem> AvailableClients { get; set; } = new();
        public List<SelectListItem> AvailableWarehouses { get; set; } = new();
        public List<CreateTransactionItemViewModel> SelectedItems { get; set; } = new();
    }
}
