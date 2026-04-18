using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebOperator.ViewModels
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