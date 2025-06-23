using ERP.Infrastructure.Persistence;
using ERP.Infrastructure.MultiTenancy;
using ERP.Infrastructure.Security;
using ERP.Application.Common.Interfaces;
using ERP.Application.Features.Financial.Commands.CreateAccount;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using ERP.Infrastructure.Identity;
using MediatR;
using ERP.Infrastructure.Middleware;
var builder = WebApplication.CreateBuilder(args);

// ConnectionStrings
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Server=.\\SQLEXPRESS;Database=ERP;User Id=admin_mandarin;Password=mandarin2021@;TrustServerCertificate=True";

builder.Services.AddHttpContextAccessor();
builder.Services.AddStackExchangeRedisCache(o => o.Configuration = "redis:6379");

builder.Services.AddAuthentication().AddJwtBearer(); // ← تبسيط، استخدم المفاتيح من JwtTokenService


// DB + Identity
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
       .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// MediatR + AutoMapper
builder.Services.AddMediatR(typeof(CreateAccountCommand).Assembly);   // ✅ توقيع الإصدار 11
builder.Services.AddAutoMapper(typeof(ERP.Application.Common.DTOs.AccountDto).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ERP API", Version = "v1" });
});

var app = builder.Build();

app.UseMiddleware<AuditMiddleware>();   // تسجيل التدقيق
app.UseAuthentication();
app.UseAuthorization();

// Use Tenant Middleware
app.UseMiddleware<TenantMiddleware>();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP API v1"));

app.MapControllers();
app.Run();
