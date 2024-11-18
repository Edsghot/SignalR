using serverApiSignal; // A// Aseg�rate de que NotificationHub est� en el espacio de nombres correcto.
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configura SignalR con Azure SignalR
builder.Services.AddSignalR().AddAzureSignalR(options =>
{
    // Aseg�rate de que esta clave existe en tu archivo appsettings.json
    options.ConnectionString = builder.Configuration["AzureSignalRConnectionString"];
});

// Configura Swagger para la documentaci�n de API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuraci�n de Kestrel para habilitar HTTPS
builder.WebHost.ConfigureKestrel(option =>
{
    option.Listen(IPAddress.Any, 7192, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

// Configura CORS para permitir todos los or�genes
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

// Habilitar autorizaci�n (si es necesario)
app.UseAuthorization();

// Mapear el Hub de SignalR
app.MapHub<NotificationHub>("/notificationHub");

// Mapear controladores para las APIs
app.MapControllers();

// Ejecutar la aplicaci�n
app.Run();
