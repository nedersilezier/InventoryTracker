
using System.ComponentModel.DataAnnotations;

namespace InventoryTracker.WebAdmin.ViewModels.Transactions
{
    public class CreateTransactionItemViewModel
    {
        [Required]
        public Guid ItemId { get; set; }
        public string Name { get; set; } = default!;
        public decimal Quantity { get; set; }
    }
}
