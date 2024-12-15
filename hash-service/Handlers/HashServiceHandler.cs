using hash_service;
using Microsoft.Extensions.Logging;
using Thrift.Protocol;
using Thrift.Server;


public class HashServiceHandler : HashService.IAsync
{
    public HashServiceHandler(ILogger<HashServiceHandler> logger)
    {
        _logger = logger;
        _logger.LogInformation("Создан обработчик запросов хеш");
    }

    public async Task<string> GetHash(HashParams hashParams, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Получен запрос на получение хеша");

            var hash = await Task.Run(() => Hasher.ComputeHash(hashParams.Data,
                                                                hashParams.Algorithm,
                                                                hashParams.NumberIteration,
                                                                hashParams.OutputFormat));

            return hash;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(message: $"Исключение: {ex}");
            throw;
        }
    }
    private readonly ILogger<HashServiceHandler> _logger;
}