using System.Linq.Expressions;

namespace RestrantApplication.Core.Interfaces
{
    /// <summary>
    /// Defines generic repository operations for any entity.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves all records of the entity.
        /// </summary>
        Task<IReadOnlyList<T>> GetAllAsync();

        /// <summary>
        /// Retrieves all records with related entities included.
        /// </summary>
        /// <param name="includes">Navigation properties to include</param>
        Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Gets an entity by its ID, including specified navigation properties.
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="includes">Navigation properties to include</param>
        Task<T> GetByIDAsync(int id, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Gets an entity by its ID.
        /// </summary>
        /// <param name="id">Entity ID</param>
        Task<T> GetByIDAsync(int id);

        /// <summary>
        /// Returns the total count of entities.
        /// </summary>
        Task<int> Count();

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="item">Entity to update</param>
        void Update(T item);

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="id">Entity ID</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        /// <param name="item">Entity to add</param>
        Task AddAsync(T item);
    }
}
