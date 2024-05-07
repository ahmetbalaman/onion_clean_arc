using OnionArcAndAll.Persistence;
using OnionArcAndAll.Application;
using OnionArcAndAll.Mapper;
using OnionArcAndAll.Application.Exceptions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var env = builder.Environment;

builder.Configuration.AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
    ;


//Adding Persistence Layer
builder.Services.AddPersistence(builder.Configuration);

//Adding Application Layer
builder.Services.AddApplication();

//Adding Custom Mapper for DTOs Mapping
builder.Services.AddCustomMapper();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adding Middleware nd Exception Handling
app.ConfigureExceptionHandlingMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
