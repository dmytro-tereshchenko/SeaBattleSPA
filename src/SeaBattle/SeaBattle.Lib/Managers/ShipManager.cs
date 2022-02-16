using SeaBattle.Lib.Entities;
using System.Collections.Generic;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// Manager which response for creating and change ships, implements <see cref="IShipManager"/>.
    /// </summary>
    public class ShipManager : IShipManager
    {
        public ShipManager() { }

        public StateCode BuyShip(ICollection<GamePlayer> players, GameShip gameShip, StartField startField)
        {
            if (!players.Contains(startField.GamePlayer))
            {
                return StateCode.InvalidPlayer;
            }

            if (gameShip.Points > startField.Points)
            {
                return StateCode.PointsShortage;
            }

            startField.GameShips.Add(gameShip);
            startField.Points -= gameShip.Points;


            return StateCode.Success;
        }

        public StateCode SellShip(ICollection<GamePlayer> players, GameShip gameShip, StartField startField)
        {
            if (!players.Contains(startField.GamePlayer))
            {
                return StateCode.InvalidPlayer;
            }

            startField.GameShips.Remove(gameShip);
            startField.Points += gameShip.Points;

            return StateCode.Success;
        }

       /* public ICollection<(IShip, int)> GetShips() => Ships;

        public ICollection<IRepair> GetRepairs() => Repairs;

        public ICollection<IWeapon> GetWeapons() => Weapons;

        public IGameShip GetNewShip(GamePlayer gamePlayer, Ship ship)
        {
            IGameShip gameShip = new GameShip(ship, gamePlayer, GetShipCost(ship.Size));
            switch (ship.Type)
            {
                case ShipType.Military:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        AddWeapon(gamePlayer, gameShip, Weapons.First());
                    }
                    break;
                case ShipType.Auxiliary:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        AddRepair(gamePlayer, gameShip, Repairs.First());
                    }
                    break;
            }

            return gameShip;
        }

        public StateCode AddWeapon(IGamePlayer gamePlayer, IGameShip gameShip, IWeapon weapon)
        {
            if (gameShip.GamePlayer != gamePlayer)
            {
                return StateCode.InvalidPlayer;
            }

            if (gameShip.Weapons.Count + gameShip.Repairs.Count == gameShip.Size)
            {
                return StateCode.LimitEquipment;
            }

            if (gameShip.Ship.Type == ShipType.Auxiliary)
            {
                return StateCode.InvalidEquipment;
            }

            gameShip.Weapons.Add(weapon);

            return StateCode.Success;
        }

        public StateCode AddRepair(IGamePlayer gamePlayer, IGameShip gameShip, IRepair repair)
        {
            if (gameShip.GamePlayer != gamePlayer)
            {
                return StateCode.InvalidPlayer;
            }

            if (gameShip.Weapons.Count + gameShip.Repairs.Count == gameShip.Size)
            {
                return StateCode.LimitEquipment;
            }

            if (gameShip.Ship.Type == ShipType.Military)
            {
                return StateCode.InvalidEquipment;
            }

            gameShip.Repairs.Add(repair);

            return StateCode.Success;
        }*/

        public StateCode RemoveWeapon(IGamePlayer gamePlayer, IGameShip gameShip, IWeapon weapon)
        {
            if (gameShip.GamePlayer != gamePlayer)
            {
                return StateCode.InvalidPlayer;
            }

            return gameShip.Weapons.Remove(weapon as Weapon) ? StateCode.Success : StateCode.InvalidEquipment;
        }

        public StateCode RemoveRepair(IGamePlayer gamePlayer, IGameShip gameShip, IRepair repair)
        {
            if (gameShip.GamePlayer != gamePlayer)
            {
                return StateCode.InvalidPlayer;
            }

            return gameShip.Repairs.Remove(repair as Repair) ? StateCode.Success : StateCode.InvalidEquipment;
        }
    }
}
