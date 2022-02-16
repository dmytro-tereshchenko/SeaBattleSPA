namespace SeaBattle.Lib.Managers
{
    public class ShipStorageUtility : IShipStorageUtility
    {
        private readonly int price;

        public ShipStorageUtility()
        {
            price = 1000;
        }

        public int CalculatePointCost(int size, int ShipTypeId) => size * price;
    }
}
