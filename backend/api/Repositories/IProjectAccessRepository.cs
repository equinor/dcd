namespace api.Repositories
{
    public interface IProjectAccessRepository
    {
        Task<T?> Get<T>(Guid id) where T : class;
    }
}
