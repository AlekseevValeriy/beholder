using System.Net.Http.Json;

using Microsoft.Maui.Storage;

namespace Beholder.Service;
internal class DataLoaderService : IDataLoaderService
{
    readonly String appDataDirectory = FileSystem.AppDataDirectory;
    readonly String fileName = "user.json";

    public String GetFilePath()
    {
        String appDataDirectory = FileSystem.AppDataDirectory;
        return Path.Combine(appDataDirectory, fileName);
    }

    public UserRequest? Load()
    {
        String path = GetFilePath();

        if (!File.Exists(path))
        {
            File.WriteAllText(path, JsonSerializer.Serialize(new UserRequest("", "")));
            return null;
        }

        try
        {
            String jsonString = File.ReadAllText(path);
            UserRequest? deserializeModels = JsonSerializer.Deserialize<UserRequest>(jsonString);
            
            return deserializeModels;
        }
        catch
        {
            return null;
        }
    }

    public void Upload(String login, String password)
    {
        String path = GetFilePath();

        if (File.Exists(path))
        {
            try
            {
                String jsonContent = JsonSerializer.Serialize(new UserRequest(login, password));

                File.WriteAllText(path, jsonContent);
            }
            catch { }
        }
    }
}
