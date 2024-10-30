using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    public class ClientCardStatusChangeRepository : IClientCardStatusChangeRepository
    {
        private readonly LibraryDbContext dbContext;

        public ClientCardStatusChangeRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<ClientCardStatusChange> DbSet => dbContext.Set<ClientCardStatusChange>();


        public IQueryable<ClientCardStatusChange> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<ClientCardStatusChange> AllAsQueryable(Expression<Func<ClientCardStatusChange, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<ClientCardStatusChange>> AllReadOnlyAsync()
        {
            return await DbSet
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<ClientCardStatusChange>> AllReadOnlyAsync(Expression<Func<ClientCardStatusChange, bool>> search)
        {
            return await DbSet
                .Where(search)
                .ToListAsync();
        }

        public async Task AddAsync(ClientCardStatusChange entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<ClientCardStatusChange> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(ClientCardStatusChange entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public async Task<ClientCardStatusChange?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(ClientCardStatusChange entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<ClientCardStatusChange> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Delete(ClientCardStatusChange entity)
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
            ClientCardStatusChange entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public void DeleteRange(IEnumerable<ClientCardStatusChange> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<ClientCardStatusChange, bool>> deleteWhereClause)
        {
            IQueryable<ClientCardStatusChange> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}
