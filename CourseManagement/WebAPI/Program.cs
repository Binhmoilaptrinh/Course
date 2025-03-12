using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.FileProviders;
using System.Text;
using WebAPI.Extensions;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services;
using WebAPI.Services.Interfaces;
using WebAPI.DTOS;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<ICourseLearningService, CourseLearningService>();
builder.Services.AddScoped<ICategoryService, CategoryServiceImpl>();
builder.Services.AddScoped<ICourseService, CourseServiceImpl>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();
builder.Services.AddScoped<IChapterService, ChapterServiceImpl>();

// Đăng ký IFileService với Transient Lifetime
builder.Services.AddTransient<IFileService, FileService>();

// 🛠 Cấu hình CORS
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

// 🛠 Cấu hình Static Files (Uploads)
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Resources"
});

// 🛠 Kích hoạt Swagger
app.UseSwagger();
app.UseSwaggerUI();

// 🛠 Thêm Authentication và Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
