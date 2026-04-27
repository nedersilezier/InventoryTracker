using InventoryTracker.APIClient;
using InventoryTracker.AuthClient;
using InventoryTracker.WebAdmin.Interfaces;
using InventoryTracker.WebAdmin.Providers;
using InventoryTracker.WebAdmin.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]!;

// !!! TO BE REMOVED after all services are updated to use ApiHttpClient
// Register HttpContextAccessor for accessing cookies in services
builder.Services.AddHttpContextAccessor();

// Register universal ApiHttpClient
builder.Services.AddHttpClient<ApiHttpClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<AccessTokenHandler>();

// Register AccessTokenProvider and HttpMessageHandler for attaching access tokens to outgoing requests
builder.Services.AddScoped<IAccessTokenProvider, HttpContextAccessTokenProvider>();
builder.Services.AddTransient<AccessTokenHandler>();

// Register HttpClient for Auth
builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});


//register ItemsService
builder.Services.AddScoped<IItemsService, ItemsService>();

//register ClientsService
builder.Services.AddScoped<IClientsService, ClientsService>();

//register LookupsService
builder.Services.AddScoped<ILookupsService, LookupsService>();

//register WarehousesService
builder.Services.AddScoped<IWarehousesService, WarehousesService>();

//register StocksService
builder.Services.AddScoped<IStocksService, StocksService>();

//register CountriesService
builder.Services.AddScoped<ICountriesService, CountriesService>();

//register UsersService
builder.Services.AddScoped<IUsersService, UsersService>();

//register TransactionsService
builder.Services.AddScoped<ITransactionsService, TransactionsService>();

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
