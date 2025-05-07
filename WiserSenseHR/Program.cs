using Microsoft.EntityFrameworkCore;
using FluentValidation;
using WiserSenseHR.Data.Entities;
using WiserSenseHR.Validator.FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Login/Index"; 
                options.LogoutPath = "/Login/Logout"; 
                options.ExpireTimeSpan = TimeSpan.FromDays(1); 
                options.SlidingExpiration = true; 
                options.Cookie.Name = "UserSession";  
                options.Cookie.HttpOnly = true; 
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Strict; 
            });
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IValidator<User>, UserValidator>();
builder.Services.AddScoped<IValidator<Meeting>, MeetingValidator>();
builder.Services.AddScoped<IValidator<Announcement>, AnnouncementValidator>();
builder.Services.AddScoped<IValidator<AssignedItem>, AssignedItemValidator>();
builder.Services.AddScoped<IValidator<UserInfo>, UserInfoValidator>();
builder.Services.AddScoped<IValidator<FoodList>, FoodListValidator>();
builder.Services.AddScoped<IValidator<Rule>, RuleValidator>();

builder.Services.AddDbContext<WiserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WiserHR")));

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

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
