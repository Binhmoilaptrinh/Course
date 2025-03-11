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
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using WebAPI.Models;
using WebAPI.Utilities;


var builder = WebApplication.CreateBuilder(args);
//odata
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Lesson>("Lesson");
modelBuilder.EntitySet<Chapter>("Chapter");
modelBuilder.EntitySet<Course>("Course");
builder.Services.AddControllers().AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null)
    .AddRouteComponents(
    "odata",
        modelBuilder.GetEdmModel())
);

// Inject IConfiguration
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
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IChapterRepository, ChapterRepository>();

// 🛠 Đăng ký Service
builder.Services.Configure<SendEmail>(builder.Configuration.GetSection("SendEmail"));
builder.Services.AddScoped<ISendEmail, SendEmailService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
builder.Services.AddScoped<PaymentHelper>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ICourseLearningService, CourseLearningService>();
builder.Services.AddScoped<ICategoryService, CategoryServiceImpl>();
builder.Services.AddScoped<ICourseService, CourseServiceImpl>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();
builder.Services.AddScoped<IChapterService, ChapterServiceImpl>();
builder.Services.AddScoped<ICourseClientService, CourseClientService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
// Đăng ký IFileService với Transient Lifetime
builder.Services.AddTransient<IFileService, FileService>();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserServiceImpl>();
builder.Services.AddTransient<IChapterRepository, ChapterRepository>();
builder.Services.AddTransient<IChapterService, ChapterServiceImpl>();
builder.Services.AddTransient<ILessonRepository, LessonRepository>();
builder.Services.AddTransient<ILessonService, LessonServiceImpl>();

builder.Services.AddTransient<IAnswerRepository, AnswerRepository>();
builder.Services.AddTransient<IAnswerService, AnswerServiceImpl>();

builder.Services.AddTransient<IQuestionRepository, QuestionRepository>();
builder.Services.AddTransient<IQuestionService, QuestionServiceImpl>();

builder.Services.AddAutoMapper(typeof(Program));


// Cấu hình CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

// Cấu hình Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
