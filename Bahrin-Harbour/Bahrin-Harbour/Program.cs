using Bahrin.Harbour.Data.ClientDA;
using Bahrin.Harbour.Data.DataContext;
using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Data.EMailDA;
using Bahrin.Harbour.Data.LocationDA;
using Bahrin.Harbour.Data.LoyalityCardDA;
using Bahrin.Harbour.Data.OutletDA;
using Bahrin.Harbour.Data.VisitHistoryDA;
using Bahrin.Harbour.Data.VisitHistoryService;
using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Model.EmailModel;
using Bahrin.Harbour.Service.AccoutService;
using Bahrin.Harbour.Service.AppUserService;
using Bahrin.Harbour.Service.ClientService;
using Bahrin.Harbour.Service.DashboardService;
using Bahrin.Harbour.Service.EmailService;
using Bahrin.Harbour.Service.LoyalityCard;
using Bahrin.Harbour.Service.QRCodeService;
using Bahrin.Harbour.Service.Reportservices;
using Bahrin.Harbour.Service.UserAccountService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System.Text;
using static Bahrin.Harbour.Model.EmailModel.EmailModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation(); ;

builder.Services.AddRazorPages();

builder.Services.AddDbContext<BahrinHarbourContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Bahrin.Harbour.Data")));

builder.Services.Configure<SMTPConfigModel>(builder.Configuration.GetSection("SMTPConfig"));

var sessionSettings = builder.Configuration.GetSection("Session");
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.Parse(sessionSettings.GetValue<string>("IdleTimeout"));
    options.Cookie.HttpOnly = sessionSettings.GetValue<bool>("CookieHttpOnly");
    options.Cookie.IsEssential = sessionSettings.GetValue<bool>("CookieIsEssential");
});

var authSettings = builder.Configuration.GetSection("Authentication:Cookie");
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.Name = authSettings.GetValue<string>("Name");
        options.LoginPath = authSettings.GetValue<string>("LoginPath");
        options.LogoutPath = authSettings.GetValue<string>("LogoutPath");
        options.ExpireTimeSpan = TimeSpan.Parse(authSettings.GetValue<string>("ExpireTimeSpan"));
        options.SlidingExpiration = authSettings.GetValue<bool>("SlidingExpiration");
    });


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "shubham.m@mishainfotech.com", 
        ValidAudience = "rashi.k@mishainfotech.com",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345678901234567890123456789012"))
    };
});


////
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<BahrinHarbourContext>().AddDefaultTokenProviders();
//////
///Dipendency Injection
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IUserAccountService, UserAccountService>();
builder.Services.AddTransient<IClientDA, ClientDA>();
builder.Services.AddTransient<IClientService, ClientService>();
builder.Services.AddTransient<IAppUserService, AppUserService>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IQRCodeService, QRCodeService>();
builder.Services.AddTransient<IOutletService, OutletService>();
builder.Services.AddTransient<IOutletDA, OutletDA>();
builder.Services.AddTransient<ILoyalityCardDA, LoyalityCardDA>();
builder.Services.AddTransient<ILoyalityCardService, LoyalityCardService>();
builder.Services.AddTransient<IDashboardService, DashboardService>();
builder.Services.AddTransient<IVisitHistoryService, VisitHistoryService>();
builder.Services.AddTransient<IVisitHistoryDA, VisitHistoryDA>();
builder.Services.AddTransient<IEMailDA, EMailDA>();
builder.Services.AddTransient<ILocationService, LocationService>();
builder.Services.AddTransient<ILocationDA, LocationDA>();
builder.Services.AddTransient<IClientReports, ReportServicesdata>();

////
///



/////
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

// Set EPPlus license context globally
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BahrinHarbourContext>();
   // dbContext.Database.Migrate();
    var services = scope.ServiceProvider;
    await DataSeed.Initialize(services);
}


app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        context.Request.Path = "/";
        context.Response.Redirect("/Administration/Account/Signin");
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();
/*app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
});*/
app.UseRouting();

app.UseSession();

app.MapDefaultControllerRoute();

app.UseAuthorization();

/*app.MapControllerRoute(
    name: "default",
    pattern: "{area=administration}/{controller=Account}/{action=SignIn}/{id?}");*/

app.MapControllerRoute(
     name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.Run();
