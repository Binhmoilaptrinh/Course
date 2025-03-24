using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using WebAPI.Models;
using WebAPI.Utilities;
using WebAPI.DTOS.response;
using PdfSharp.Charting;

using System.Text;
using WebAPI.Extensions;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services;
using WebAPI.Services.Interfaces;
using WebAPI.DTOS;
using Microsoft.AspNetCore.Http.Features;
using PdfSharp.Fonts;

var builder = WebApplication.CreateBuilder(args);
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<LessonResponseAdmin>("Lesson");
modelBuilder.EntitySet<Chapter>("Chapter");
modelBuilder.EntitySet<CourseAdminResponseDto>("Course");
modelBuilder.EntitySet<UserReponseDto>("User");
builder.Services.AddControllers().AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null)
    .AddRouteComponents(
    "odata",
        modelBuilder.GetEdmModel())
);
// 🛠 Inject IConfiguration
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// 🛠 Cấu hình Database
builder.Services.ConfigureSqlContext(builder.Configuration);

// 🛠 Cấu hình Authentication với JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["secretKey"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["validIssuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["validAudience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// 🛠 Đăng ký AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// 🛠 Đăng ký Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserPermissionRepository, UserPermissionRepository>();
builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

// 🛠 Đăng ký Service
builder.Services.Configure<SendEmail>(builder.Configuration.GetSection("SendEmail"));
builder.Services.AddScoped<ISendEmail, SendEmailService>();
builder.Services.AddScoped<ICustomAuthorizationService, CustomAuthorizationService>();
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
builder.Services.AddScoped<PaymentHelper>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ICourseLearningService, CourseLearningService>();
builder.Services.AddScoped<ICategoryService, CategoryServiceImpl>();
builder.Services.AddScoped<ICourseService, CourseServiceImpl>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();
builder.Services.AddScoped<IChapterService, ChapterServiceImpl>();
builder.Services.AddScoped<ICourseClientService, CourseClientService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
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


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 🛠 Thêm Controllers
builder.Services.AddControllers();

// 🛠 Cấu hình Swagger hỗ trợ JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Nhập 'Bearer <token>'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseCors("AllowAllOrigins");

app.UseSwaggerUI();

// 🛠 Thêm Authentication và Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
