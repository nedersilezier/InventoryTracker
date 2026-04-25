using InventoryTracker.Contracts.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.WebAdmin.Controllers
{
    public abstract class BaseController : Controller
    {
        protected void AddServiceErrorsToModelState<T>(ServiceResult<T> result, string fallbackMessage = "Operation failed.")
        {
            var hasAnyError = false;

            if (result.ValidationErrors is not null)
            {
                foreach (var error in result.ValidationErrors)
                {
                    var key = string.Equals(error.Key, "General", StringComparison.OrdinalIgnoreCase)
                        ? string.Empty
                        : error.Key;

                    foreach (var message in error.Value)
                    {
                        ModelState.AddModelError(key, message);
                        hasAnyError = true;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                hasAnyError = true;
            }

            if (!hasAnyError)
            {
                ModelState.AddModelError(string.Empty, fallbackMessage);
            }
        }
    }
}
