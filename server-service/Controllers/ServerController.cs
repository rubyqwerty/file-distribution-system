using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ServerController : ControllerBase
{
    public ServerController(ILogger logger, IStorage storage, VirtualNodeManager virtualNodeManager)
    {
        _logger = logger;
        _storageProvider = storage;
        _virtualNodeManager = virtualNodeManager;
    }

    [HttpPost]
    public IActionResult Create([FromBody] models.Server server)
    {
        try
        {
            _storageProvider.AddServer(server, _virtualNodeManager.CreateVirtualNodes);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem($"Ошибка на сервере: {ex.Message}");
        }
    }

    [HttpDelete("{idServer}")]
    public async Task<IActionResult> DeleteServer(int idServer)
    {
        try
        {
            await _storageProvider.DeleteServer(idServer, _virtualNodeManager.DeleteVirtualNodes);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem($"Ошибка на сервере: {ex.Message}");
        }
    }

    [HttpGet("{idServer}")]
    public IActionResult GetServer(int idServer)
    {
        try
        {
            var server = _storageProvider.GetServer(idServer);
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

    private readonly IStorage _storageProvider;
    private readonly ILogger _logger;
    private readonly VirtualNodeManager _virtualNodeManager;
}