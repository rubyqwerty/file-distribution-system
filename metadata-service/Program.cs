

// Эмуляция файла: создаем поток с данными
using (var inputStream = new MemoryStream())
{
    // Наполняем поток тестовыми данными
    var testData = new byte[5500000]; // 5.5 MB данных
    new Random().NextBytes(testData);
    await inputStream.WriteAsync(testData, 0, testData.Length);

    // Сбрасываем позицию в начало для чтения
    inputStream.Seek(0, SeekOrigin.Begin);

    long partSize = 2 * 1024 * 1024; // 2 MB

    try
    {
        // Разделение на части
        var streams = await FileSplitter.SplitStreamAsync(inputStream, partSize);

        Console.WriteLine("Файл разделен на части (потоки):");
        int partIndex = 1;
        foreach (var stream in streams)
        {
            Console.WriteLine($"Часть {partIndex}: {stream.Length} байт");
            partIndex++;
        }

        // Не забудьте освободить потоки после использования
        foreach (var stream in streams)
        {
            stream.Dispose();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }

}