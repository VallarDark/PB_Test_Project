using WebApi.Utils;

var builder = WebApplication.CreateBuilder(args);

#region ServicesInjection

builder.Services.AddConfiguredAuthentication(builder.Configuration);
builder.Services.AddConfiguredAuthorization();
builder.Services.AddConfiguredControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRepositoryResolver();
builder.Services.AddRepositories();
builder.Services.AddServices();

#endregion

var app = builder.Build();

#region PiplineSetup

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

app.MapFallbackToController("Fallback", "Account");

#endregion