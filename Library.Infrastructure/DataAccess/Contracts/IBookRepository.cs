using Library.Infrastructure.Entities;
using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Contracts
{
    public interface IBookRepository : IBaseRepository<long, Book>
    {
        /// <summary>
        /// The result collection won't be tracked by the context
        /// </summary>
        /// <returns>Expression tree</returns>
        Task<IReadOnlyCollection<Book>> AllReadOnlyAsync(IEnumerable<Expression<Func<Book, bool>>> search);
    }
}
