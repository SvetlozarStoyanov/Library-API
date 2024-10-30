using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    public class PhoneNumberRepository : IPhoneNumberRepository
    {
        private readonly LibraryDbContext dbContext;

        public PhoneNumberRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<PhoneNumber> DbSet => dbContext.Set<PhoneNumber>();


        public IQueryable<PhoneNumber> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<PhoneNumber> AllAsQueryable(Expression<Func<PhoneNumber, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<PhoneNumber>> AllReadOnlyAsync()
        {
            return await DbSet
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<PhoneNumber>> AllReadOnlyAsync(Expression<Func<PhoneNumber, bool>> search)
        {
            return await DbSet
                .Where(search)
                .ToListAsync();
        }

        public async Task AddAsync(PhoneNumber entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<PhoneNumber> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(PhoneNumber entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public async Task<PhoneNumber?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(PhoneNumber entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<PhoneNumber> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Delete(PhoneNumber entity)
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
            PhoneNumber entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public void DeleteRange(IEnumerable<PhoneNumber> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<PhoneNumber, bool>> deleteWhereClause)
        {
            IQueryable<PhoneNumber> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}
