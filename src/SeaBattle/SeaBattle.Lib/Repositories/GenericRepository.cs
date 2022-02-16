using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SeaBattle.Lib.Repositories
{
    /// <summary>
    /// Generic class for a repository for storage Entities and CRUD-operations for manipulation of them
    /// Implements generic interface IRepository
    /// </summary>
    /// <param name="TEntity">Entity of database</param>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Data context for EF
        /// </summary>
        private DbContext _context;

        /// <summary>
        /// Data from database for <see cref="TEntity"/>
        /// </summary>
        private DbSet<TEntity> _dbSet;

        /// <summary>
        /// Default constructor that initializes an empty object of collection. 
        /// </summary>
        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual ICollection<TEntity> GetAll() => _dbSet.AsNoTracking().ToList();

        public virtual TEntity FindById(uint id) => _dbSet.Find(id);

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToList();
        }

        public virtual TEntity Create(TEntity item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();

            return item;
        }

        public virtual async Task<TEntity> CreateAsync(TEntity item)
        {
            _dbSet.Add(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public virtual TEntity Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();

            return item;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return item;
        }

        public virtual TEntity Update(Expression<Func<TEntity, bool>> filter,
            IEnumerable<object> updatedSet, // Updated many-to-many relationships
            IEnumerable<object> availableSet, // Lookup collection
            string propertyName) // The name of the navigation property
        {
            TEntity item = UpdateEntity(filter, updatedSet, availableSet, propertyName);

            _context.SaveChanges();

            return item;
        }

        public virtual async Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> filter,
            IEnumerable<object> updatedSet, // Updated many-to-many relationships
            IEnumerable<object> availableSet, // Lookup collection
            string propertyName) // The name of the navigation property
        {
            TEntity item = UpdateEntity(filter, updatedSet, availableSet, propertyName);

            await _context.SaveChangesAsync();

            return item;
        }

        public virtual TEntity Delete(TEntity item)
        {
            _dbSet.Remove(item);

            _context.SaveChanges();

            return item;
        }

        public virtual async Task<TEntity> DeleteAsync(TEntity item)
        {
            _dbSet.Remove(item);

            await _context.SaveChangesAsync();

            return item;
        }

        public virtual IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Include(includeProperties).ToList();
        }

        public virtual IEnumerable<TEntity> GetWithInclude(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.Where(predicate).ToList();
        }

        /// <summary>
        /// Create <see cref="IList"/> by <see cref="Type"/>
        /// </summary>
        /// <param name="type"><see cref="Type"/> of list's elements.</param>
        /// <returns><see cref="IList{Type}"/></returns>
        private IList CreateList(Type type)
        {
            var genericList = typeof(List<>).MakeGenericType(type);
            return (IList)Activator.CreateInstance(genericList);
        }

        /// <summary>
        /// Download related data
        /// </summary>
        /// <param name="includeProperties"><see cref="Expression{TDelegate}"/> whose generic type argument is <see cref="Func{TEntity, object}"/> included properties to main entity.</param>
        /// <returns><see cref="IQueryable{TEntity}"/> query to database with loading data.</returns>
        private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        /// <summary>
        /// Update <see cref="TEntity"/> whose have many-to-many relationships.
        /// </summary>
        /// <param name="filter"><see cref="Expression{TDelegate}"/> whose generic type argument is <see cref="Func{TEntity, bool}"/>
        /// whose (<see cref="TEntity"/>, <see cref="bool"/>). Expression to filter target <see cref="TEntity"/></param>
        /// <param name="updatedSet"><see cref="IEnumerable{T}"/> whose generic type argument is <see cref="object"/>.
        /// Set of updated many-to-many relationships.</param>
        /// <param name="availableSet"><see cref="IEnumerable{T}"/> whose generic type argument is <see cref="object"/>.
        /// Full set of lookup collection which we update with target <see cref="TEntity"/>.</param>
        /// <param name="propertyName"><see cref="string"/> The name of the navigation property.</param>
        /// <returns><see cref="TEntity"/> updated entity.</returns>
        private TEntity UpdateEntity(Expression<Func<TEntity, bool>> filter,
            IEnumerable<object> updatedSet, // Updated many-to-many relationships
            IEnumerable<object> availableSet, // Lookup collection
            string propertyName) // The name of the navigation property
        {
            // Get the generic type of the set
            var type = updatedSet.GetType().GetGenericArguments()[0];

            // Get the previous entity from the database based on repository type
            var previous = _context
                .Set<TEntity>()
                .Include(propertyName)
                .FirstOrDefault(filter);

            /* Create a container that will hold the values of
                * the generic many-to-many relationships we are updating.
                */
            var values = CreateList(type);

            /* For each object in the updated set find the existing
                 * entity in the database. This is to avoid Entity Framework
                 * from creating new objects or throwing an
                 * error because the object is already attached.
                 */
            foreach (var entry in updatedSet
                         .Select(obj => (int)obj
                             .GetType()
                             .GetProperty("Id")
                             .GetValue(obj, null))
                         .Select(value => _context.Set<TEntity>(type.Name).Find(value)))
            {
                values.Add(entry);
            }

            /* Get the collection where the previous many to many relationships
                * are stored and assign the new ones.
                */
            _context.Entry(previous).Collection(propertyName).CurrentValue = values;

            return previous;

            /*Example:
            GameRepository<User> _gameRepository = new GameRepository<User>(new GameContext());
            User user = new User();
            _gameRepository.Update(u => u.UserId == user.UserId,
                user.BusinessModels, // Many-to-many relationship to update
                _gameRepository.Get(), // Full set
                "BusinessModels"); // Property name*/
        }
    }
}
