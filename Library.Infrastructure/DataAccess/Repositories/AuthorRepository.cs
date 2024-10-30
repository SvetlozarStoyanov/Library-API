using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext dbContext;

        public AuthorRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<Author> DbSet => dbContext.Set<Author>();


        public IQueryable<Author> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<Author> AllAsQueryable(Expression<Func<Author, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<Author>> AllReadOnlyAsync()
        {
            return await DbSet
                .Include(a => a.Books)
                .ThenInclude(b => b.Genres)
                .Include(a => a.Books)
                .ThenInclude(b => b.Series)
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<Author>> AllReadOnlyAsync(Expression<Func<Author, bool>> search)
        {
            return await DbSet
                .Where(search)
                .Include(a => a.Books)
                .ThenInclude(b => b.Genres)
                .Include(a => a.Books)
                .ThenInclude(b => b.Series)
                .ToListAsync();
        }

        public virtual async Task AddAsync(Author entity)
        {
            await DbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<Author> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(Author entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public virtual async Task<Author?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual void Update(Author entity)
        {
            DbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<Author> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public virtual void Delete(Author entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            entry.State = EntityState.Deleted;
        }

        public virtual async Task DeleteAsync(long id)
        {
            Author entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public virtual void DeleteRange(IEnumerable<Author> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public virtual void DeleteRange(Expression<Func<Author, bool>> deleteWhereClause)
        {
            IQueryable<Author> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}

