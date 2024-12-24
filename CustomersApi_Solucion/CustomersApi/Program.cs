using CustomersApi.CasosDeUso;
using CustomersApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Agregar política CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
        {
            // Permite localhost con cualquier puerto
            return new Uri(origin).Host == "localhost";
        })
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Agregar otros servicios
builder.Services.AddScoped<IUpdateCustomerUseCase, UpdateCustomerUseCase>();
builder.Services.AddControllers();
builder.Services.AddRouting(routing => routing.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API",
        Version = "v1",
        Description = "Documentación para la API."
    });
});
builder.Services.AddDbContext<CustomerDatabaseContext>(mySqlBuilder =>
{
    mySqlBuilder.UseMySql(builder.Configuration.GetConnectionString("Connection1"),
    new MySqlServerVersion(new Version(10, 4, 32)) // Especifica la versión del servidor
    );
});

var app = builder.Build();

// Configuración del pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// Agregar middleware de CORS
app.UseCors("AllowFrontend");

app.UseAuthorization();
app.MapControllers();
app.Run();
