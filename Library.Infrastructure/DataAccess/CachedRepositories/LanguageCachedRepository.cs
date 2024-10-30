using Library.Infrastructure.Constants.CacheTimes;
using Library.Infrastructure.DataAccess.Repositories;
using Library.Infrastructure.Entities;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;
using static Library.Infrastructure.Constants.CacheKeys.CacheKeys;

namespace Library.Infrastructure.DataAccess.CachedRepositories
{
    public class LanguageCachedRepository : LanguageRepository
    {
        private readonly IMemoryCache cache;

        public LanguageCachedRepository(LibraryDbContext dbContext, IMemoryCache cache) : base(dbContext)
        {
            this.cache = cache;
        }

        public override async Task<IReadOnlyCollection<Language>> AllReadOnlyAsync()
        {
            if (!cache.TryGetValue(LanguageCacheKeys.All, out IReadOnlyCollection<Language>? allLanguages))
            {
                allLanguages = await base.AllReadOnlyAsync();

                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(CacheTimes.CachingDurationsInHours.LongTerm);

                cache.Set(LanguageCacheKeys.All, allLanguages, cacheEntryOptions);
            }

            return allLanguages!;
        }

        public override async Task<Language?> GetByIdAsync(long id)
        {
            if (cache.TryGetValue(LanguageCacheKeys.All, out IReadOnlyCollection<Language>? allLanguages))
            {
                Language? language = allLanguages!.FirstOrDefault(s => s.Id == id);

                return language;
            }
            return await base.GetByIdAsync(id);
        }

        public override async Task AddAsync(Language entity)
        {
            cache.Remove(LanguageCacheKeys.All);
            await base.AddAsync(entity);
        }

        public override async Task AddRangeAsync(IEnumerable<Language> entities)
        {
            cache.Remove(LanguageCacheKeys.All);
            await base.AddRangeAsync(entities);
        }

        public override void Delete(Language entity)
        {
            cache.Remove(LanguageCacheKeys.All);
            base.Delete(entity);
        }

        public override async Task DeleteAsync(long id)
        {
            cache.Remove(LanguageCacheKeys.All);
            await base.DeleteAsync(id);
        }

        public override void DeleteRange(IEnumerable<Language> entities)
        {
            cache.Remove(LanguageCacheKeys.All);
            base.DeleteRange(entities);
        }

        public override void DeleteRange(Expression<Func<Language, bool>> deleteWhereClause)
        {
            cache.Remove(LanguageCacheKeys.All);
            base.DeleteRange(deleteWhereClause);
        }
    }
}
