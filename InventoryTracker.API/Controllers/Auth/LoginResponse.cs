namespace InventoryTracker.API.Controllers.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
