using Microsoft.AspNetCore.Mvc;
using models;

[ApiController]
[Route("api/[controller]")]
public class MetadataController : ControllerBase
{
    public MetadataController(ILogger<MetadataController> logger, storage.IStorage storage)
    {
        _logger = logger;
        _storageProvider = storage;
        _logger.LogInformation("Контроллер обработки серверов запущен");
    }

    /// <summary>
    /// Загрузить файл на сервер
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл не выбран для загрузки.");


        var fileName = file.FileName;
        var fileSize = file.Length;

        var addedFile = await _storageProvider.AddMetadata(new models.Metadata()
        {
            Name = fileName,
            Size = (int)fileSize,
            CreationDate = DateTime.UtcNow,
            ModificationDate = DateTime.UtcNow
        });

        var chunks = await FileSplitter.SplitStreamAsync(file, 32);


        List<models.Chunk> addedChunks = [];

        HashManager manager = new();

        List<distribution_service.Chunk> replicationRequest = [];
        var counter = 0;
        foreach (var chunk in chunks)
        {
            var hash = await _hashManager.GetHash(new hash_service.HashParams()
            {
                Data = addedFile.Id.ToString() + addedFile.Name + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
                NumberIteration = 2,
                Algorithm = hash_service.Algorithms.SHA256,
                OutputFormat = hash_service.Format.HEX
            });

            addedChunks.Add(await _storageProvider.AddChunk(new models.Chunk()
            {
                IdMetadata = addedFile.Id,
                Hash = hash,
                Position = counter++,
                Size = (int)chunk.Length
            }));

            replicationRequest.Add(new distribution_service.Chunk()
            {
                ChunkHash = addedChunks.Last().Hash,
                IdMetadata = addedChunks.Last().IdMetadata
            });
        }






        return Ok(addedFile);
    }

    private readonly IHashManager _hashManager;
    private readonly storage.IStorage _storageProvider;
    private readonly ILogger<MetadataController> _logger;
}