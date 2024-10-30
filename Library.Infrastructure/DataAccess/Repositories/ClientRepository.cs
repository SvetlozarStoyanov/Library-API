using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly LibraryDbContext dbContext;

        public ClientRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<Client> DbSet => dbContext.Set<Client>();


        public IQueryable<Client> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<Client> AllAsQueryable(Expression<Func<Client, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<Client>> AllReadOnlyAsync()
        {
            return await DbSet
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<Client>> AllReadOnlyAsync(Expression<Func<Client, bool>> search)
        {
            return await DbSet
                .Where(search)
                .ToListAsync();
        }

        public async Task AddAsync(Client entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<Client> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(Client entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public async Task<Client?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(Client entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<Client> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Delete(Client entity)
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
            Client entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public void DeleteRange(IEnumerable<Client> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<Client, bool>> deleteWhereClause)
        {
            IQueryable<Client> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}
