using WebAppMVC.Utils;

var builder = WebApplication.CreateBuilder(args);

//Dem Container werden Dienste hinzugefügt
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IConfigReader, ConfigReader>();
builder.Services.AddSingleton<ConfigReader>();
builder.Services.AddScoped<ConfigReader>();
builder.Services.AddTransient<ConfigReader>();
var app = builder.Build();


//HTTP Pipeline wird Konfiguriert
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
