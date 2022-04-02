using Application.Interfaces.Contexts;
using Application.VisitorOnline;
using Application.Visitors.SaveVisitorInfo;
using Microsoft.EntityFrameworkCore;
using persistent;
using infrastructure.IdentityConfig;
using persistent.Context;
using persistent.Context.MongoContext;
using Web.Endpoint.Hubs;
using Web.Endpoint.Utilities.Filters;
using Web.Endpoint.Utilities.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddRazorPages();
builder.Services.AddIdentityService(configuration);
builder.Services.AddAuthorization();

#region IOC

builder.Services.AddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));
builder.Services.AddTransient<ISaveVisitorInfoService, SaveVisitorInfoService>();
builder.Services.AddScoped<SaveVisitorFilter>();
builder.Services.AddSignalR();
builder.Services.AddScoped<IVisitorOnlineService, VisitorOnlineService>();

#endregion

builder.Services.ConfigureApplicationCookie(option =>
{
    option.ExpireTimeSpan = TimeSpan.FromDays(1);
    option.LoginPath = "/login";
    option.AccessDeniedPath = "/AccessDenied";
});
#region connection string

var connection = configuration.GetConnectionString("AdvancedConnection");
builder.Services.AddDbContext<DatabaseContext>(option => option.UseSqlServer(connection));

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSetVisitorId();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapRazorPages();
app.UseEndpoints(option =>
{
    option.MapHub<OnlineVisitorHub>("/chathub");
});
app.Run();
