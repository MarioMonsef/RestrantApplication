using Microsoft.EntityFrameworkCore;
using RestrantApplication.Core.Interfaces;
using RestrantApplication.EF.Entities;
using System.Linq.Expressions;

namespace RestrantApplication.EF.Repository
{
    /// <summary>
    /// A generic repository implementation for CRUD operations.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        #region Fields
        protected readonly AppDBContext _context;

        #endregion

        #region Constructors
        public GenericRepository(AppDBContext context)
        {
            _context = context;
        }
        #endregion

        #region Handel Functions

        /// <summary>
        /// Adds a new entity asynchronously to the database context.
        /// </summary>
        /// <param name="item">The entity to add.</param>
        public async Task AddAsync(T item)
        =>  await _context.Set<T>().AddAsync(item);
        

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }

        /// <summary>
        /// Returns the total count of entities of type T.
        /// </summary>
        public async Task<int> Count() =>
            await _context.Set<T>().CountAsync();

        /// <summary>
        /// Gets all entities as a read-only list.
        /// </summary>
        public async Task<IReadOnlyList<T>> GetAllAsync() =>
            await _context.Set<T>().AsNoTracking().ToListAsync();

        /// <summary>
        /// Gets all entities including related data.
        /// </summary>
        /// <param name="includes">Navigation properties to include.</param>
        public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets a single entity by ID including related data.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="includes">Navigation properties to include.</param>
        public async Task<T> GetByIDAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Note: Assumes entity has a property named "ID"
            var entity = await query.FirstOrDefaultAsync(
                x => Microsoft.EntityFrameworkCore.EF.Property<int>(x, "ID") == id
            );

            return entity;
        }

        /// <summary>
        /// Gets a single entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        public async Task<T> GetByIDAsync(int id)
        => await _context.Set<T>().FindAsync(id);
            
        

        /// <summary>
        /// Marks the entity as modified for updating in the database.
        /// </summary>
        /// <param name="item">The entity to update.</param>
        public void Update(T item)
        => _context.Entry(item).State = EntityState.Modified;
        #endregion

    }
}
