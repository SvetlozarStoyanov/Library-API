using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    internal class SeriesBookRepository : ISeriesBookRepository
    {
        private readonly LibraryDbContext dbContext;

        public SeriesBookRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<SeriesBook> DbSet => dbContext.Set<SeriesBook>();


        public IQueryable<SeriesBook> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<SeriesBook> AllAsQueryable(Expression<Func<SeriesBook, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<SeriesBook>> AllReadOnlyAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<SeriesBook>> AllReadOnlyAsync(Expression<Func<SeriesBook, bool>> search)
        {
            return await DbSet
                .Where(search)
                .ToListAsync();
        }

        public async Task AddAsync(SeriesBook entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<SeriesBook> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(SeriesBook entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public async Task<SeriesBook?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(SeriesBook entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<SeriesBook> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Delete(SeriesBook entity)
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
            SeriesBook entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public void DeleteRange(IEnumerable<SeriesBook> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<SeriesBook, bool>> deleteWhereClause)
        {
            IQueryable<SeriesBook> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}
