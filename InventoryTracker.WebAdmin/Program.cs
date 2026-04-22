using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.Services;
using InventoryTracker.AuthClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]!;

// Register HttpClient for Countries
builder.Services.AddHttpClient<ICountriesService, CountriesService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Register HttpClient for Auth
builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Register HttpClient for Transactions
builder.Services.AddHttpClient<ITransactionsService, TransactionsService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Register HttpClient for Users
builder.Services.AddHttpClient<IUsersService, UsersService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Register HttpClient for Stocks
builder.Services.AddHttpClient<IStocksService, StocksService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// register HttpClient for Warehouses
builder.Services.AddHttpClient<IWarehousesService, WarehousesService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// register HttpClient for Items
builder.Services.AddHttpClient<IItemsService, ItemsService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// register HttpClient for Clients
builder.Services.AddHttpClient<IClientsService, ClientsService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Register HttpContextAccessor for accessing cookies in services
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
