using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    /// <summary>
    /// Decorator for cached repository
    /// </summary>
    //public class LanguageCachedRepository : LanguageRepository
    //{
    //    private readonly ICacheProvider cacheProvider;
    //    public LanguageCachedRepository(LibraryDbContext dbContext) : base(dbContext)
    //    {
    //        cacheProvider = provider;
    //    }

    //    public override IQueryable<Language> All()
    //    {
    //        List<Language> languages = cacheProvider.GetAll();
    //        if (!languages.Any())
    //        {
    //            languages = base.All();
    //            // logic for cache
    //        }
    //        return languages;
    //    }
    //}

    public class LanguageRepository : ILanguageRepository
    {
        private readonly LibraryDbContext dbContext;

        public LanguageRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<Language> DbSet => dbContext.Set<Language>();


        public IQueryable<Language> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<Language> AllAsQueryable(Expression<Func<Language, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<Language>> AllReadOnlyAsync()
        {
            return await DbSet
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<Language>> AllReadOnlyAsync(Expression<Func<Language, bool>> search)
        {
            return await DbSet
                .Where(search)
                .ToListAsync();
        }

        public virtual async Task AddAsync(Language entity)
        {
            await DbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<Language> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(Language entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public virtual async Task<Language?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual void Update(Language entity)
        {
            DbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<Language> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public virtual void Delete(Language entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            entry.State = EntityState.Deleted;
        }

        public virtual async Task DeleteAsync(long id)
        {
            Language entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public virtual void DeleteRange(IEnumerable<Language> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public virtual void DeleteRange(Expression<Func<Language, bool>> deleteWhereClause)
        {
            IQueryable<Language> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}
