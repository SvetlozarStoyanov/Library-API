using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly LibraryDbContext dbContext;

        public GenreRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<Genre> DbSet => dbContext.Set<Genre>();


        public IQueryable<Genre> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<Genre> AllAsQueryable(Expression<Func<Genre, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<Genre>> AllReadOnlyAsync()
        {
            return await DbSet
                .Include(g => g.Books)
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<Genre>> AllReadOnlyAsync(Expression<Func<Genre, bool>> search)
        {
            return await DbSet
                .Where(search)
                .Include(g => g.Books)
                .ToListAsync();
        }

        public virtual async Task AddAsync(Genre entity)
        {
            await DbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<Genre> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(Genre entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public virtual async Task<Genre?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual void Update(Genre entity)
        {
            DbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<Genre> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public virtual void Delete(Genre entity)
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
            Genre entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public virtual void DeleteRange(IEnumerable<Genre> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public virtual void DeleteRange(Expression<Func<Genre, bool>> deleteWhereClause)
        {
            IQueryable<Genre> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}
