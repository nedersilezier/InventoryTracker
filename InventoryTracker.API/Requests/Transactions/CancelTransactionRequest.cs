namespace InventoryTracker.API.Requests.Transactions
{
    public class CancelTransactionRequest
    {
        public string CancellationReason { get; set; } = string.Empty;
    }
}
