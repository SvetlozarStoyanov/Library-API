using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    public class SeriesRepository : ISeriesRepository
    {
        private readonly LibraryDbContext dbContext;

        public SeriesRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<Series> DbSet => dbContext.Set<Series>();


        public IQueryable<Series> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<Series> AllAsQueryable(Expression<Func<Series, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<Series>> AllReadOnlyAsync()
        {
            return await DbSet
                .Include(s => s.Books)
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<Series>> AllReadOnlyAsync(Expression<Func<Series, bool>> search)
        {
            return await DbSet
                .Where(search)
                .Include(s => s.Books)
                .ToListAsync();
        }

        public virtual async Task AddAsync(Series entity)
        {
            await DbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<Series> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(Series entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public virtual async Task<Series?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual void Update(Series entity)
        {
            DbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<Series> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public virtual void Delete(Series entity)
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
            Series entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public virtual void DeleteRange(IEnumerable<Series> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public virtual void DeleteRange(Expression<Func<Series, bool>> deleteWhereClause)
        {
            IQueryable<Series> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}
