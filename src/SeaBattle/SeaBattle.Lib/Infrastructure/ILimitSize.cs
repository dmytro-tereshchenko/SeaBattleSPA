using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Infrastructure
{
    /// <summary>
    /// Dto for getting border size of the game field (<see cref="IGameField"/>)
    /// </summary>
    public interface ILimitSize
    {
        /// <summary>
        /// Max size X of <see cref="IGameField"/>
        /// </summary>
        ushort MaxSizeX { get; set; }

        /// <summary>
        /// Max size Y of <see cref="IGameField"/>
        /// </summary>
        ushort MaxSizeY { get; set; }

        /// <summary>
        /// Min size X of <see cref="IGameField"/>
        /// </summary>
        ushort MinSizeX { get; set; }
        
        /// <summary>
        /// Min size Y of <see cref="IGameField"/>
        /// </summary>
        ushort MinSizeY { get; set; }
    }
}
