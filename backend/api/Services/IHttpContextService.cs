namespace api.Services
{
    public interface IHttpContextService
    {
        Task<string> GetToken();
    }
}
