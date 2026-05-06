
using System.ComponentModel.DataAnnotations;

namespace InventoryTracker.WebOperator.ViewModels.Transactions
{
    public class CreateEditTransactionItemViewModel
    {
        [Required]
        public Guid ItemId { get; set; }
        public string? Name { get; set; }
        public decimal Quantity { get; set; }
    }
}
