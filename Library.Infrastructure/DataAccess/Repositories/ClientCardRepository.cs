using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    public class ClientCardRepository : IClientCardRepository
    {
        private readonly LibraryDbContext dbContext;

        public ClientCardRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<ClientCard> DbSet => dbContext.Set<ClientCard>();


        public IQueryable<ClientCard> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<ClientCard> AllAsQueryable(Expression<Func<ClientCard, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<ClientCard>> AllReadOnlyAsync()
        {
            return await DbSet
                .Include(cc => cc.Client)
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<ClientCard>> AllReadOnlyAsync(Expression<Func<ClientCard, bool>> search)
        {
            return await DbSet
                .Where(search)
                .Include(cc => cc.Client)
                .ToListAsync();
        }

        public async Task AddAsync(ClientCard entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<ClientCard> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(ClientCard entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public async Task<ClientCard?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(ClientCard entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<ClientCard> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Delete(ClientCard entity)
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
            ClientCard entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public void DeleteRange(IEnumerable<ClientCard> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<ClientCard, bool>> deleteWhereClause)
        {
            IQueryable<ClientCard> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}
