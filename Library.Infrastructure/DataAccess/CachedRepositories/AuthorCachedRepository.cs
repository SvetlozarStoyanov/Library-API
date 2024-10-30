using Library.Infrastructure.Constants.CacheTimes;
using Library.Infrastructure.DataAccess.Repositories;
using Library.Infrastructure.Entities;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;
using static Library.Infrastructure.Constants.CacheKeys.CacheKeys;

namespace Library.Infrastructure.DataAccess.CachedRepositories
{
    public class AuthorCachedRepository : AuthorRepository
    {
        private readonly IMemoryCache cache;

        public AuthorCachedRepository(LibraryDbContext dbContext, IMemoryCache cache) : base(dbContext)
        {
            this.cache = cache;
        }

        public override async Task<IReadOnlyCollection<Author>> AllReadOnlyAsync()
        {
            if (!cache.TryGetValue(AuthorCacheKeys.All, out IReadOnlyCollection<Author>? allAuthors))
            {
                allAuthors = await base.AllReadOnlyAsync();

                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(CacheTimes.CachingDurationsInHours.LongTerm);

                cache.Set(AuthorCacheKeys.All, allAuthors, cacheEntryOptions);
            }
            return allAuthors!;
        }

        public override async Task<Author?> GetByIdAsync(long id)
        {
            if (cache.TryGetValue(AuthorCacheKeys.All, out IReadOnlyCollection<Author>? allAuthors))
            {
                Author? author = allAuthors!.FirstOrDefault(a => a.Id == id);

                return author!;
            }
            return await base.GetByIdAsync(id);
        }

        public override async Task AddAsync(Author entity)
        {
            cache.Remove(AuthorCacheKeys.All);
            await base.AddAsync(entity);
        }

        public override async Task AddRangeAsync(IEnumerable<Author> entities)
        {
            cache.Remove(AuthorCacheKeys.All);
            await base.AddRangeAsync(entities);
        }

        public override void Update(Author entity)
        {
            cache.Remove(string.Format(AuthorCacheKeys.ById, entity.Id));
            base.Update(entity);
        }

        public override void UpdateRange(IEnumerable<Author> entities)
        {
            cache.Remove(AuthorCacheKeys.All);
            base.UpdateRange(entities);
        }

        public override void Delete(Author entity)
        {
            cache.Remove(AuthorCacheKeys.All);
            base.Delete(entity);
        }

        public override async Task DeleteAsync(long id)
        {
            cache.Remove(AuthorCacheKeys.All);
            await base.DeleteAsync(id);
        }

        public override void DeleteRange(IEnumerable<Author> entities)
        {
            cache.Remove(AuthorCacheKeys.All);
            base.DeleteRange(entities);
        }

        public override void DeleteRange(Expression<Func<Author, bool>> deleteWhereClause)
        {
            IQueryable<Author> entities = AllAsQueryable(deleteWhereClause);
            this.DeleteRange(entities);
        }
    }
}
