using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services;
using WebAPI.Services.Interfaces;
using WebAPI.Utilities;
using WebAPI.DTOS;
using Microsoft.AspNetCore.Http.Features;
using PdfSharp.Fonts;
using WebAPI.Tasks;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ---------------------- OData Configuration ----------------------
var modelBuilderOData = new ODataConventionModelBuilder();
modelBuilderOData.EntitySet<LessonResponseAdmin>("Lesson");
modelBuilderOData.EntitySet<Chapter>("Chapter");
modelBuilderOData.EntitySet<CourseAdminResponseDto>("Course");
modelBuilderOData.EntitySet<UserReponseDto>("User");
modelBuilderOData.EntitySet<DiscountResponseDto>("Discount");

builder.Services.AddControllers().AddOData(options =>
    options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null)
           .AddRouteComponents("odata", modelBuilderOData.GetEdmModel())
);

// ---------------------- IConfiguration ----------------------
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// ---------------------- Database & Other Configurations ----------------------
builder.Services.ConfigureSqlContext(builder.Configuration);

// ---------------------- BlobContainerClient Registration ----------------------
string? storageAccount = builder.Configuration["AzureBlobStorage:AccountName"];
string? storageKey = builder.Configuration["AzureBlobStorage:AccountKey"];
string? containerName = builder.Configuration["AzureBlobStorage:ContainerName"];
string? blobUri = builder.Configuration["AzureBlobStorage:BlobServiceUrl"];

if (string.IsNullOrEmpty(storageAccount) || string.IsNullOrEmpty(storageKey) || string.IsNullOrEmpty(containerName) || string.IsNullOrEmpty(blobUri))
{
    throw new InvalidOperationException("Azure Blob Storage configuration is missing or incorrect.");
}

builder.Services.AddSingleton(new BlobContainerClient(
    new Uri($"{blobUri.TrimEnd('/')}/{containerName}"),
    new StorageSharedKeyCredential(storageAccount, storageKey)));

// ---------------------- Authentication with JWT ----------------------
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["secretKey"] ?? "");

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

// ---------------------- AutoMapper ----------------------
builder.Services.AddAutoMapper(typeof(Program));

// ---------------------- Repository Registrations ----------------------
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
builder.Services.AddHostedService<EnrollmentStatusUpdater>();

builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();

// ---------------------- Service Registrations ----------------------
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
builder.Services.AddScoped<IEnrollmentAdminService, EnrollmentAdminService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IFinanceService, FinanceService>();

builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<ILessonService, LessonServiceImpl>();
builder.Services.AddTransient<IAnswerService, AnswerServiceImpl>();
builder.Services.AddTransient<IQuestionService, QuestionServiceImpl>();

// ---------------------- CORS Configuration ----------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ---------------------- Swagger Configuration ----------------------
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

// ---------------------- Build and Configure the App ----------------------
var app = builder.Build();

app.UseHttpsRedirection();
app.UseSwagger();
app.UseCors("AllowAllOrigins");
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
