using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using WebAPI.Extensions;
using Microsoft.AspNetCore.Http.Features;
using WebAPI.Repositories.Interfaces;
using WebAPI.Repositories;
using WebAPI.Services.Interfaces;
using WebAPI.Services;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login"; // Adjust this path as needed
});
//builder.Services.Configure<SendEmail>(builder.Configuration.GetSection("SendEmail"));
//builder.Services.AddScoped<ISendEmail, SendEmailServices>();
//builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryServiceImpl>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseServiceImpl>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(Program));
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
