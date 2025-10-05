namespace Beholder.Service;
internal class DataLoaderService : IDataLoaderService
{
    readonly String path = "..\\..\\..\\..\\Assets\\user.json";

    public UserRequest? Load()
    {
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
        {
            UserRequest? deserializeModels = JsonSerializer.Deserialize<UserRequest>(fs);
            if (deserializeModels is not null)
            {
                return deserializeModels;
            }
            return null;
        }
    }

    public void Upload(String login, String password)
    {
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            JsonSerializer.Serialize(fs, new UserRequest(login, password));
        }
    }
}
