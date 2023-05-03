using Salon.DataContext;

namespace Salon.WebApi.Repositories
{
    public interface IWorkerRepository
    {
        Task<Worker?> CreateAsync(Worker w);
        Task<IEnumerable<Worker>> RetrieveAllAsync();
        Task<Worker?> RetrieveAsync(string id);
        Task<Worker?> UpdateAsync(string id, Worker w);
        Task<bool?> DeleteAsync(string id);
    }
}
