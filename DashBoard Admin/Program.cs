using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Souq.Core.Entites.Identity;
using Souq.Infrastructure.Data.DbContexts;
using Stripe;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDbContext<SouqeIdentityDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});
builder.Services.AddIdentity<AppUser, IdentityRole>()
               .AddEntityFrameworkStores<SouqeIdentityDbContext>()
               .AddDefaultTokenProviders();
builder.Services.AddCors(Options =>
Options.AddPolicy("MyPolicy", Options =>
Options.AllowAnyHeader()
.AllowAnyMethod()
.WithOrigins(builder.Configuration["FrontBaseUrl"]!)
));






var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
