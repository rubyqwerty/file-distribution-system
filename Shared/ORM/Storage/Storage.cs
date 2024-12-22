
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace storage;

public interface IStorage
{
    /// <summary>
    /// Получить все репликации чанка
    /// </summary>
    /// <param name="idMetadata"></param>
    /// <returns></returns>
    public Task<List<models.Replication>> GetReplications(string chunkHash);
    /// <summary>
    /// Добавить репликацию
    /// </summary>
    /// <param name="replication"></param>
    /// <returns></returns>
    public Task AddReplication(models.Replication replication);
    /// <summary>
    /// Добавить список репликаций
    /// </summary>
    /// <param name="replications"></param>
    /// <returns></returns>
    public Task AddReplications(List<models.Replication> replications);
    /// <summary>
    /// Получить все серверы
    /// </summary>
    /// <returns></returns>
    public Task<List<models.Server>> GetServers();
    /// <summary>
    /// Получить сервер по идентификатору
    /// </summary>
    /// <param name="idServer"></param>
    /// <returns></returns>
    public Task<models.Server> GetServer(int idServer);
    /// <summary>
    /// Обновить сервер
    /// </summary>
    /// <param name="idServer"></param>
    /// <param name="newServer"></param>
    /// <returns></returns>
    public Task UpdateServer(int idServer, models.Server newServer, Action<models.Server> callback);
    /// <summary>
    /// Получить все виртувальные узлы
    /// </summary>
    /// <returns></returns>
    public Task<List<models.VirtualNode>> GetVirtualNodes();
    /// <summary>
    /// Получить виртуальные узлы, принадлежащие серверу
    /// </summary>
    /// <param name="idServer"></param>
    /// <returns></returns>
    public Task<List<models.VirtualNode>> GetVirtualNodes(int idServer);
    /// <summary>
    /// Добавить сервер
    /// </summary>
    /// <param name="server"></param>
    /// <param name="callback">Триггер на добавление</param>
    /// <returns></returns>
    public Task AddServer(models.Server server, Action<models.Server> callback);
    /// <summary>
    /// Добавить виртуальный узел
    /// </summary>
    /// <param name="virtualNode"></param>
    /// <returns></returns>
    public Task AddVirtualNode(models.VirtualNode virtualNode);
    /// <summary>
    /// Удалить виртуальные узлы принадлежащие серверу
    /// </summary>
    /// <param name="idServer"></param>
    /// <returns></returns>
    public Task DeleteVirtualNodes(int idServer);
    /// <summary>
    /// Удалить сервер
    /// </summary>
    /// <param name="idServer"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public Task DeleteServer(int idServer);
    /// <summary>
    /// Получить сервер, которому приндлежит виртуальный узел
    /// </summary>
    /// <param name="virtualNodeHash"></param>
    /// <returns></returns>
    public Task<models.Server> GetServerByVirtualNodeHash(string virtualNodeHash);
    /// <summary>
    /// Добавить метаданные
    /// </summary>
    /// <param name="metadata"></param>
    /// <returns></returns>
    public Task<models.Metadata> AddMetadata(models.Metadata metadata);
    /// <summary>
    /// Добавить чанк
    /// </summary>
    /// <param name="chunk"></param>
    /// <returns></returns>
    public Task<models.Chunk> AddChunk(models.Chunk chunk);
    /// <summary>
    /// Получить чанки, принадлежащие файлу
    /// </summary>
    /// <param name="idMetadata"></param>
    /// <returns></returns>
    public Task<List<models.Chunk>> GetChunkByMetadataId(int idMetadata);
    /// <summary>
    /// Получить метаданные по хэшу чанка
    /// </summary>
    /// <param name="hash"></param>
    /// <returns></returns>
    public Task<models.Metadata> GetMetadataByChunkHash(string hash);
    /// <summary>
    /// Получить метаданные по id
    /// </summary>
    /// <param name="idMetadata"></param>
    /// <returns></returns>
    public Task<models.Metadata> GetMetadataById(int idMetadata);
    /// <summary>
    /// Получить все доступные метаданные
    /// </summary>
    /// <returns></returns>
    public Task<List<models.Metadata>> GetMetadata();
    /// <summary>
    /// Удаление метаданных по id
    /// </summary>
    /// <param name="idMetadata"></param>
    /// <returns></returns>
    public Task DeleteMetadataById(int idMetadata);
}

public class Storage : IStorage
{
    public async Task<List<models.Replication>> GetReplications(string chunkHash)
    {
        ApplicationContext db = new();

        var replications = await (from replication in db.Replications
                                  where replication.HashChunk == chunkHash
                                  select replication).ToListAsync()
                            ?? throw new KeyNotFoundException($"Нет репликаций с chunkHash: {chunkHash}");

        return replications;
    }

    public async Task AddReplication(models.Replication replication)
    {
        ApplicationContext db = new();
        await db.Replications.AddAsync(replication);

        await db.SaveChangesAsync();
    }

    public async Task AddReplications(List<models.Replication> replications)
    {
        ApplicationContext db = new();
        await db.Replications.AddRangeAsync(replications);

        await db.SaveChangesAsync();
    }

    public async Task<List<models.Server>> GetServers()
    {
        ApplicationContext db = new();
        var servers = await db.Servers.ToListAsync();

        return servers;
    }
    public async Task<models.Server> GetServer(int idServer)
    {
        ApplicationContext db = new();
        var serverWithId = await (from server in db.Servers
                                  where server.Id == idServer
                                  select server).FirstOrDefaultAsync() ??
                                  throw new KeyNotFoundException($"Не найдено сервера с id: {idServer}");

        return serverWithId;
    }

    public async Task UpdateServer(int idServer, models.Server newServer, Action<models.Server> callback)
    {
        ApplicationContext db = new();

        var server = await (from srv in db.Servers
                            where srv.Id == idServer
                            select srv).FirstOrDefaultAsync() ??
                      throw new KeyNotFoundException($"Не найдено сервера с id: {idServer}");

        if (newServer.Priority != server.Priority)
        {
            callback(server);
        }
        server.Address = newServer.Address;
        server.Memory = newServer.Memory;
        server.Priority = newServer.Priority;

        await db.SaveChangesAsync();
    }

    public async Task<List<models.VirtualNode>> GetVirtualNodes()
    {
        ApplicationContext db = new();
        var virtualNodes = await db.VirtualNodes.ToListAsync();

        return virtualNodes;
    }

    public async Task<List<models.VirtualNode>> GetVirtualNodes(int idServer)
    {
        ApplicationContext db = new();
        var virtualNodes = await (from vrtNode in db.VirtualNodes
                                  where vrtNode.IdServer == idServer
                                  select vrtNode).ToListAsync() ??
                                  throw new KeyNotFoundException($"Не найдено виртуальных узлов с idServer {idServer}");
        return virtualNodes;
    }

    public async Task AddServer(models.Server server, Action<models.Server> callback)
    {
        ApplicationContext db = new();
        await db.AddAsync(server);

        await db.SaveChangesAsync();

        callback(server);
    }

    public async Task AddVirtualNode(models.VirtualNode virtualNode)
    {
        ApplicationContext db = new();
        await db.AddAsync(virtualNode);

        await db.SaveChangesAsync();
    }

    public async Task DeleteServer(int idServer)
    {
        ApplicationContext db = new();
        var server = await db.Servers.FindAsync(idServer) ??
                    throw new KeyNotFoundException($"Нет сервера с id {idServer}");
        db.Servers.Remove(server);

        await db.SaveChangesAsync();
    }

    public async Task DeleteVirtualNodes(int idServer)
    {
        ApplicationContext db = new();
        var virtualNodes = await (from vrtNodes in db.VirtualNodes
                                  where vrtNodes.IdServer == idServer
                                  select vrtNodes).ToListAsync() ??
                                  throw new KeyNotFoundException($"Не найдено виртуальных узлов с idServer {idServer}");

        db.VirtualNodes.RemoveRange(virtualNodes);

        await db.SaveChangesAsync();
    }

    public async Task<models.Server> GetServerByVirtualNodeHash(string virtualNodeHash)
    {
        ApplicationContext db = new();
        var idServer = await (from vrtNode in db.VirtualNodes
                              where vrtNode.Hash == virtualNodeHash
                              select vrtNode.IdServer).FirstOrDefaultAsync();

        var serverWithId = await (from server in db.Servers
                                  where server.Id == idServer
                                  select server).FirstOrDefaultAsync() ??
                                 throw new KeyNotFoundException($"Не найдено сервера с id: {idServer}");

        return serverWithId;
    }

    public async Task<models.Metadata> AddMetadata(models.Metadata metadata)
    {
        ApplicationContext db = new();

        var addedMeta = await db.Metadata.AddAsync(metadata);

        await db.SaveChangesAsync();

        return addedMeta.Entity;
    }

    public async Task<models.Chunk> AddChunk(models.Chunk chunk)
    {
        ApplicationContext db = new();

        var addedChunk = await db.Chunks.AddAsync(chunk);

        await db.SaveChangesAsync();

        return addedChunk.Entity;
    }

    public async Task<List<models.Chunk>> GetChunkByMetadataId(int idMetadata)
    {
        ApplicationContext db = new();

        var chunks = await (from chunk in db.Chunks
                            where chunk.IdMetadata == idMetadata
                            orderby chunk.Position ascending
                            select chunk).ToListAsync() ??
                        throw new KeyNotFoundException($"Не найдено ни одного чанка для метаданных с id {idMetadata}");

        return chunks;
    }

    public async Task<models.Metadata> GetMetadataByChunkHash(string hash)
    {
        ApplicationContext db = new();

        var chunk = await (from chnk in db.Chunks
                           where chnk.Hash == hash
                           select chnk).FirstOrDefaultAsync() ??
                        throw new KeyNotFoundException($"Не найдено ни одного чанка c hash {hash}");

        var metadata = await (from meta in db.Metadata
                              where meta.Id == chunk.IdMetadata
                              select meta).FirstOrDefaultAsync() ??
                            throw new KeyNotFoundException($"Не найдено ни объекта метаданных с id {chunk.IdMetadata}");

        return metadata;

    }

    public async Task<models.Metadata> GetMetadataById(int idMetadata)
    {
        ApplicationContext db = new();
        var metadata = await (from meta in db.Metadata
                              where meta.Id == idMetadata
                              select meta).FirstOrDefaultAsync() ??
                       throw new KeyNotFoundException($"Не найдены метаданные с id {idMetadata}");

        return metadata;
    }

    public async Task<List<models.Metadata>> GetMetadata()
    {
        ApplicationContext db = new();
        return await db.Metadata.ToListAsync();
    }

    public async Task DeleteMetadataById(int idMetadata)
    {
        ApplicationContext db = new();
        var metadata = await (from meta in db.Metadata
                              where meta.Id == idMetadata
                              select meta).FirstOrDefaultAsync() ??
                       throw new KeyNotFoundException($"Не найдены метаданные с id {idMetadata}");

        db.Metadata.Remove(metadata);

        await db.SaveChangesAsync();
    }

}