using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Infrastructure
{
    /// <summary>
    /// Dto for getting border size of the game field (<see cref="IGameField"/>)
    /// </summary>
    public class LimitSize : ILimitSize
    {
        /// <summary>
        /// Max size X of <see cref="IGameField"/>
        /// </summary>
        public ushort MaxSizeX { get; set; }

        /// <summary>
        /// Max size Y of <see cref="IGameField"/>
        /// </summary>
        public ushort MaxSizeY { get; set; }

        /// <summary>
        /// Min size X of <see cref="IGameField"/>
        /// </summary>
        public ushort MinSizeX { get; set; }

        /// <summary>
        /// Min size Y of <see cref="IGameField"/>
        /// </summary>
        public ushort MinSizeY { get; set; }

        public LimitSize(ushort maxSizeX, ushort maxSizeY, ushort minSizeX, ushort minSizeY)
        {
            MaxSizeX = maxSizeX;
            MaxSizeY = maxSizeY;
            MinSizeX = minSizeX;
            MinSizeY = minSizeY;
        }
    }
}
