using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext dbContext;

        public BookRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected DbSet<Book> DbSet => dbContext.Set<Book>();


        public IQueryable<Book> AllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<Book> AllAsQueryable(Expression<Func<Book, bool>> search)
        {
            return DbSet.Where(search).AsQueryable();
        }

        public virtual async Task<IReadOnlyCollection<Book>> AllReadOnlyAsync()
        {
            return await DbSet
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .Include(b => b.Series)
                .ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<Book>> AllReadOnlyAsync(Expression<Func<Book, bool>> search)
        {
            return await DbSet
                .Where(search)
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .Include(b => b.Series)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Book>> AllReadOnlyAsync(IEnumerable<Expression<Func<Book, bool>>> searchList)
        {
            IQueryable<Book>? result = DbSet
                  .Include(b => b.Authors)
                  .Include(b => b.Genres)
                  .Include(b => b.Series)
                  .AsQueryable()
                  .AsNoTracking();

            foreach (Expression<Func<Book, bool>> expression in searchList)
            {
                result = result.Where(expression);
            }

            return await result
                  .ToListAsync();
        }

        public virtual async Task AddAsync(Book entity)
        {
            await DbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<Book> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Detach(Book entity)
        {
            EntityEntry entry = dbContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public virtual async Task<Book?> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual void Update(Book entity)
        {
            DbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<Book> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Delete(Book entity)
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
            Book entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public void DeleteRange(IEnumerable<Book> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<Book, bool>> deleteWhereClause)
        {
            IQueryable<Book> entities = AllAsQueryable(deleteWhereClause);
            DbSet.RemoveRange(entities);
        }

    }
}
