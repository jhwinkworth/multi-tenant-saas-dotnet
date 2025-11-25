using Api.Middleware;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Infrastructure.Repositories;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// Add Controllers + Swagger
// ---------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------------------
// Add HttpContextAccessor & Tenant Provider
// ---------------------------
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();

// ---------------------------
// Add DbContext (PostgreSQL)
// ---------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ---------------------------
// Register Repositories
// ---------------------------
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ---------------------------
// Register Services
// ---------------------------
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskItemService, TaskItemService>();
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// ---------------------------
// JWT Authentication
// ---------------------------
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateLifetime = true
    };
});

// ---------------------------
// Build & Middleware
// ---------------------------

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseTenantMiddleware();
app.UseAuthorization();

app.MapControllers();

app.Run();
