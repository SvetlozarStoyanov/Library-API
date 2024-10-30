using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    public class ClientCardTypeRepository : IClientCardTypeRepository
    {
        private readonly LibraryDbContext dbContext;

        public ClientCardTypeRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<ClientCardType> DbSet => dbContext.Set<ClientCardType>();


        public IQueryable<ClientCardType> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<ClientCardType> AllAsQueryable(Expression<Func<ClientCardType, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<ClientCardType>> AllReadOnlyAsync()
        {
            return await DbSet
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<ClientCardType>> AllReadOnlyAsync(Expression<Func<ClientCardType, bool>> search)
        {
            return await DbSet
                .Where(search)
                .ToListAsync();
        }

        public async Task AddAsync(ClientCardType entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<ClientCardType> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(ClientCardType entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public async Task<ClientCardType?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(ClientCardType entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<ClientCardType> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Delete(ClientCardType entity)
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
            ClientCardType entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public void DeleteRange(IEnumerable<ClientCardType> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<ClientCardType, bool>> deleteWhereClause)
        {
            IQueryable<ClientCardType> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}
