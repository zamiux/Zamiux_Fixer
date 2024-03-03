using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ZamiuxFixer.Application.SendEmail;
using ZamiuxFixer.DataLayer;
using ZamiuxFixer.DataLayer.Context;
using ZamiuxFixer.DataLayer.Contract;
using ZamiuxFixer.DataLayer.Repositories;
using ZamiuxFixer.IOC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Context Connection
builder.Services.AddDbContext<ZamiuxFixerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ZamiuxFixerConnection"));
});
#endregion

#region Add Service Repository From Data Layer
//builder.Services.AddScoped<IRoleRepository, RoleRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IMailSender, SendEmail>();
builder.Services.RegisterServices();

#endregion

#region Authentication Type : Cookie Browser

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
    options.AccessDeniedPath = "/Notaccess";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

#endregion

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

#region Authentication Middleware
app.UseAuthentication();
#endregion

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    #region User Panel Area
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    #endregion

    #region Admin Area
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    #endregion

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});




app.Run();
