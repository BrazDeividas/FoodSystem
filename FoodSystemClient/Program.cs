using FoodSystemClient.Authentication;
using FoodSystemClient.Components;
using FoodSystemClient.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBootstrapBlazor();


 builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<StorageService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();

builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddHttpClient("FoodSystemAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5173/");
});

builder.Services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.Authority = "http://localhost:5173";
    options.Audience = "FoodSystemAPI";
    options.SaveToken = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
