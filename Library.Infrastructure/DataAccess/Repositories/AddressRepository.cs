using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly LibraryDbContext dbContext;

        public AddressRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<Address> DbSet => dbContext.Set<Address>();

        public IQueryable<Address> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<Address> AllAsQueryable(Expression<Func<Address, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<Address>> AllReadOnlyAsync()
        {
            return await DbSet
                .Include(a => a.Country)
                .Include(a => a.Client)
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<Address>> AllReadOnlyAsync(Expression<Func<Address, bool>> search)
        {
            return await DbSet
                .Where(search)
                .Include(a => a.Country)
                .Include(a => a.Client)
                .ToListAsync();
        }

        public async Task AddAsync(Address entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<Address> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(Address entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public async Task<Address?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(Address entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<Address> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Delete(Address entity)
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
            Address entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public void DeleteRange(IEnumerable<Address> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<Address, bool>> deleteWhereClause)
        {
            IQueryable<Address> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}
