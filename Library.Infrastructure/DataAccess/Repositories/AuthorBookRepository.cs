using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    public class AuthorBookRepository : IAuthorBookRepository
    {
        private readonly LibraryDbContext dbContext;

        public AuthorBookRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<AuthorBook> DbSet => dbContext.Set<AuthorBook>();

        public IQueryable<AuthorBook> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<AuthorBook> AllAsQueryable(Expression<Func<AuthorBook, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<AuthorBook>> AllReadOnlyAsync()
        {
            return await DbSet
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<AuthorBook>> AllReadOnlyAsync(Expression<Func<AuthorBook, bool>> search)
        {
            return await DbSet
                .Where(search)
                .ToListAsync();
        }

        public async Task AddAsync(AuthorBook entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<AuthorBook> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(AuthorBook entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public async Task<AuthorBook?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(AuthorBook entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<AuthorBook> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Delete(AuthorBook entity)
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
            AuthorBook? entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public void DeleteRange(IEnumerable<AuthorBook> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<AuthorBook, bool>> deleteWhereClause)
        {
            IQueryable<AuthorBook> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}
