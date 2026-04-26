using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Stocks;
using InventoryTracker.WebAdmin.ViewModels.Stocks;

namespace InventoryTracker.WebAdmin.Interfaces
{
    public interface IStocksService
    {
        Task<ServiceResult<StocksIndexViewModel>> GetAllAsync(GetStocksRequest request, CancellationToken cancellationToken);
    }
}
