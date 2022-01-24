using SeaBattle.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeaBattle.Lib.Repositories
{
    /// <summary>
    /// Generic class for a repository for storage Entities and CRUD-operations for manipulation of them
    /// Implements generic interface IRepository
    /// </summary>
    /// <param name="T">Entity which implements IEntity.</param>
    public class EntityRepository<T> : IRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Generic private collection for storing data
        /// </summary>
        /// <param name="T">Entity which implements <see cref="IEntity"/>.</param>
        private ICollection<T> _data;

        /// <summary>
        /// Default constructor that initializes an empty object of collection. 
        /// </summary>
        public EntityRepository()
        {
            _data = new List<T>();
        }

        /// <summary>
        /// Method for get all objects from repository.
        /// </summary>
        /// <param name="T">Entity which implements <see cref="IEntity"/>.</param>
        /// <returns>Returns collection of Entities (ICollection).</returns>
        public ICollection<T> GetAll() => _data;

        /// <summary>
        /// Method for finding and getting object from the repository by id.
        /// </summary>
        /// <param name="id">Id of entities object.</param>
        /// <returns>Entities object which implements <see cref="IEntity"/>
        /// or a default value if the sequence contains no elements.</returns>
        public T Get(uint id) => _data.FirstOrDefault(t => t.Id == id);

        /// <summary>
        /// Method for create and add object to the repository.
        /// </summary>
        /// <param name="item">Entities object which implements <see cref="IEntity"/>.</param>
        /// <returns><paramref name="item"/> created entity, otherwise null (there is no data for the given id)</returns>
        public T Create(T item)
        {
            if (_data.Count == 0)
            {
                //If _data is empty then its first element
                item.Id = 1u;
            }
            else
            {
                item.Id = _data.Max(i => i.Id) + 1;
            }

            //Set new value Id to item
            _data.Add(item);

            return item;
        }

        /// <summary>
        /// Method for edit (update) object in the repository.
        /// </summary>
        /// <param name="item">Entities object which implements <see cref="IEntity"/>.</param>
        /// <returns><paramref name="item"/> deleted entity, otherwise null (there is no data for the given id)</returns>
        public T Update(T item)
        {
            foreach (var tempItem in _data)
            {
                if (tempItem.Id == item.Id)
                {
                    _data.Remove(tempItem);
                    _data.Add(item);
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Method for delete object from the repository.
        /// </summary>
        /// <param name="id">Id of entities object.</param>
        public T Delete(uint id)
        {
            foreach (var tempItem in _data)
            {
                if (tempItem.Id == id)
                {
                    _data.Remove(tempItem);
                    return tempItem;
                }
            }

            return null;
        }
    }
}
