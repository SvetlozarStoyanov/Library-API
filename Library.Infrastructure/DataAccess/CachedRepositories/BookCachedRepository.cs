using Library.Infrastructure.Constants.CacheTimes;
using Library.Infrastructure.DataAccess.Repositories;
using Library.Infrastructure.Entities;
using Microsoft.Extensions.Caching.Memory;
using static Library.Infrastructure.Constants.CacheKeys.CacheKeys;

namespace Library.Infrastructure.DataAccess.CachedRepositories
{
    public class BookCachedRepository : BookRepository
    {
        private readonly IMemoryCache cache;

        public BookCachedRepository(LibraryDbContext dbContext, IMemoryCache cache) : base(dbContext)
        {
            this.cache = cache;
        }

        public override async Task<IReadOnlyCollection<Book>> AllReadOnlyAsync()
        {
            if (!cache.TryGetValue(BookCacheKeys.All, out IReadOnlyCollection<Book>? allBooks))
            {
                allBooks = await base.AllReadOnlyAsync();

                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(CacheTimes.CachingDurationsInHours.MediumTerm)
                    .SetSlidingExpiration(CacheTimes.CachingDurationsInMinutes.LongTerm);

                cache.Set(BookCacheKeys.All, allBooks, cacheEntryOptions);
            }
            return allBooks!;
        }

        public override async Task<Book?> GetByIdAsync(long id)
        {
            if (cache.TryGetValue(BookCacheKeys.All, out IReadOnlyCollection<Book>? allBooks))
            {
                Book? book = allBooks!.FirstOrDefault(a => a.Id == id);
                return book!;
            }
            return await base.GetByIdAsync(id);
        }

        public override Task AddAsync(Book entity)
        {
            cache.Remove(BookCacheKeys.All);
            return base.AddAsync(entity);
        }

        public override Task AddRangeAsync(IEnumerable<Book> entities)
        {
            cache.Remove(BookCacheKeys.All);
            return base.AddRangeAsync(entities);
        }

        public override void Update(Book entity)
        {
            cache.Remove(BookCacheKeys.All);
            base.Update(entity);
        }

        public override void UpdateRange(IEnumerable<Book> entities)
        {
            cache.Remove(BookCacheKeys.All);
            base.UpdateRange(entities);
        }
    }
}
