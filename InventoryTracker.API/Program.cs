using FluentValidation;
using InventoryTracker.API.Middleware;
using InventoryTracker.API.Services;
using InventoryTracker.Application;
using InventoryTracker.Application.Common.Behaviors;
using InventoryTracker.Application.Common.Interfaces;
using InventoryTracker.Infrastructure.Identity;
using InventoryTracker.Infrastructure.Identity.Entities;
using InventoryTracker.Infrastructure.Persistence;
using InventoryTracker.Infrastructure.Repositories;
using InventoryTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    //Authentication configuration for Swagger/OpenAPI
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Description = "Enter your JWT token"
        };
        document.Security ??= [];
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
        });
        return Task.CompletedTask;
    });
});

// Configure Entity Framework and Identity
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<JwtTokenGenerator>();

// Configure JWT authentication
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
                  ?? throw new InvalidOperationException("Jwt settings are missing.");
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });
// Register the refresh token generator
builder.Services.AddScoped<RefreshTokenGenerator>();

// Configure MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(InventoryTracker.Application.AssemblyReference).Assembly);
    //validation pipeline
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

// Register FluentValidation validators from Application assembly
builder.Services.AddValidatorsFromAssembly(typeof(InventoryTracker.Application.AssemblyReference).Assembly);

// Register the IAppDbContext to resolve to AppDbContext
builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

// Register the current user service
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Register global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// register identity service
builder.Services.AddScoped<IIdentityService, IdentityService>();

// register users service
builder.Services.AddScoped<IUsersService, UsersService>();

// register countries service
builder.Services.AddScoped<ICountriesService, CountriesService>();

// register clients service
builder.Services.AddScoped<IClientsQueryService, ClientsQueryService>();

builder.Services.AddScoped<IWarehousesQueryService, WarehousesQueryService>();
builder.Services.AddScoped<IItemsQueryService, ItemsQueryService>();

// register repositories
builder.Services.AddScoped<IAddressesRepository, AddressesRepository>();
builder.Services.AddScoped<IClientsRepository, ClientsRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IWarehousesRepository, WarehousesRepository>();
builder.Services.AddScoped<IItemsRepository, ItemsRepository>();

// register authorization services
builder.Services.AddAuthorization();

// Configure CORS to allow requests from frontend applications
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(
                "https://localhost:7244", // WebAdmin
                "https://localhost:7141/" // WebOperator
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "InventoryTracker API");
    });
}

app.UseExceptionHandler();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var configuration = services.GetRequiredService<IConfiguration>();

    await IdentitySeeder.SeedAsync(roleManager, userManager, configuration);
}

app.Run();
