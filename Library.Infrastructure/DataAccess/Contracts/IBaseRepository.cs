using System.Linq.Expressions;

namespace Library.Infrastructure.DataAccess.Contracts
{
    public interface IBaseRepository<TKey, TEntity> where TEntity : class
    {
        /// <summary>
        /// All records in a table
        /// </summary>
        /// <returns>Queryable expression tree</returns>
        IQueryable<TEntity> AllAsQueryable();

        /// <summary>
        /// All records in a table
        /// </summary>
        /// <returns>Queryable expression tree</returns>
        IQueryable<TEntity> AllAsQueryable(Expression<Func<TEntity, bool>> search);

        /// <summary>
        /// The result collection won't be tracked by the context
        /// </summary>
        /// <returns>Expression tree</returns>
        Task<IReadOnlyCollection<TEntity>> AllReadOnlyAsync();

        /// <summary>
        /// The result collection won't be tracked by the context
        /// </summary>
        /// <returns>Expression tree</returns>
        Task<IReadOnlyCollection<TEntity>> AllReadOnlyAsync(Expression<Func<TEntity, bool>> search);

        /// <summary>
        /// Gets specific record from database by primary key
        /// </summary>
        /// <param name="id">record identificator</param>
        /// <returns>Single record</returns>
        Task<TEntity?> GetByIdAsync(TKey id);

        //Task<TEntity> GetByIdsAsync(object[] id);

        /// <summary>
        /// Adds entity to the database
        /// </summary>
        /// <param name="entity">Entity to add</param>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Ads collection of entities to the database
        /// </summary>
        /// <param name="entities">Enumerable list of entities</param>
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates a record in database
        /// </summary>
        /// <param name="entity">Entity for record to be updated</param>
        void Update(TEntity entity);

        /// <summary>
        /// Updates set of records in the database
        /// </summary>
        /// <param name="entities">Enumerable collection of entities to be updated</param>
        void UpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes a record from database
        /// </summary>
        /// <param name="id">Identificator of record to be deleted</param>
        Task DeleteAsync(TKey id);

        /// <summary>
        /// Deletes a record from database
        /// </summary>
        /// <param name="entity">Entity representing record to be deleted</param>
        void Delete(TEntity entity);

        void DeleteRange(IEnumerable<TEntity> entities);

        void DeleteRange(Expression<Func<TEntity, bool>> deleteWhereClause);

        /// <summary>
        /// Detaches given entity from the context
        /// </summary>
        /// <param name="entity">Entity to be detached</param>
        void Detach(TEntity entity);
    }
}
