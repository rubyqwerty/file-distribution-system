using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public static class FileSplitter
{
    /// <summary>
    /// Разделяет файл из входного потока на части заданного размера.
    /// </summary>
    /// <param name="inputStream">Входной поток файла.</param>
    /// <param name="partSize">Размер одной части (в байтах).</param>
    /// <returns>Список объектов типа MemoryStream, содержащих части файла.</returns>
    public static async Task<List<MemoryStream>> SplitStreamAsync(IFormFile file, long partSize)
    {
        var inputStream = file.OpenReadStream();

        if (inputStream == null || !inputStream.CanRead)
            throw new ArgumentException("Входной поток недоступен для чтения.", nameof(inputStream));

        if (partSize <= 0)
            throw new ArgumentException("Размер части должен быть больше нуля.", nameof(partSize));

        var streams = new List<MemoryStream>();
        var totalSize = inputStream.Length;

        while (inputStream.Position < totalSize)
        {
            // Создаем новый MemoryStream для текущей части
            var memoryStream = new MemoryStream();
            var bytesRemaining = Math.Min(partSize, totalSize - inputStream.Position);

            var buffer = new byte[8192]; // 8 KB буфер

            while (bytesRemaining > 0)
            {
                var bytesToRead = (int)Math.Min(buffer.Length, bytesRemaining);
                var bytesRead = await inputStream.ReadAsync(buffer, 0, bytesToRead);
                if (bytesRead == 0) break;

                await memoryStream.WriteAsync(buffer, 0, bytesRead);
                bytesRemaining -= bytesRead;
            }

            // Сбрасываем позицию потока на начало для дальнейшего чтения
            memoryStream.Seek(0, SeekOrigin.Begin);
            streams.Add(memoryStream);
        }

        return streams;
    }
}
