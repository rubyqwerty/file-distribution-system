var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IHashManager, HashManager>();
builder.Services.AddSingleton<storage.IStorage, storage.Storage>();
builder.Services.AddTransient<VirtualNodeManager>();

var app = builder.Build();


app.UseSwagger(); // Генерация спецификации Swagger
app.UseSwaggerUI(); // Включение UI для тестирования

app.MapControllers();

app.Run();