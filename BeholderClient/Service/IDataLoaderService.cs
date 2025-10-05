namespace Beholder.Service;

public interface IDataLoaderService
{
    UserRequest? Load();
    void Upload(String login, String password);
}
