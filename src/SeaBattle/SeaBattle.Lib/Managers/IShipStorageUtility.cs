using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// Formation of ship indicators
    /// </summary>
    public interface IShipStorageUtility
    {
        /// <summary>
        /// Generate ship's cost.
        /// </summary>
        /// <param name="size">Ship's size</param>
        /// <param name="ShipTypeId">Id of <see cref="ShipType"/></param>
        /// <returns><see cref="int"/> Ship's cost</returns>
        int CalculatePointCost(int size, int ShipTypeId = 1);
    }
}
