using Library.Infrastructure.Constants.CacheTimes;
using Library.Infrastructure.DataAccess.Repositories;
using Library.Infrastructure.Entities;
using Microsoft.Extensions.Caching.Memory;
using static Library.Infrastructure.Constants.CacheKeys.CacheKeys;

namespace Library.Infrastructure.DataAccess.CachedRepositories
{
    public class CountryCachedRepository : CountryRepository
    {
        private readonly IMemoryCache cache;

        public CountryCachedRepository(LibraryDbContext dbContext, IMemoryCache cache) : base(dbContext)
        {
            this.cache = cache;
        }

        public override async Task<IReadOnlyCollection<Country>> AllReadOnlyAsync()
        {
            if (!cache.TryGetValue(CountryCacheKeys.All, out IReadOnlyCollection<Country>? allCountries))
            {
                allCountries = await base.AllReadOnlyAsync();

                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(CacheTimes.CachingDurationsInHours.MediumTerm)
                    .SetAbsoluteExpiration(CacheTimes.CachingDurationsInHours.LongTerm);

                cache.Set(CountryCacheKeys.All, allCountries, cacheOptions);
            }

            return allCountries!;
        }

        public override async Task<Country?> GetByIdAsync(long id)
        {
            if (cache.TryGetValue(CountryCacheKeys.All, out IReadOnlyCollection<Country>? allCountries))
            {
                Country? country = allCountries!.FirstOrDefault(a => a.Id == id);
                return country!;
            }
            return await base.GetByIdAsync(id);
        }

        public override Task AddAsync(Country entity)
        {
            cache.Remove(CountryCacheKeys.All);
            return base.AddAsync(entity);
        }

        public override Task AddRangeAsync(IEnumerable<Country> entities)
        {
            cache.Remove(CountryCacheKeys.All);
            return base.AddRangeAsync(entities);
        }
    }
}
