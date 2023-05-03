using Salon.DataContext;
using Microsoft.EntityFrameworkCore.ChangeTracking; //EntityEntry<T>
using System.Collections.Concurrent; //для потоко-безопасного словаря


namespace Salon.WebApi.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private static ConcurrentDictionary<short, PriceList>? serviceCache;

        private SalonProjectContext db;
        public ServiceRepository(SalonProjectContext injectedContext)
        {
            db = injectedContext;
            if (serviceCache is null)
            {
                serviceCache = new ConcurrentDictionary<short, PriceList>(
                    db.PriceLists.ToDictionary(c => c.ServiceId));
            }
        }

        public async Task<PriceList?> CreateAsync(PriceList c)
        {
            EntityEntry<PriceList> added = await db.PriceLists.AddAsync(c);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (serviceCache is null)
                    return c;
                //нового клиента в кэш, иначе вызываем UpdateCache
                return serviceCache.AddOrUpdate(c.ServiceId, c, UpdateCache);
            }
            else
            {
                return null;
            }
        }

        public async Task<short?> GetMaxIdAsync() => await Task.FromResult(db.PriceLists.Max(p => p.ServiceId));
        

        public Task<IEnumerable<PriceList>> RetrieveAllAsync()
        {
            return Task.FromResult(serviceCache is null
                ? Enumerable.Empty<PriceList>() : serviceCache.Values);
        }

        public Task<PriceList?> RetrieveAsync(short id)
        {
            if (serviceCache is null) return null!;
            serviceCache.TryGetValue(id, out PriceList? service);
            return Task.FromResult(service);
        }

        private PriceList UpdateCache(short id, PriceList c)
        {
            PriceList? old;
            if (serviceCache is not null)
            {
                if (serviceCache.TryGetValue(id, out old))
                {
                    if (serviceCache.TryUpdate(id, c, old))
                    {
                        return c;
                    }
                }
            }
            return null!;
        }

        public async Task<PriceList?> UpdateAsync(short id, PriceList c)
        {
            //обновляем в базе
            db.PriceLists.Update(c);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                return UpdateCache(id, c);
            }
            return null;
        }

        public async Task<bool?> DeleteAsync(short id)
        {
            PriceList? c = db.PriceLists.Find(id);
            if (c is null) return null;
            db.PriceLists.Remove(c);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (serviceCache is null) return null;
                return serviceCache.TryRemove(id, out c);
            }
            else
            {
                return null;
            }
        }
    }
}
