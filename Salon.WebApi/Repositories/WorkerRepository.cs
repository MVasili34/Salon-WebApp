using Salon.DataContext;
using Microsoft.EntityFrameworkCore.ChangeTracking; //EntityEntry<T>
using System.Collections.Concurrent; //для потоко-безопасного словаря

namespace Salon.WebApi.Repositories
{
    public class WorkerRepository : IWorkerRepository
    {
        //используем потокобезопасный словарь для кэширования работников
        private static ConcurrentDictionary<string, Worker>? workerCache;

        private SalonProjectContext db;
        public WorkerRepository(SalonProjectContext injectedContext) 
        {
            db = injectedContext;
            if (workerCache is null)
            {
                workerCache = new ConcurrentDictionary<string, Worker>(
                    db.Workers.ToDictionary(c=>c.PatentId));
            }
        }

        public async Task<Worker?> CreateAsync(Worker c)
        {
            c.PatentId = c.PatentId.ToUpper();
            EntityEntry<Worker> added = await db.Workers.AddAsync(c);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (workerCache is null)
                    return c;
                //нового клиента в кэш, иначе вызываем UpdateCache
                return workerCache.AddOrUpdate(c.PatentId, c, UpdateCache);
            }
            else
            {
                return null;
            }
        }

        public Task<IEnumerable<Worker>> RetrieveAllAsync()
        {
            return Task.FromResult(workerCache is null
                ? Enumerable.Empty<Worker>() : workerCache.Values);
        }

        public Task<Worker?> RetrieveAsync(string id)
        {
            id = id.ToUpper();
            if (workerCache is null) return null!;
            workerCache.TryGetValue(id, out Worker? worker);
            return Task.FromResult(worker);
        }

        private Worker UpdateCache(string id, Worker c)
        {
            Worker? old;
            if (workerCache is not null)
            {
                if (workerCache.TryGetValue(id, out old)) 
                {
                    if (workerCache.TryUpdate(id, c, old))
                    {
                        return c;
                    }
                }
            }
            return null!;
        }

        public async Task<Worker?> UpdateAsync(string id, Worker c)
        {
            id = id.ToUpper();
            c.PatentId = c.PatentId.ToUpper();
            //обновляем в базе
            db.Workers.Update(c);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                return UpdateCache(id, c);
            }
            return null;
        }

        public async Task<bool?> DeleteAsync(string id)
        {
            id = id.ToUpper();
            Worker? c = db.Workers.Find(id);
            if (c is null) return null;
            db.Workers.Remove(c);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (workerCache is null) return null;
                return workerCache.TryRemove(id, out c);
            }
            else
            {
                return null;
            }
        }
    }
}
