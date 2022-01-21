using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Repositories;
using System;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Managers
{
    public class UnitOfWork : IUnitOfWork
    { 
        private IRepository<IGame> _games;

        private IRepository<IGameField> _gameFields;

        private IRepository<IStartField> _startFields;

        private IRepository<IGameShip> _gameShips;

        private IRepository<IShip> _ships;

        private IRepository<IWeapon> _weapons;

        private IRepository<IRepair> _repairs;

        private IRepository<ITeam> _teams;

        private bool disposed = false;

        public IRepository<IGame> Games => _games ??= new EntityRepository<IGame>();

        public IRepository<IGameField> GameFields => _gameFields ??= new EntityRepository<IGameField>();

        public IRepository <IStartField> StartFields => _startFields ??= new EntityRepository<IStartField>();

        public IRepository <IGameShip> GameShips => _gameShips ??= new EntityRepository<IGameShip>();

        public IRepository<IShip> Ships => _ships ??= new EntityRepository<IShip>();

        public IRepository<IWeapon> Weapons => _weapons ??= new EntityRepository<IWeapon>();

        public IRepository<IRepair> Repairs => _repairs ??= new EntityRepository<IRepair>();

        public IRepository<ITeam> Teams => _teams ??= new EntityRepository<ITeam>();

        //Command save to external data storage when we use ORM
        public void Save() {}

        //Async command save to external data storage when we use ORM
        public async Task SaveAsync() => await Task.CompletedTask;

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
