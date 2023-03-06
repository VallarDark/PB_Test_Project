using PB_WebApi.ErrorHandler;
using PB_WebApi.Utils;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

#region ServicesInjection

builder.Services.AddConfiguredAuthentication(builder.Configuration);
builder.Services.AddConfiguredAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRepositoryResolver();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddControllers();
builder.Services.AddConfiguredCors();
builder.Services.AddScoped<ExceptionMiddleware>();
builder.Services.AddLogging();

builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(60);
    options.ExcludedHosts.Add("example.com");
    options.ExcludedHosts.Add("www.example.com");
});

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
    options.HttpsPort = 5001;
});

#endregion

builder.Logging.AddJsonConsole();

var app = builder.Build();

#region PiplineSetup

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.Run();

#endregion