using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.ViewModels.HelperVMs
{
    public class PaginationViewModel
    {
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int PageSize { get; set; } = 1;
        public string Action { get; set; } = "Index";
        public string Controller { get; set; } = "";
        public Dictionary<string, string?> RouteValues { get; set; } = new();
    }
}