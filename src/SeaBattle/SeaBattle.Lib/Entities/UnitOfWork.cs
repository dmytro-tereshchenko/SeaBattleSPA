using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
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

        public IRepository<IGame> Games { get => _games; }
        
        public IRepository<IGameField> GameFields { get => _gameFields; }

        public IRepository <IStartField> StartFields { get => _startFields; }

        public IRepository <IGameShip> GameShips { get => _gameShips; }

        public IRepository<IShip> Ships { get => _ships; }

        public IRepository<IWeapon> Weapons { get => _weapons; }

        public IRepository<IRepair> Repairs { get => _repairs; }

        public IRepository<ITeam> Teams { get => _teams; }

        //command save to external data storage when we use ORM
        public void Save() { }

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
