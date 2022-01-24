using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Repositories;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// Unit of work, which consolidation data repositories of the domain.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Getting of repository of <see cref="IGame"/>
        /// </summary>
        /// <value><see cref="IRepository{T}"/> whose generic type argument is <see cref="IGame"/></value>
        IRepository<IGame> Games { get; }

        /// <summary>
        /// Getting of repository of <see cref="IGameField"/>
        /// </summary>
        /// <value><see cref="IRepository{T}"/> whose generic type argument is <see cref="IGameField"/></value>
        IRepository<IGameField> GameFields { get; }

        /// <summary>
        /// Getting of repository of <see cref="IStartField"/>
        /// </summary>
        /// <value><see cref="IRepository{T}"/> whose generic type argument is <see cref="IStartField"/></value>
        IRepository<IStartField> StartFields { get; }

        /// <summary>
        /// Getting of repository of <see cref="IGameShip"/>
        /// </summary>
        /// <value><see cref="IRepository{T}"/> whose generic type argument is <see cref="IGameShip"/></value>
        IRepository<IGameShip> GameShips { get; }

        /// <summary>
        /// Getting of repository of <see cref="IShip"/>
        /// </summary>
        /// <value><see cref="IRepository{T}"/> whose generic type argument is <see cref="IShip"/></value>
        IRepository<ICommonShip> Ships { get; }

        /// <summary>
        /// Getting of repository of <see cref="IWeapon"/>
        /// </summary>
        /// <value><see cref="IRepository{T}"/> whose generic type argument is <see cref="IWeapon"/></value>
        IRepository<IWeapon> Weapons { get; }

        /// <summary>
        /// Getting of repository of <see cref="IRepair"/>
        /// </summary>
        /// <value><see cref="IRepository{T}"/> whose generic type argument is <see cref="IRepair"/></value>
        IRepository<IRepair> Repairs { get; }

        /// <summary>
        /// Getting of repository of <see cref="IPlayer"/>
        /// </summary>
        /// <value><see cref="IRepository{T}"/> whose generic type argument is <see cref="IPlayer"/></value>
        IRepository<IPlayer> Players { get; }

        /// <summary>
        /// Command save to external data storage when we use ORM
        /// </summary>
        public void Save();

        /// <summary>
        /// Async command save to external data storage when we use ORM
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        public Task SaveAsync();

        /// <summary>
        /// Dispose of resources.
        /// </summary>
        /// <param name="disposing">Label <see cref="bool"/> for disposing</param>
        public void Dispose(bool disposing);

        /// <summary>
        /// Dispose of resources.
        /// </summary>
        public void Dispose();
    }
}
