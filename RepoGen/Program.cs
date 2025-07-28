using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using RepoGen.Contexts;
using RepoGen.Interface;
using RepoGen.Interface.EntitiesSlices.Auth;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddCascadingAuthenticationState();

// Auth
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth";
        options.LogoutPath = "/logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/";
        options.Events.OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.Redirect("/");
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToLogin = ctx =>
        {
            ctx.Response.Redirect("/");
            return Task.CompletedTask;
        };
    });

// Context
builder.Services.AddDbContextFactory<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Services injections
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddScoped(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navigationManager.BaseUri) };
});

// Radzen
builder.Services.AddRadzenComponents();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapPost("/auth/login", async (
    AuthDto auth,
    IMediator mediator,
    HttpContext httpContext
) =>
{
    try
    {
        var principal = await mediator.Send(new AuthCommand(auth.Login, auth.Password));
        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/auth/logout", async (
    HttpContext httpContext
) =>
{
    try
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Results.Redirect("/");
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();