using System;
using SeaBattle.Lib.Entities;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Repositories
{
    /// <summary>
    /// Generic interface for a repository for storage Entities and CRUD-operations for manipulation of them
    /// </summary>
    /// <param name="TEntity">Entity of database</param>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get all objects from repository.
        /// </summary>
        /// <param name="TEntity">Entity which implements <see cref="IEntity"/>.</param>
        /// <returns>Returns collection of Entities (ICollection).</returns>
        ICollection<TEntity> GetAll();

        /// <summary>
        /// Async get all objects from repository.
        /// </summary>
        /// <param name="TEntity">Entity which implements <see cref="IEntity"/>.</param>
        /// <returns>Returns collection of Entities (ICollection).</returns>
        Task<ICollection<TEntity>> GetAllAsync();

        /// <summary>
        /// Find and get object from the repository by id.
        /// </summary>
        /// <param name="id">Id of entities object.</param>
        /// <returns>Entities object or a default value if the sequence contains no elements.</returns>
        TEntity FindById(int id);

        /// <summary>
        /// Async find and get object from the repository by id.
        /// </summary>
        /// <param name="id">Id of entities object.</param>
        /// <returns>Entities object or a default value if the sequence contains no elements.</returns>
        ValueTask<TEntity> FindByIdAsync(int id);

        /// <summary>
        /// Get entities with predicate expression.
        /// </summary>
        /// <param name="predicate"><see cref="Expression{TDelegate}"/> whose generic type argument is <see cref="Func{TEntity, bool}"/>
        /// whose (<see cref="TEntity"/>, <see cref="bool"/>). Expression to filter <see cref="TEntity"/>.</param>
        /// <returns></returns>
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Async get entities with predicate expression.
        /// </summary>
        /// <param name="predicate"><see cref="Expression{TDelegate}"/> whose generic type argument is <see cref="Func{TEntity, bool}"/>
        /// whose (<see cref="TEntity"/>, <see cref="bool"/>). Expression to filter <see cref="TEntity"/>.</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Create and add object to the repository.
        /// </summary>
        /// <param name="item">Entities object which implements <see cref="IEntity"/>.</param>
        /// <returns><see cref="TEntity"/> created entity.</returns>
        TEntity Create(TEntity item);

        /// <summary>
        /// Async create and add object to the repository.
        /// </summary>
        /// <param name="item">Entities object which implements <see cref="IEntity"/>.</param>
        /// <returns><see cref="TEntity"/> created entity.</returns>
        Task<TEntity> CreateAsync(TEntity item);

        /// <summary>
        /// Edit (update) object in the repository.
        /// </summary>
        /// <param name="item">Entities object which implements <see cref="IEntity"/>.</param>
        /// <returns><see cref="TEntity"/> updated entity.</returns>
        TEntity Update(TEntity item);

        /// <summary>
        /// Async edit (update) object in the repository.
        /// </summary>
        /// <param name="item">Entities object which implements <see cref="IEntity"/>.</param>
        /// <returns><see cref="TEntity"/> updated entity.</returns>
        Task<TEntity> UpdateAsync(TEntity item);

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
        TEntity Update(Expression<Func<TEntity, bool>> filter,
            IEnumerable<object> updatedSet,
            IEnumerable<object> availableSet,
            string propertyName);

        /// <summary>
        /// Async update <see cref="TEntity"/> whose have many-to-many relationships.
        /// </summary>
        /// <param name="filter"><see cref="Expression{TDelegate}"/> whose generic type argument is <see cref="Func{TEntity, bool}"/>
        /// whose (<see cref="TEntity"/>, <see cref="bool"/>). Expression to filter target <see cref="TEntity"/></param>
        /// <param name="updatedSet"><see cref="IEnumerable{T}"/> whose generic type argument is <see cref="object"/>.
        /// Set of updated many-to-many relationships.</param>
        /// <param name="availableSet"><see cref="IEnumerable{T}"/> whose generic type argument is <see cref="object"/>.
        /// Full set of lookup collection which we update with target <see cref="TEntity"/>.</param>
        /// <param name="propertyName"><see cref="string"/> The name of the navigation property.</param>
        /// <returns><see cref="TEntity"/> updated entity.</returns>
        Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> filter,
            IEnumerable<object> updatedSet,
            IEnumerable<object> availableSet,
            string propertyName);

        /// <summary>
        /// Delete object from the repository.
        /// </summary>
        /// <param name="item">Entities object which implements <see cref="IEntity"/>.</param>
        /// <returns><see cref="TEntity"/> deleted entity.</returns>
        TEntity Delete(TEntity item);

        /// <summary>
        /// Async delete object from the repository.
        /// </summary>
        /// <param name="item">Entities object which implements <see cref="IEntity"/>.</param>
        /// <returns><see cref="TEntity"/> deleted entity.</returns>
        Task<TEntity> DeleteAsync(TEntity item);

        /// <summary>
        /// Get all <see cref="TEntity"/> with included relation entities.
        /// </summary>
        /// <param name="includeProperties"><see cref="Expression{TDelegate}"/>
        /// whose generic type argument is <see cref="Func{TEntity, bool}"/> whose (<see cref="TEntity"/>, <see cref="object"/>).
        /// Collection (params) whose we get with target <see cref="TEntity"/>.</param>
        /// <returns><see cref="IEnumerable{T}"/> whose generic type argument is <see cref="TEntity"/> Collection of <see cref="TEntity"/>.</returns>
        IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Async get all <see cref="TEntity"/> with included relation entities.
        /// </summary>
        /// <param name="includeProperties"><see cref="Expression{TDelegate}"/>
        /// whose generic type argument is <see cref="Func{TEntity, bool}"/> whose (<see cref="TEntity"/>, <see cref="object"/>).
        /// Collection (params) whose we get with target <see cref="TEntity"/>.</param>
        /// <returns><see cref="IEnumerable{T}"/> whose generic type argument is <see cref="TEntity"/> Collection of <see cref="TEntity"/>.</returns>
        Task<IEnumerable<TEntity>> GetWithIncludeAsync(params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Get <see cref="TEntity"/> with included relation entities end filtered by predicate.
        /// </summary>
        /// <param name="predicate"><see cref="Expression{TDelegate}"/> whose generic type argument is <see cref="Func{TEntity, bool}"/>
        /// whose (<see cref="TEntity"/>, <see cref="bool"/>). Expression to filter target <see cref="TEntity"/>.</param>
        /// <param name="includeProperties"><see cref="Expression{TDelegate}"/>
        /// whose generic type argument is <see cref="Func{TEntity, bool}"/> whose (<see cref="TEntity"/>, <see cref="object"/>).
        /// Collection (params) whose we get with target <see cref="TEntity"/>.</param>
        /// <returns><see cref="IEnumerable{T}"/> whose generic type argument is <see cref="TEntity"/> Collection of <see cref="TEntity"/>.</returns>
        IEnumerable<TEntity> GetWithInclude(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Async get <see cref="TEntity"/> with included relation entities end filtered by predicate.
        /// </summary>
        /// <param name="predicate"><see cref="Expression{TDelegate}"/> whose generic type argument is <see cref="Func{TEntity, bool}"/>
        /// whose (<see cref="TEntity"/>, <see cref="bool"/>). Expression to filter target <see cref="TEntity"/>.</param>
        /// <param name="includeProperties"><see cref="Expression{TDelegate}"/>
        /// whose generic type argument is <see cref="Func{TEntity, bool}"/> whose (<see cref="TEntity"/>, <see cref="object"/>).
        /// Collection (params) whose we get with target <see cref="TEntity"/>.</param>
        /// <returns><see cref="IEnumerable{T}"/> whose generic type argument is <see cref="TEntity"/> Collection of <see cref="TEntity"/>.</returns>
        Task<IEnumerable<TEntity>> GetWithIncludeAsync(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);
    }
}

