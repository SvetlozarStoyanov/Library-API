using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    public class BookAcquisitionRepository : IBookAcquisitionRepository
    {
        private readonly LibraryDbContext dbContext;

        public BookAcquisitionRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<BookAcquisition> DbSet => dbContext.Set<BookAcquisition>();


        public IQueryable<BookAcquisition> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<BookAcquisition> AllAsQueryable(Expression<Func<BookAcquisition, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<BookAcquisition>> AllReadOnlyAsync()
        {
            return await DbSet
                .Include(b => b.Book)
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<BookAcquisition>> AllReadOnlyAsync(Expression<Func<BookAcquisition, bool>> search)
        {
            return await DbSet
                .Where(search)
                .Include(b => b.Book)
                .ToListAsync();
        }

        public async Task AddAsync(BookAcquisition entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<BookAcquisition> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(BookAcquisition entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public async Task<BookAcquisition?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(BookAcquisition entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<BookAcquisition> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Delete(BookAcquisition entity)
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
            BookAcquisition entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public void DeleteRange(IEnumerable<BookAcquisition> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<BookAcquisition, bool>> deleteWhereClause)
        {
            IQueryable<BookAcquisition> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}
