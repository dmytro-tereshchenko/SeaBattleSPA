namespace SeaBattle.Lib.Managers
{
    public class ShipStorageUtility : IShipStorageUtility
    {
        private readonly int _price;

        public ShipStorageUtility(int price)
        {
            this._price = price;
        }

        public int CalculatePointCost(int size, int ShipTypeId = 1) => size * _price;
    }
}
