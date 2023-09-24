using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using OnlineBrief24.Auth;
using OnlineBrief24.Models;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<LdapAuthenticator, LdapAuthenticator>();

// Get ConnString values from env variables
var db = Environment.GetEnvironmentVariable("DB_DATABASE");
var host = Environment.GetEnvironmentVariable("DB_HOST");
var user = Environment.GetEnvironmentVariable("DB_USER");
var pw = Environment.GetEnvironmentVariable("DB_PASSWORD");

builder.Services.AddDbContextPool<OnlineBrief24Context>(o => o.UseSqlServer($"Data Source = {host}; Database={db};Integrated Security = True; Connect Timeout = 30; Encrypt = False; Trust Server Certificate=False; Application Intent = ReadWrite; Multi Subnet Failover=False\r\n" ?? throw new InvalidOperationException("Invalid ConnectionString"))
													  .EnableDetailedErrors()
													  .EnableThreadSafetyChecks()
													  .LogTo(Console.WriteLine));


builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ITicketStore, InMemoryTicketStore>(serviceProvider =>
{
    return new InMemoryTicketStore(serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IMemoryCache>()!);
});

builder.Services.AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme)

                .Configure<ITicketStore>((o, ticketStore) =>
                {
                    o.LoginPath = "/";
                    o.LogoutPath = "/logout";
                    o.AccessDeniedPath = "/accessDenied";
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    o.SessionStore = ticketStore;
                    o.Cookie.HttpOnly = true;
                    o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    o.Cookie.SameSite = SameSiteMode.Strict;
                    o.Cookie.Name = "session";
                });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie("Cookies");

builder.Services.AddAntiforgery(o =>
{
    o.SuppressXFrameOptionsHeader = false;
    o.HeaderName = "X-CSRF-TOKEN";
    o.Cookie.HttpOnly = true;
    o.Cookie.SameSite = SameSiteMode.Strict;
    o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
