using WebApi.Utils;

var builder = WebApplication.CreateBuilder(args);

#region ServicesInjection

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRepositoryResolver();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddJwt(builder.Configuration);

#endregion

var app = builder.Build();

#region PiplineSetup

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion