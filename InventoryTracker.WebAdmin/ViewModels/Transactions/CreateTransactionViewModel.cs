using InventoryTracker.Contracts.Requests.Transactions;
using InventoryTracker.Shared.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InventoryTracker.WebAdmin.ViewModels.Transactions
{
    public class CreateTransactionViewModel
    {
        [Required(ErrorMessage = "Transaction type is required.")]
        public TransactionType Type { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? SourceWarehouseId { get; set; }
        public Guid? DestinationWarehouseId { get; set; }

        [Required(ErrorMessage = "Transaction date is required.")]
        public DateTime TransactionDate { get; set; } = DateTime.Today;
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public List<SelectListItem> AvailableItems { get; set; } = new();
        public List<SelectListItem> AvailableClients { get; set; } = new();
        public List<SelectListItem> AvailableWarehouses { get; set; } = new();
        public List<CreateTransactionItemViewModel> SelectedItems { get; set; } = new();
    }
}
