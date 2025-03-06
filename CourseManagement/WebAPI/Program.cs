using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using WebAPI.Extensions;
using Microsoft.AspNetCore.Http.Features;
using WebAPI.Services.Interfaces;
using WebAPI.Services;
using WebAPI.Repositories.Interfaces;
using WebAPI.Repositories;
using Microsoft.AspNetCore.Identity.UI.Services;
using WebAPI.DTOS;

var builder = WebApplication.CreateBuilder(args);
// Thêm dòng này ?? inject IConfiguration
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Add services to the container.
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login"; // Adjust this path as needed
});

// 🛠 Đăng ký AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// 🛠 Đăng ký Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

// 🛠 Đăng ký Service
builder.Services.Configure<SendEmail>(builder.Configuration.GetSection("SendEmail"));
builder.Services.AddScoped<ISendEmail, SendEmailService>(); // Sửa lại đúng Interface
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin() // Allow any origin
               .AllowAnyMethod() // Allow any HTTP method
               .AllowAnyHeader(); // Allow any header
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
