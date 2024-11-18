using serverApiSignal; // A// Asegúrate de que NotificationHub está en el espacio de nombres correcto.
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configura SignalR con Azure SignalR
builder.Services.AddSignalR().AddAzureSignalR(options =>
{
    // Asegúrate de que esta clave existe en tu archivo appsettings.json
    options.ConnectionString = builder.Configuration["AzureSignalRConnectionString"];
});

// Configura Swagger para la documentación de API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de Kestrel para habilitar HTTPS
builder.WebHost.ConfigureKestrel(option =>
{
    option.Listen(IPAddress.Any, 7192, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

// Configura CORS para permitir todos los orígenes
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Habilitar Swagger solo en entornos de desarrollo
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilitar HTTPS redirection
app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowAll");

// Habilitar autorización (si es necesario)
app.UseAuthorization();

// Mapear el Hub de SignalR
app.MapHub<NotificationHub>("/notificationHub");

// Mapear controladores para las APIs
app.MapControllers();

// Ejecutar la aplicación
app.Run();
