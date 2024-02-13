namespace TodoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Konfiguriere Logging
            builder.Logging.ClearProviders(); // Entfernt alle vorherigen Logging-Provider
            builder.Logging.AddConsole(); // Fügt den Konsolen-Logging-Provider hinzu

            // Fügt CORS-Dienste hinzu und konfiguriert eine benannte CORS-Richtlinie
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5173") // Erlaubt Zugriffe von dieser Origin
                               .AllowAnyHeader() // Erlaubt alle Header
                               .AllowAnyMethod(); // Erlaubt alle Methoden
                    });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowSpecificOrigin"); // Verwendet die CORS-Richtlinie

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
