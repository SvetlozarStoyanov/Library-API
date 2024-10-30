using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly LibraryDbContext dbContext;

        public CheckoutRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<Checkout> DbSet => dbContext.Set<Checkout>();


        public IQueryable<Checkout> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<Checkout> AllAsQueryable(Expression<Func<Checkout, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<Checkout>> AllReadOnlyAsync()
        {
            return await DbSet
                .Include(ch => ch.ClientCard)
                .Include(ch => ch.Book)
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<Checkout>> AllReadOnlyAsync(Expression<Func<Checkout, bool>> search)
        {
            return await DbSet
                .Where(search)
                .Include(ch => ch.ClientCard)
                .Include(ch => ch.Book)
                .ToListAsync();
        }

        public async Task AddAsync(Checkout entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<Checkout> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(Checkout entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public async Task<Checkout?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(Checkout entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<Checkout> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Delete(Checkout entity)
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
            Checkout entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public void DeleteRange(IEnumerable<Checkout> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<Checkout, bool>> deleteWhereClause)
        {
            IQueryable<Checkout> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}
