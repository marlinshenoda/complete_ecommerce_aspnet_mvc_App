using eTickets.Core.Entities;
using eTickets.Core.Interfaces;
using eTickets.Data;
using eTickets.Data.Data;
using eTickets.Data.Repositories;
using eTickets.Web.Exetention;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'MyPortolioContext' not found.")));


builder.Services.AddScoped<IActorsRepository, ActorsRepository>();


builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;

}).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
var app = builder.Build();

builder.Services.AddControllersWithViews();
//Services configuration
//services.AddScoped<IProducersService, ProducersService>();
//services.AddScoped<ICinemasService, CinemasService>();
//services.AddScoped<IMoviesService, MoviesService>();
//services.AddScoped<IOrdersService, OrdersService>();
// Seeddata
await app.SeedDataAsync();

//using (var scope = app.Services.CreateScope())
//{
//  //  var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    var services = scope.ServiceProvider;
//    var db = services.GetRequiredService<ApplicationDbContext>();
//    //db.Database.EnsureDeleted();
//    //db.Database.Migrate();

//    try
//    {
//        SeedData.InitAsync(db, services).Wait();
//     //  await SeedData.InitAsync(db);
//    }
//    catch (Exception e)
//    {
//        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
//        logger.LogError(string.Join(" ", e.Message));
//        //throw;
//    }
//}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movies}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
