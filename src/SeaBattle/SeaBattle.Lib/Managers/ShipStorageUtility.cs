using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Managers
{
    public class ShipStorageUtility : IShipStorageUtility
    {
        private readonly int _price;

        public ShipStorageUtility(int price)
        {
            this._price = price;
        }

        public int CalculatePointCost(int size, ShipType ShipType = ShipType.Military) => size * _price;
    }
}
