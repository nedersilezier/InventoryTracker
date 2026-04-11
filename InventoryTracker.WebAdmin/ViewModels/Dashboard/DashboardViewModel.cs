using InventoryTracker.Contracts.Responses.Transactions;

namespace InventoryTracker.WebAdmin.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public int TotalItems { get; set; }
        public int ActiveClients { get; set; }
        public int Warehouses { get; set; }
        public int StockCapacity { get; set; }

        public List<TransactionListDTO> RecentTransactions { get; set; } = new();
    }
}
