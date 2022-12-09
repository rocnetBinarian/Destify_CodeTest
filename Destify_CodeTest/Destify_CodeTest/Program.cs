using Serilog;
using Destify_CodeTest;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Destify_CodeTest.Models.Entities;
using Destify_CodeTest.Models.Services;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Destify_CodeTest.Models;

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

builder.AddAuthentication(e => {
    e.AddScheme<DefaultAuthHandler>("DefaultHandler", "Default Handler");
    e.AddScheme<APIAuthHandler>("APIAuthHandler", "Main Authenticator");
});


builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo{
        Title = "Destify Code Test API Documentation",
        Description = "Contains documentation information for the API endpoints available in the Destify Code Test API.",
        Contact = new OpenApiContact {
            Name = "Russell Uhl"
        },
        Version = "v1"
    });
    c.EnableAnnotations();
    //var xmlFile = Path.ChangeExtension(typeof(Program).Assembly.Location, ".xml");
    //c.IncludeXmlComments(xmlFile);
    //c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>(); // needed?
});
builder.Services.AddMvcCore()
    .AddApiExplorer();

var CONNSTRING = builder.Configuration.GetConnectionString("MovieConnString");
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddSqlite<MovieContext>(CONNSTRING);

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
app.UseSwagger(c => {
    c.SerializeAsV2 = false;
});
app.UseSwaggerUI(c =>  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Destify Code Test API");
});

#region Configure Logging
Globals.LogConf = new LoggerConfiguration()
    .ReadFrom.Configuration(app.Configuration);
Log.Logger = Globals.LogConf.CreateLogger();
#endregion

Log.Information("Logging Initialized");


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<MovieContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
    dbContext.SaveChanges();


    var movieService = scope.ServiceProvider.GetService<IMovieService>();
    movieService.Create(new Movie()
    {
        Name = "First Movie",
        Actors = new List<Actor>()
        {
            new Actor() {Name = "Al Alington"},
            new Actor() {Name = "Bob Bobbington"}
        }
    });
}


app.Run();
