using StockApp.Infraestructure.Persistence;
using StockApp.Infraestructure.Shared;
using StockApp.Infraestructure.Identity;
using StockApp.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//Decorator
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddApplicationLayer(builder.Configuration);
builder.Services.AddSharedInfraestructure(builder.Configuration);
builder.Services.AddIdentityInfraestructure(builder.Configuration);
builder.Services.AddHealthChecks();

builder.Services.AddSwaggerExtension();
builder.Services.AddApiVersioningExtension();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExtension();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/health");
app.UseSession();

app.MapControllers();

app.Run();