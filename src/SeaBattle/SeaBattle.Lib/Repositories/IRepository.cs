using SeaBattle.Lib.Entities;
using System.Collections.Generic;

namespace SeaBattle.Lib.Repositories
{
    /// <summary>
    /// Generic interface for a repository for storage Entities and CRUD-operations for manipulation of them
    /// </summary>
    /// <param name="T">Entity which implements IEntity.</param>
    public interface IRepository<T> where T : IEntity
    {
        /// <summary>
        /// Method for get all objects from repository.
        /// </summary>
        /// <param name="T">Entity which implements IEntity.</param>
        /// <returns>Returns collection of Entities (ICollection).</returns>
        ICollection<T> GetAll();

        /// <summary>
        /// Method for finding and getting object from the repository by id.
        /// </summary>
        /// <param name="id">Id of entities object.</param>
        /// <returns>Entities object which implements IEntity
        /// or a default value if the sequence contains no elements.</returns>
        T Get(uint id);

        /// <summary>
        /// Method for create and add object to the repository.
        /// </summary>
        /// <param name="item">Entities object which implements IEntity.</param>
        void Create(T item);

        /// <summary>
        /// Method for edit (update) object in the repository.
        /// </summary>
        /// <param name="item">Entities object which implements IEntity.</param>
        void Update(T item);

        /// <summary>
        /// Method for delete object from the repository.
        /// </summary>
        /// <param name="id">Id of entities object.</param>
        void Delete(uint id);
    }
}
