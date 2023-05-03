using Salon.DataContext;

namespace Salon.WebApi.Repositories
{
    public interface IServiceRepository
    {
        Task<PriceList?> CreateAsync(PriceList w);
        Task<IEnumerable<PriceList>> RetrieveAllAsync();
        Task<short?> GetMaxIdAsync();
        Task<PriceList?> RetrieveAsync(short id);
        Task<PriceList?> UpdateAsync(short id, PriceList w);
        Task<bool?> DeleteAsync(short id);
    }
}
