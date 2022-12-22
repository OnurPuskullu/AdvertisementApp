using AdvertisementApp.Business.DependencyResolvers.Microsoft;
using AdvertisementApp.Business.Helpers;
using AdvertisementApp.Business.Interfaces;
using AdvertisementApp.Business.Mappings.AutoMapper;
using AdvertisementApp.Business.Services;
using AdvertisementApp.Business.ValidationRules;
using AdvertisementApp.DataAccess.Contexts;
using AdvertisementApp.DataAccess.UnitOfWork;
using AdvertisementApp.Dtos;
using AdvertisementApp.UI.Mappings.AutoMapper;
using AdvertisementApp.UI.Models;
using AdvertisementApp.UI.ValidationRules;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
{
    opt.Cookie.Name = "Cookie";
    opt.Cookie.HttpOnly = true;
    opt.Cookie.SameSite = SameSiteMode.Strict;
    opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    opt.ExpireTimeSpan = TimeSpan.FromDays(20);
    opt.LoginPath = new PathString("/Account/SignIn");
    opt.LogoutPath = new PathString("/Account/LogOut");
    opt.AccessDeniedPath = new PathString("/Account/AccessDenied");
});
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AdvertisementContext>(opt =>
{
    opt.UseSqlServer("server=LAPTOP-HUG0NL9Q;database=AdvertisementDb;uid=sa;pwd=Test123!;TrustServerCertificate=True;");
});
var profiles = ProfileHelper.GetProfiles();
profiles.Add(new UserCreateModelProfile());
var mapperConfiguration = new MapperConfiguration(opt =>
{
    opt.AddProfiles(profiles);
    opt.AddProfile(new ProvidedServiceProfile());
    opt.AddProfile(new AdvertisementProfile());
    opt.AddProfile(new AppUserProfile());
    opt.AddProfile(new GenderProfile());
    opt.AddProfile(new AppRoleProfile());
    opt.AddProfile(new AdvertisementAppUserProfile());
    opt.AddProfile(new AdvertisementAppUserStatusProfile());
    opt.AddProfile(new MilitaryStatusProfile());
});
var mapper = mapperConfiguration.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<IUow, Uow>();

builder.Services.AddTransient<IValidator<ProvidedServiceCreateDto>, ProvidedServiceCreateDtoValidator>();
builder.Services.AddTransient<IValidator<ProvidedServiceUpdateDto>, ProvidedServiceUpdateDtoValidator>();
builder.Services.AddTransient<IValidator<AdvertisementCreateDto>, AdvertisementCreateDtoValidator>();
builder.Services.AddTransient<IValidator<AdvertisementUpdateDto>, AdvertisementUpdateDtoValidator>();
builder.Services.AddTransient<IValidator<AppUserCreateDto>, AppUserCreateDtoValidator>();
builder.Services.AddTransient<IValidator<AppUserUpdateDto>, AppUserUpdateDtoValidator>();
builder.Services.AddTransient<IValidator<AppUserLoginDto>, AppUserLoginDtoValidator>();
builder.Services.AddTransient<IValidator<GenderCreateDto>, GenderCreateDtoValidator>();
builder.Services.AddTransient<IValidator<GenderUpdateDto>, GenderUpdateDtoValidator>();
builder.Services.AddTransient<IValidator<UserCreateModel>, UserCreateModelValidator>();
builder.Services.AddTransient<IValidator<AdvertisementAppUserCreateDto>, AdvertisementAppUserCreateDtoValidator>();

builder.Services.AddScoped<IProvidedServiceService, ProvidedServiceService>();
builder.Services.AddScoped<IAdvertisementService, AdvertisementService>();
builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<IGenderService, GenderService>();
builder.Services.AddScoped<IAdvertisementAppUserService, AdvertisementAppUserService>();

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

app.MapDefaultControllerRoute();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
