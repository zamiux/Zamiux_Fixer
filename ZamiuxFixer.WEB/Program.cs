using Microsoft.EntityFrameworkCore;
using ZamiuxFixer.DataLayer;
using ZamiuxFixer.DataLayer.Context;
using ZamiuxFixer.DataLayer.Contract;
using ZamiuxFixer.DataLayer.Repositories;

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
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
