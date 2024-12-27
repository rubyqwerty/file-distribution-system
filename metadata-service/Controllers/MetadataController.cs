using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using models;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    public FileController(ILogger<FileController> logger, storage.IStorage storage, IFileStorage fileStorage)
    {
        _logger = logger;
        _storageProvider = storage;
        _fileStorage = fileStorage;
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

        var addedFile = _storageProvider.AddMetadata(new models.Metadata()
        {
            Name = fileName,
            Size = (int)fileSize,
            CreationDate = DateTime.UtcNow,
            ModificationDate = DateTime.UtcNow
        }).Result;

        try
        {
            StoredChunk storedChunks = new() { Chunks = await FileSplitter.SplitStreamAsync(file, (int)fileSize / 5) };

            var counter = 0;
            foreach (var chunk in storedChunks.Chunks)
            {
                var hash = new HashManager().GetHash(new hash_service.HashParams()
                {
                    Data = addedFile.Id.ToString() + addedFile.Name + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
                    NumberIteration = 2,
                    Algorithm = hash_service.Algorithms.SHA256,
                    OutputFormat = hash_service.Format.HEX
                }).Result;

                storedChunks.Hashes.Add(hash);

                await _storageProvider.AddChunk(new models.Chunk()
                {
                    IdMetadata = addedFile.Id,
                    Hash = hash,
                    Position = counter++,
                    Size = (int)chunk.Length
                });
            }

            new DistributionManager().MakeReplication(addedFile.Id).Wait();


            // Распределение файлов
            for (int i = 0; i < storedChunks.Chunks.Count; ++i)
            {
                var replications = _storageProvider.GetReplications(storedChunks.Hashes[i]).Result;

                foreach (var replication in replications)
                {
                    var server = _storageProvider.GetServer(replication.IdServer).Result;

                    _fileStorage.SendChunkToStorage(storedChunks.Chunks[i], storedChunks.Hashes[i], server.Address).Wait();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Чанки не были размещены, ошибка {ex.Message}");
        }


        return Ok(addedFile);
    }

    /// <summary>
    /// Получить файл
    /// </summary>
    /// <param name="idMetadata"></param>
    /// <returns></returns>
    [HttpGet("{idMetadata}")]
    public async Task<ActionResult> GetFile(int idMetadata)
    {
        try
        {
            var chunks = await _storageProvider.GetChunkByMetadataId(idMetadata);

            List<Task<MemoryStream>> chunksInFile = [];

            foreach (var chunk in chunks)
            {
                var replications = await _storageProvider.GetReplications(chunk.Hash);

                bool isLoadedChunk = false;

                foreach (var replication in replications)
                {
                    var address = _storageProvider.GetServer(replication.IdServer).Result.Address;
                    try
                    {
                        chunksInFile.Add(_fileStorage.GetChunkFromStorage(address, chunk.Hash));
                        isLoadedChunk = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"Не получилось считать чанк: {replication.HashChunk} из {address} {ex.Message}");
                    }
                }

                if (isLoadedChunk == false)
                {
                    return NotFound("Не удалось восстановить файл. Был потерен чанк");
                }
            }

            var metadata = await _storageProvider.GetMetadataById(idMetadata);

            var sourceFile = FileMerger.MergeFilePartsToFormFile(chunksInFile, metadata.Name);


            return File(sourceFile, "application/octet-stream", metadata.Name);

        }
        catch (Exception ex)
        {
            return Problem($"Ошибка на сервере: {ex.Message}");
        }
    }

    /// <summary>
    /// Получить все доступнве метаданые
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult GetAllMetadata()
    {
        try
        {
            var metadata = _storageProvider.GetMetadata().Result;

            return Ok(metadata);
        }
        catch (Exception ex)
        {
            return Problem($"Ошибка на сервере: {ex.Message}");
        }
    }

    /// <summary>
    /// Удалить файл
    /// </summary>
    /// <param name="idMetadata"></param>
    /// <returns></returns>
    [HttpDelete("{idMetadata}")]
    public ActionResult DeleteMetadata(int idMetadata)
    {
        try
        {
            _storageProvider.DeleteMetadataById(idMetadata);

            return Ok();
        }
        catch (Exception ex)
        {
            return Problem($"Ошибка на сервере: {ex.Message}");
        }
    }

    private readonly IFileStorage _fileStorage;
    private readonly storage.IStorage _storageProvider;
    private readonly ILogger<FileController> _logger;
}