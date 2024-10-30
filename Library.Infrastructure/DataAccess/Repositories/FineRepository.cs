using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    public class FineRepository : IFineRepository
    {
        private readonly LibraryDbContext dbContext;

        public FineRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<Fine> DbSet => dbContext.Set<Fine>();


        public IQueryable<Fine> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<Fine> AllAsQueryable(Expression<Func<Fine, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<Fine>> AllReadOnlyAsync()
        {
            return await DbSet
                .Include(f => f.Checkout)
                .ThenInclude(ch => ch.Book)
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<Fine>> AllReadOnlyAsync(Expression<Func<Fine, bool>> search)
        {
            return await DbSet
                .Where(search)
                .Include(f => f.Checkout)
                .ThenInclude(ch => ch.Book)
                .ToListAsync();
        }

        public async Task AddAsync(Fine entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<Fine> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(Fine entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public async Task<Fine?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(Fine entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<Fine> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Delete(Fine entity)
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
            Fine entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public void DeleteRange(IEnumerable<Fine> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<Fine, bool>> deleteWhereClause)
        {
            IQueryable<Fine> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}
