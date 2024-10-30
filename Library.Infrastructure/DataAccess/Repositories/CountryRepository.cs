using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly LibraryDbContext dbContext;

        public CountryRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<Country> DbSet => dbContext.Set<Country>();


        public IQueryable<Country> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<Country> AllAsQueryable(Expression<Func<Country, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<Country>> AllReadOnlyAsync()
        {
            return await DbSet
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<Country>> AllReadOnlyAsync(Expression<Func<Country, bool>> search)
        {
            return await DbSet
                .Where(search)
                .ToListAsync();
        }

        public virtual async Task AddAsync(Country entity)
        {
            await DbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<Country> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(Country entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public virtual async Task<Country?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(Country entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<Country> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Delete(Country entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            entry.State = EntityState.Deleted;
        }

        public async Task DeleteAsync(long id)
        {
            Country entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public void DeleteRange(IEnumerable<Country> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<Country, bool>> deleteWhereClause)
        {
            IQueryable<Country> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}
