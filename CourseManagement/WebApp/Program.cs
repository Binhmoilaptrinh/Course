
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký HttpClient
builder.Services.AddHttpClient();
// Add services to the container.
builder.Services.AddRazorPages();
// Đăng ký HttpClient
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();

    // Redirect root URL ("/") to "/homepage/index"
    endpoints.MapGet("/", async context =>
    {
        context.Response.Redirect("/Homepage/Index");
    });
});


app.UseAuthorization();

app.MapRazorPages();

app.Run();
