using Library.Infrastructure.Constants.CacheTimes;
using Library.Infrastructure.DataAccess.Repositories;
using Library.Infrastructure.Entities;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;
using static Library.Infrastructure.Constants.CacheKeys.CacheKeys;

namespace Library.Infrastructure.DataAccess.CachedRepositories
{
    internal class GenreCachedRepository : GenreRepository
    {
        private readonly LibraryDbContext context;
        private readonly IMemoryCache cache;

        public GenreCachedRepository(LibraryDbContext dbContext, IMemoryCache cache) : base(dbContext)
        {
            this.cache = cache;
        }

        public override async Task<IReadOnlyCollection<Genre>> AllReadOnlyAsync()
        {
            if (!cache.TryGetValue(GenreCacheKeys.All, out IReadOnlyCollection<Genre>? allGenres))
            {
                allGenres = await base.AllReadOnlyAsync();

                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(CacheTimes.CachingDurationsInMinutes.MediumTerm)
                    .SetAbsoluteExpiration(CacheTimes.CachingDurationsInHours.MediumTerm);

                cache.Set(GenreCacheKeys.All, allGenres, cacheEntryOptions);
            }

            return allGenres!;
        }

        public override async Task<Genre?> GetByIdAsync(long id)
        {
            if (cache.TryGetValue(GenreCacheKeys.All, out IReadOnlyCollection<Genre>? allGenres))
            {
                Genre? genre = allGenres!.FirstOrDefault(s => s.Id == id);

                return genre;
            }
            return await base.GetByIdAsync(id);
        }

        public override async Task AddAsync(Genre entity)
        {
            cache.Remove(GenreCacheKeys.All);
            await base.AddAsync(entity);
        }

        public override async Task AddRangeAsync(IEnumerable<Genre> entities)
        {
            cache.Remove(GenreCacheKeys.All);
            await base.AddRangeAsync(entities);
        }

        public override void Delete(Genre entity)
        {
            cache.Remove(GenreCacheKeys.All);
            base.Delete(entity);
        }

        public override async Task DeleteAsync(long id)
        {
            cache.Remove(GenreCacheKeys.All);
            await base.DeleteAsync(id);
        }

        public override void DeleteRange(IEnumerable<Genre> entities)
        {
            cache.Remove(GenreCacheKeys.All);
            base.DeleteRange(entities);
        }

        public override void DeleteRange(Expression<Func<Genre, bool>> deleteWhereClause)
        {
            cache.Remove(GenreCacheKeys.All);
            base.DeleteRange(deleteWhereClause);
        }
    }
}
