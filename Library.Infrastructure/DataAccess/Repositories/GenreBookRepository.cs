using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    public class GenreBookRepository : IGenreBookRepository
    {
        private readonly LibraryDbContext dbContext;

        public GenreBookRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<GenreBook> DbSet => dbContext.Set<GenreBook>();


        public IQueryable<GenreBook> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<GenreBook> AllAsQueryable(Expression<Func<GenreBook, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<GenreBook>> AllReadOnlyAsync()
        {
            return await DbSet
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<GenreBook>> AllReadOnlyAsync(Expression<Func<GenreBook, bool>> search)
        {
            return await DbSet
                .Where(search)
                .ToListAsync();
        }

        public async Task AddAsync(GenreBook entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<GenreBook> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(GenreBook entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public async Task<GenreBook?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(GenreBook entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<GenreBook> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Delete(GenreBook entity)
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
            GenreBook entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public void DeleteRange(IEnumerable<GenreBook> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<GenreBook, bool>> deleteWhereClause)
        {
            IQueryable<GenreBook> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }
    }
}
