using Serilog;
using TEMPLATE;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddLogging(c =>
{
    c.AddSerilog();
});

#if DEBUG
Globals.CompilationEnvironment = "Debug";
#else
Globals.CompilationEnvironment = "Production";
#endif

builder.Configuration
    .AddJsonFile($"appsettings.{Globals.CompilationEnvironment}.json", optional: true);

var app = builder.Build();

Globals.Config = app.Configuration.GetSection("ConfigSettings").Get<ConfigSettings>();

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

app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();

#region Configure Logging
Globals.LogConf = new LoggerConfiguration()
    .ReadFrom.Configuration(app.Configuration);
Log.Logger = Globals.LogConf.CreateLogger();
#endregion

Log.Information("Logging Initialized");

app.Run();
