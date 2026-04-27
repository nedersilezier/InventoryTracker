using InventoryTracker.Contracts.Helpers;
using InventoryTracker.Contracts.Requests.Users;
using InventoryTracker.Contracts.Requests.Warehouses;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.Services;
using InventoryTracker.WebAdmin.ViewModels.HelperVMs;
using InventoryTracker.WebAdmin.ViewModels.Users;
using InventoryTracker.WebAdmin.ViewModels.Warehouses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static InventoryTracker.Contracts.Requests.Warehouses.UpdateWarehouseRequest;

namespace InventoryTracker.WebAdmin.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUsersService _usersService;
        private readonly ILookupsService _lookupsService;
        public UsersController(IUsersService usersService, ILookupsService lookupsService)
        {
            _usersService = usersService;
            _lookupsService = lookupsService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(GetUsersRequest request, CancellationToken cancellationToken)
        {
            var result = await _usersService.GetAllAsync(request, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to load users.";

                return View(new UsersIndexViewModel
                {
                    Users = new List<UserListItemViewModel>(),
                    SearchTerm = request.SearchTerm,
                    PageSize = request.PageSize ?? 5,
                    TableFooter = new TableFooterViewModel
                    {
                        DisplayedCount = 0,
                        TotalCount = 0,
                        EntityName = "users",
                        Pagination = new PaginationViewModel
                        {
                            CurrentPage = request.PageNumber,
                            TotalPages = 1,
                            PageSize = request.PageSize ?? 5,
                            Controller = "Users",
                            Action = "Index"
                        }
                    }
                });
            }
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var rolesResult = await GetRoleSelectListAsync(null, cancellationToken);

            if (!rolesResult.Success)
            {
                var authFailure = HandleAuthFailure(rolesResult);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = rolesResult.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new CreateUserViewModel
            {
                AvailableRoles = rolesResult.Data!
            };

            return View("Create", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                var rolesResult = await GetRoleSelectListAsync(null, cancellationToken);

                if (!rolesResult.Success)
                {
                    var authFailure = HandleAuthFailure(rolesResult);
                    if (authFailure is not null)
                        return authFailure;

                    TempData["ErrorMessage"] = rolesResult.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
                vm.AvailableRoles = rolesResult.Data!;
                return View("CreateEdit", vm);
            }

            var request = new CreateUserRequest
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber,
                Password = vm.Password,
                Role = vm.SelectedRole
            };

            var result = await _usersService.CreateUserAsync(request, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                AddServiceErrorsToModelState(result, "Unable to create user");
                var rolesResult = await GetRoleSelectListAsync(null, cancellationToken);

                if (!rolesResult.Success)
                {
                    authFailure = HandleAuthFailure(rolesResult);
                    if (authFailure is not null)
                        return authFailure;

                    TempData["ErrorMessage"] = rolesResult.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
                vm.AvailableRoles = rolesResult.Data!;
                return View("Create", vm);
            }
            TempData["SuccessMessage"] = $"User '{result?.Data?.Email}' created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id, CancellationToken cancellationToken)
        {
            var result = await _usersService.GetByIdAsync(id, cancellationToken);
            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to load user.";
                return RedirectToAction(nameof(Index));
            }
            var rolesResult = await GetRoleSelectListAsync(null, cancellationToken);

            if (!rolesResult.Success)
            {
                var authFailure = HandleAuthFailure(rolesResult);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = rolesResult.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            result.Data!.AvailableRoles = rolesResult.Data!;
            return View("Edit", result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                var rolesResult = await GetRoleSelectListAsync(null, cancellationToken);

                if (!rolesResult.Success)
                {
                    var authFailure = HandleAuthFailure(rolesResult);
                    if (authFailure is not null)
                        return authFailure;

                    TempData["ErrorMessage"] = rolesResult.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }

                vm.AvailableRoles = rolesResult.Data!;
                return View("Edit", vm);
            }
            var request = new UpdateUserRequest
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                PhoneNumber = vm.PhoneNumber,
                Role = vm.SelectedRole
            };
            var id = vm.UserId;
            var result = await _usersService.UpdateUserAsync(id, request, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                AddServiceErrorsToModelState(result, "Unable to update the user");
                var rolesResult = await GetRoleSelectListAsync(null, cancellationToken);

                if (!rolesResult.Success)
                {
                    authFailure = HandleAuthFailure(rolesResult);
                    if (authFailure is not null)
                        return authFailure;

                    TempData["ErrorMessage"] = rolesResult.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }

                vm.AvailableRoles = rolesResult.Data!;
                return View("Edit", vm);
            }
            TempData["SuccessMessage"] = $"User '{result?.Data?.Email}' updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(string id, CancellationToken cancellationToken)
        {
            var result = await _usersService.DeactivateUserAsync(id, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to deactivate user.";
            }
            else
            {
                TempData["SuccessMessage"] = $"User '{result.Data!.Email}' deactivated successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(string id, CancellationToken cancellationToken)
        {
            var result = await _usersService.ActivateUserAsync(id, cancellationToken);

            if (!result.Success)
            {
                var authFailure = HandleAuthFailure(result);
                if (authFailure is not null)
                    return authFailure;

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to activate user.";
            }
            else
            {
                TempData["SuccessMessage"] = $"User '{result.Data!.Email}' activated successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
        #region Helpers
        private async Task<ServiceResult<List<SelectListItem>>> GetRoleSelectListAsync(string? selectedRole, CancellationToken cancellationToken)
        {
            var rolesResult = await _lookupsService.GetRolesAsync(cancellationToken);

            if (!rolesResult.Success)
                return ServiceResult<List<SelectListItem>>.Fail(rolesResult.ErrorMessage ?? "Unable to load roles.", rolesResult.ValidationErrors, rolesResult.StatusCode);

            var items = rolesResult.Data!
                .Select(c => new SelectListItem
                {
                    Value = c.Name,
                    Text = c.Name,
                    Selected = !string.IsNullOrWhiteSpace(selectedRole) && c.Name == selectedRole
                })
                .ToList();

            return ServiceResult<List<SelectListItem>>.Ok(items);
        }
        #endregion
    }
}
