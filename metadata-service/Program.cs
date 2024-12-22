var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAnyOrigin", builder =>
            builder.AllowAnyOrigin()  // Разрешить запросы с любых источников
                   .AllowAnyMethod()  // Разрешить любые HTTP-методы (GET, POST, и т.д.)
                   .AllowAnyHeader());  // Разрешить любые заголовки
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IFileStorage, FileSaver>();
builder.Services.AddSingleton<storage.IStorage, storage.Storage>();

var app = builder.Build();
app.UseCors("AllowAnyOrigin");

app.UseSwagger(); // Генерация спецификации Swagger
app.UseSwaggerUI(); // Включение UI для тестирования

app.MapControllers();

app.Run();