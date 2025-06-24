using MudBlazor.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ خدمات واجهة Blazor كاملة
builder.Services.AddRazorPages();      // ⬅️ يسجِّل PersistentComponentState
builder.Services.AddServerSideBlazor();

// 2️⃣ MudBlazor و بقية الخدمات
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddHttpClient("ERPApi", c =>
{
    c.BaseAddress = new Uri("https://localhost:5000");
});

builder.Services.AddScoped<JwtAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthStateProvider>());
builder.Services.AddAuthorizationCore();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Error");

app.UseStaticFiles();
app.UseRouting();

// 3️⃣ نقاط النهاية
app.MapRazorPages();   // ⬅️ مطلوب مع AddRazorPages
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
