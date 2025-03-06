using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using WebAPI.Extensions;
using Microsoft.AspNetCore.Http.Features;
using WebAPI.Services.Interfaces;
using WebAPI.Services;
using WebAPI.Repositories.Interfaces;
using WebAPI.Repositories;
<<<<<<< HEAD
using Microsoft.AspNetCore.Identity.UI.Services;
using WebAPI.DTOS;

var builder = WebApplication.CreateBuilder(args);
// Thêm dòng này ?? inject IConfiguration
=======
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
>>>>>>> 87b1073cfd82b875b0b5b5b2d6f1d83de6a1a9f6
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Add services to the container.
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
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
<<<<<<< HEAD
builder.Services.AddScoped<IDiscountService, DiscountService>();

=======
//builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICourseLearningService, CourseLearningService>();
builder.Services.AddScoped<ICategoryService, CategoryServiceImpl>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseServiceImpl>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserServiceImpl>();
builder.Services.AddTransient<IChapterRepository, ChapterRepository>();
builder.Services.AddTransient<IChapterService, ChapterServiceImpl>();
builder.Services.AddAutoMapper(typeof(Program));
>>>>>>> 87b1073cfd82b875b0b5b5b2d6f1d83de6a1a9f6
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
app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Resources"
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

