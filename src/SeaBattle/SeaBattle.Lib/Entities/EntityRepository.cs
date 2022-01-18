using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public class EntityRepository<T> : IRepository<T> where T : IEntity
    {
        private ICollection<T> _data;

        public EntityRepository()
        {
            _data = new List<T>();
        }

        public ICollection<T> GetAll() => _data;

        public T Get(uint id) => _data.FirstOrDefault(t => t.Id == id);

        public void Create(T item) => _data.Add(item);

        public void Update(T item)
        {
            foreach (var tempItem in _data)
            {
                if (tempItem.Id == item.Id)
                {
                    _data.Remove(tempItem);
                    _data.Add(item);
                    return;
                }
            }
            throw new ArgumentOutOfRangeException($"Not found {item} in data {this.ToString()}");
        }

        public void Delete(uint id)
        {
            foreach (var tempItem in _data)
            {
                if (tempItem.Id == id)
                {
                    _data.Remove(tempItem);
                    return;
                }
            }
            throw new ArgumentOutOfRangeException($"Not found item with {id} in data {this.ToString()}");
        }
    }
}
