using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ServerController : ControllerBase
{
    public ServerController(ILogger<ServerController> logger, storage.IStorage storage, VirtualNodeManager virtualNodeManager)
    {
        _logger = logger;
        _storageProvider = storage;
        _virtualNodeManager = virtualNodeManager;
    }

    /// <summary>
    /// Создать новый сервер
    /// </summary>
    /// <param name="server"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Create([FromBody] models.Server server)
    {
        try
        {
            _logger.LogInformation("Пришел запрос на создание сервера");

            Task.Run(() => _storageProvider.AddServer(server, _virtualNodeManager.CreateVirtualNodes));
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem($"Ошибка на сервере: {ex.Message}");
        }
    }

    /// <summary>
    /// Удалить сервер
    /// </summary>
    /// <param name="idServer"></param>
    /// <returns></returns>
    [HttpDelete("{idServer}")]
    public async Task<IActionResult> DeleteServer(int idServer)
    {
        _logger.LogInformation($"Пришел запрос на удаление сервера с id {idServer}");
        try
        {
            await _storageProvider.DeleteServer(idServer);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem($"Ошибка на сервере: {ex.Message}");
        }
    }

    /// <summary>
    /// Получить сервер
    /// </summary>
    /// <param name="idServer"></param>
    /// <returns></returns>
    [HttpGet("{idServer}")]
    public async Task<IActionResult> GetServer(int idServer)
    {
        _logger.LogInformation($"Пришел запрос на получение сервера с id {idServer}");
        try
        {
            var server = await _storageProvider.GetServer(idServer);
            if (server == null)
            {
                return NotFound();
            }
            return Ok(server);
        }
        catch (Exception ex)
        {
            return Problem($"Ошибка на сервере: {ex.Message}");
        }
    }

    /// <summary>
    /// Обновить сервер
    /// </summary>
    /// <param name="idServer"></param>
    /// <param name="newServer"></param>
    /// <returns></returns>
    [HttpPut("{idServer}")]
    public async Task<IActionResult> Update(int idServer, [FromBody] models.Server newServer)
    {
        _logger.LogInformation($"Пришел запрос на обновление сервера с id {idServer}");
        try
        {
            await _storageProvider.UpdateServer(idServer, newServer, _virtualNodeManager.RecreateVirtualNodes);

            return Ok();
        }
        catch (Exception ex)
        {
            return Problem($"Ошибка на сервере: {ex.Message}");
        }
    }

    /// <summary>
    /// Получить серверы
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult> GetServers()
    {
        _logger.LogInformation("Пришел запрос на получение серверов");
        try
        {
            var servers = await _storageProvider.GetServers();

            return Ok(servers);
        }
        catch (Exception ex)
        {
            return Problem($"Ошибка на сервере: {ex.Message}");
        }
    }

    private readonly storage.IStorage _storageProvider;
    private readonly ILogger<ServerController> _logger;
    private readonly VirtualNodeManager _virtualNodeManager;
}