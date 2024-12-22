using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public interface IFileStorage
{
    public Task SendChunkToStorage(MemoryStream chunk, string name, string address);
    public Task<MemoryStream> GetChunkFromStorage(string address, string name);
}

public class FileSaver : IFileStorage
{
    private readonly string _outputDirectory;
    private readonly ILogger<FileSaver> _logger;

    public FileSaver(ILogger<FileSaver> logger)
    {
        _outputDirectory = Environment.GetEnvironmentVariable("FILE_STORAGE") ?? "/tmp/file_storage";
        _logger = logger;

        if (!Directory.Exists(_outputDirectory))
        {
            Directory.CreateDirectory(_outputDirectory);
        }
    }

    public async Task SendChunkToStorage(MemoryStream chunk, string name, string address)
    {
        var serverDirectory = _outputDirectory + $"/{address}";

        _logger.LogInformation(serverDirectory);

        if (!Directory.Exists(serverDirectory))
        {
            Directory.CreateDirectory(serverDirectory);
        }

        var partFileName = serverDirectory + $"/{name}";

        using (var outputStream = new FileStream(partFileName, FileMode.Create, FileAccess.Write))
        {
            chunk.Seek(0, SeekOrigin.Begin);
            await chunk.CopyToAsync(outputStream);
        }

        _logger.LogInformation($"Записан чанк {name} --> {partFileName}");
    }

    public async Task<MemoryStream> GetChunkFromStorage(string address, string name)
    {
        var serverDirectory = _outputDirectory + $"/{address}";

        var partFileName = serverDirectory + $"/{name}";

        var file = await File.ReadAllBytesAsync(partFileName);

        _logger.LogInformation($"Прочитан чанк {name} из {partFileName}");

        return new MemoryStream(file);


    }
}
