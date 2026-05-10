using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Project_HafizA.Business;
using Project_HafizA.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAyetService, AyetService>();
builder.Services.AddScoped<ILiderlikService, LiderlikService>();
builder.Services.AddScoped<ISesService>(provider =>
{
    var env = provider.GetRequiredService<IWebHostEnvironment>();
    var modelPath = Path.Combine(env.WebRootPath, "whisper", "ggml-small.bin");
    return new SesService(modelPath);
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.WebHost.UseUrls("http://0.0.0.0:5000");

var app = builder.Build();

app.UseRouting();
app.UseStaticFiles();
app.MapDefaultControllerRoute();
app.Run();