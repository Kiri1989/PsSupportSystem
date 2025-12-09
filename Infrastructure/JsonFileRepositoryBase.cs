using System.Text.Json;
using System.IO;
using System.Collections.Generic;

namespace PsSupportSystem.App.Infrastructure;

public abstract class JsonFileRepositoryBase<T> where T : class
{
    protected readonly string FilePath;
    protected List<T> Items = new();

    protected JsonFileRepositoryBase(string filePath)
    {
        FilePath = filePath;
        Load();
    }

    private void Load()
    {
        if (!File.Exists(FilePath))
        {
            Items = new List<T>();
            return;
        }

        var json = File.ReadAllText(FilePath);
        var data = JsonSerializer.Deserialize<List<T>>(json);
        Items = data ?? new List<T>();
    }

    protected void Save()
    {
        var json = JsonSerializer.Serialize(Items, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(FilePath, json);
    }
}
