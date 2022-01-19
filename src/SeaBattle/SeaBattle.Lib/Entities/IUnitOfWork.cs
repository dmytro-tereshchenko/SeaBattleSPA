namespace SeaBattle.Lib.Entities
{
    public interface IUnitOfWork
    { 
        IRepository<IGame> Games { get; }

        IRepository<IGameField> GameFields { get; }

        IRepository<IStartField> StartFields { get; }

        IRepository<IGameShip> GameShips { get; }

        IRepository<IShip> Ships { get; }

        IRepository<IWeapon> Weapons { get; }

        IRepository<IRepair> Repairs { get; }

        IRepository<ITeam> Teams { get; }
    }
}