using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;

public static class FileMerger
{
    /// <summary>
    /// Склеивает части файла и возвращает объединенный файл в виде IFormFile.
    /// </summary>
    /// <param name="fileParts">Список частей файла в виде потоков (MemoryStream).</param>
    /// <param name="fileName">Имя объединенного файла.</param>
    /// <returns>IFormFile, представляющий объединенный файл.</returns>
    public static MemoryStream MergeFilePartsToFormFile(List<Task<MemoryStream>> fileParts, string fileName)
    {
        if (fileParts == null || fileParts.Count == 0)
            throw new ArgumentException("Список частей файла пуст.", nameof(fileParts));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Имя файла не может быть пустым.", nameof(fileName));

        // Создаем общий MemoryStream для объединенного файла
        var mergedStream = new MemoryStream();

        foreach (var part in fileParts)
        {
            part.Result.Seek(0, SeekOrigin.Begin);
            part.Result.CopyTo(mergedStream);
        }

        // Сбрасываем позицию на начало объединенного потока
        mergedStream.Seek(0, SeekOrigin.Begin);

        return mergedStream;
    }
}
