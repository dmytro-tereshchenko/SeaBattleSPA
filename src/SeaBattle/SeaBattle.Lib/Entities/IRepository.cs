using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public interface IRepository<T> where T : IEntity
    {
        ICollection<T> GetAll();
        T Get(uint id);
        void Create(T item);
        void Update(T item);
        void Delete(uint id);
    }
}
