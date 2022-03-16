using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Responses;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// The manager responsible for the actions and changes of ships on the field during the game.
    /// </summary>
    public interface IActionManager
    {
        /// <summary>
        /// Utility for actions on <see cref="IGameField"/>
        /// </summary>
        /// <value><see cref="IGameFieldActionUtility"/></value>
        IGameFieldActionUtility ActionUtility { get; }

        /// <summary>
        /// Remove <see cref="IGameShip"/> from <see cref="IGameField"/> to collection in <see cref="IStartField.Ships"/>
        /// </summary>
        /// <param name="playerName">Current player's name</param>
        /// <param name="shipId">Id of removed ship</param>
        /// <param name="startFieldId">Id of Start field with game field and collection of unused ships</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        Task<StateCode> TransferShipFromGameField(string playerName, int shipId, int startFieldId);

        /// <summary>
        /// Put <see cref="IGameShip"/> from collection of <see cref="IStartField.Ships"/> to <see cref="IGameField"/>
        /// </summary>
        /// <param name="playerName">Current player's name</param>
        /// <param name="tPosX">X coordinate of the ship's stern</param>
        /// <param name="tPosY">Y coordinate of the ship's stern</param>
        /// <param name="direction">The direction of placement ship</param>
        /// <param name="startFieldId">Id of Start field with game field and collection of unused ships</param>
        /// <param name="shipId">Id of current ship</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        Task<StateCode> TransferShipToGameField(string playerName, ushort tPosX, ushort tPosY,
            DirectionOfShipPosition direction, int startFieldId, int shipId);

        /// <summary>
        /// Move <see cref="IGameShip"/> from one position to another on <see cref="IGameField"/>
        /// </summary>
        /// <param name="playerName">Current player's name</param>
        /// <param name="gameShipId">Id of current ship</param>
        /// <param name="tPosX">X coordinate of new position of the ship's stern</param>
        /// <param name="tPosY">Y coordinate of new position of the ship's stern</param>
        /// <param name="direction">The direction of placement ship</param>
        /// <param name="gameFieldId">Id of game field</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        Task<StateCode> MoveShip(string playerName, int gameShipId, ushort tPosX, ushort tPosY,
            DirectionOfShipPosition direction, int gameFieldId);

        /// <summary>
        /// Get sorted collection of the ship by distance to the center of the field.
        /// </summary>
        /// <param name="field">Game field</param>
        /// <param name="player">Current player for filtering <see cref="IGameShip"/>, if null - get all ships without filter.</param>
        /// <returns><see cref="IDictionary{TKey,TValue}"/> whose generic key argument is <see cref="IGameShip"/>, generic type argument
        /// is <see cref="ICollection{T}"/> whose generic type argument is (<see cref="ushort"/>, <see cref="ushort"/>) coordinates (X,Y)</returns>
        IDictionary<IGameShip, ICollection<(ushort, ushort)>> GetFieldWithShips(IGameField field, IGamePlayer player = null);

        /// <summary>
        /// Attack cell by ship
        /// </summary>
        /// <param name="playerName">Current player's name</param>
        /// <param name="gameShipId">Id of current ship</param>
        /// <param name="tPosX">X coordinate of target cell</param>
        /// <param name="tPosY">Y coordinate of target cell</param>
        /// <param name="gameFieldId">Id of Game field</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        Task<StateCode> AttackShip(string playerName, int gameShipId, ushort tPosX, ushort tPosY, int gameFieldId);

        /// <summary>
        /// Repair cell by ship
        /// </summary>
        /// <param name="playerName">Current player's name</param>
        /// <param name="ship">Current ship</param>
        /// <param name="tPosX">X coordinate of target cell</param>
        /// <param name="tPosY">Y coordinate of target cell</param>
        /// <param name="gameFieldId">Id of Game field</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        Task<StateCode> RepairShip(string playerName, int gameShipId, ushort tPosX, ushort tPosY, int gameFieldId);

        /// <summary>
        /// Repair all friendly ship on distance
        /// </summary>
        /// <param name="playerName">Current player's name</param>
        /// <param name="gameShipId">Id of current ship</param>
        /// <param name="gameFieldId">Id of Game field</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        Task<StateCode> RepairAllShip(string playerName, int gameShipId, int gameFieldId);
    }
}
