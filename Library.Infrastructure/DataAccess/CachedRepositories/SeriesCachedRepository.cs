using Library.Infrastructure.Constants.CacheTimes;
using Library.Infrastructure.DataAccess.Repositories;
using Library.Infrastructure.Entities;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;
using static Library.Infrastructure.Constants.CacheKeys.CacheKeys;

namespace Library.Infrastructure.DataAccess.CachedRepositories
{
    public class SeriesCachedRepository : SeriesRepository
    {
        private readonly IMemoryCache cache;

        public SeriesCachedRepository(LibraryDbContext dbContext, IMemoryCache cache) : base(dbContext)
        {
            this.cache = cache;
        }

        public override async Task<IReadOnlyCollection<Series>> AllReadOnlyAsync()
        {
            if (!cache.TryGetValue(SeriesCacheKeys.All, out IReadOnlyCollection<Series>? series))
            {
                series = await base.AllReadOnlyAsync();

                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(CacheTimes.CachingDurationsInMinutes.MediumTerm)
                    .SetAbsoluteExpiration(CacheTimes.CachingDurationsInHours.MediumTerm);

                cache.Set(SeriesCacheKeys.All, series, cacheEntryOptions);
            }

            return series!;
        }

        public override async Task<Series?> GetByIdAsync(long id)
        {
            if (cache.TryGetValue(SeriesCacheKeys.All, out IReadOnlyCollection<Series>? allSeries))
            {
                Series? series = allSeries!.FirstOrDefault(s => s.Id == id);

                return series;
            }
            return await base.GetByIdAsync(id);
        }

        public override async Task AddAsync(Series entity)
        {
            cache.Remove(SeriesCacheKeys.All);
            await base.AddAsync(entity);
        }

        public override async Task AddRangeAsync(IEnumerable<Series> entities)
        {
            cache.Remove(SeriesCacheKeys.All);
            await base.AddRangeAsync(entities);
        }

        public override void Delete(Series entity)
        {
            cache.Remove(SeriesCacheKeys.All);
            base.Delete(entity);
        }

        public override async Task DeleteAsync(long id)
        {
            cache.Remove(SeriesCacheKeys.All);
            await base.DeleteAsync(id);
        }

        public override void DeleteRange(IEnumerable<Series> entities)
        {
            cache.Remove(SeriesCacheKeys.All);
            base.DeleteRange(entities);
        }

        public override void DeleteRange(Expression<Func<Series, bool>> deleteWhereClause)
        {
            cache.Remove(SeriesCacheKeys.All);
            base.DeleteRange(deleteWhereClause);
        }
    }
}
