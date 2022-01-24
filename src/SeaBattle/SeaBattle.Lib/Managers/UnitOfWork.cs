using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Repositories;
using System;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// Unit of work, which consolidation data repositories of the domain.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    { 
        private IRepository<IGame> _games;

        private IRepository<IGameField> _gameFields;

        private IRepository<IStartField> _startFields;

        private IRepository<IGameShip> _gameShips;

        private IRepository<IShip> _ships;

        private IRepository<IWeapon> _weapons;

        private IRepository<IRepair> _repairs;

        private IRepository<IPlayer> _teams;

        private bool disposed = false;

        /// <summary>
        /// Getting of repository of <see cref="IGame"/>
        /// </summary>
        /// <value><see cref="IRepository{T}"/> whose generic type argument is <see cref="IGame"/></value>
        public IRepository<IGame> Games => _games ??= new EntityRepository<IGame>();

        /// <summary>
        /// Getting of repository of <see cref="IGameField"/>
        /// </summary>
        /// <value><see cref="IRepository{T}"/> whose generic type argument is <see cref="IGameField"/></value>
        public IRepository<IGameField> GameFields => _gameFields ??= new EntityRepository<IGameField>();

        /// <summary>
        /// Getting of repository of <see cref="IStartField"/>
        /// </summary>
        /// <value><see cref="IRepository{T}"/> whose generic type argument is <see cref="IStartField"/></value>
        public IRepository <IStartField> StartFields => _startFields ??= new EntityRepository<IStartField>();

        /// <summary>
        /// Getting of repository of <see cref="IGameShip"/>
        /// </summary>
        /// <value><see cref="IRepository{T}"/> whose generic type argument is <see cref="IGameShip"/></value>
        public IRepository <IGameShip> GameShips => _gameShips ??= new EntityRepository<IGameShip>();

        /// <summary>
        /// Getting of repository of <see cref="IShip"/>
        /// </summary>
        /// <value><see cref="IRepository{T}"/> whose generic type argument is <see cref="IShip"/></value>
        public IRepository<IShip> Ships => _ships ??= new EntityRepository<IShip>();

        /// <summary>
        /// Getting of repository of <see cref="IWeapon"/>
        /// </summary>
        /// <value><see cref="IRepository{T}"/> whose generic type argument is <see cref="IWeapon"/></value>
        public IRepository<IWeapon> Weapons => _weapons ??= new EntityRepository<IWeapon>();

        /// <summary>
        /// Getting of repository of <see cref="IRepair"/>
        /// </summary>
        /// <value><see cref="IRepository{T}"/> whose generic type argument is <see cref="IRepair"/></value>
        public IRepository<IRepair> Repairs => _repairs ??= new EntityRepository<IRepair>();

        /// <summary>
        /// Getting of repository of <see cref="IPlayer"/>
        /// </summary>
        /// <value><see cref="IRepository{T}"/> whose generic type argument is <see cref="IPlayer"/></value>
        public IRepository<IPlayer> Teams => _teams ??= new EntityRepository<IPlayer>();

        /// <summary>
        /// Command save to external data storage when we use ORM
        /// </summary>
        public void Save() {}

        /// <summary>
        /// Async command save to external data storage when we use ORM
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        public async Task SaveAsync() => await Task.CompletedTask;

        /// <summary>
        /// Dispose of resources.
        /// </summary>
        /// <param name="disposing">Label <see cref="bool"/> for disposing</param>
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //Dispose objects .Dispose()
                    _games = null;
                    _gameFields = null;
                    _startFields = null;
                    _gameShips = null;
                    _ships = null;
                    _weapons = null;
                    _repairs = null;
                    _teams = null;
                }
                this.disposed = true;
            }
        }

        /// <summary>
        /// Dispose of resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
