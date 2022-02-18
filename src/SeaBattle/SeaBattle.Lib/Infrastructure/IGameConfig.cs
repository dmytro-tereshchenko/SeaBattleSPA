using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Infrastructure
{
    public interface IGameConfig
    {
        /// <summary>
        /// Min size X of <see cref="IGameField"/>
        /// </summary>
        ushort MinFieldSizeX { get; }

        /// <summary>
        /// Max size X of <see cref="IGameField"/>
        /// </summary>
        ushort MaxFieldSizeX { get; }

        /// <summary>
        /// Min size Y of <see cref="IGameField"/>
        /// </summary>
        ushort MinFieldSizeY { get; }

        /// <summary>
        /// Max size Y of <see cref="IGameField"/>
        /// </summary>
        ushort MaxFieldSizeY { get; }

        /// <summary>
        /// Max number of players in <see cref="IGame"/>
        /// </summary>
        byte MaxNumberOfPlayers { get; }
    }
}
