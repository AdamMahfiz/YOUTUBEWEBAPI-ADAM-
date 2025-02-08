using YouTubeApiProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add controllers and views
builder.Services.AddControllersWithViews();

// ✅ Register YouTubeApiService with IConfiguration
builder.Services.AddScoped<YouTubeApiService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new YouTubeApiService(configuration);
});

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // ✅ Ensure session middleware is enabled
app.UseAuthorization();

// Set the default route to YouTubeController
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=YouTube}/{action=Index}/{id?}");

app.Run();
