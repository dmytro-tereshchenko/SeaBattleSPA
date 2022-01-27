﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// Get actual game field
        /// </summary>
        /// <param name="player">The player who request game field</param>
        /// <param name="game">Current game</param>
        /// <returns><see cref="IResponseGameField"/></returns>
        IResponseGameField GetGameField(IGamePlayer player, IGame game);

        /// <summary>
        /// Remove <see cref="IGameShip"/> from <see cref="IGameField"/> to collection in <see cref="IStartField.Ships"/>
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="tPosX">X coordinate of removed ship</param>
        /// <param name="tPosY">Y coordinate of removed ship</param>
        /// <param name="startField">Start field with game field and collection of unused ships</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        StateCode TransferShipFromGameField(IPlayer player, ushort tPosX, ushort tPosY,
            IStartField startField);

        /// <summary>
        /// Put <see cref="IGameShip"/> from collection of <see cref="IStartField.Ships"/> to <see cref="IGameField"/>
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="tPosX">X coordinate of the ship's stern</param>
        /// <param name="tPosY">Y coordinate of the ship's stern</param>
        /// <param name="direction">The direction of placement ship</param>
        /// <param name="startField">Start field with game field and collection of unused ships</param>
        /// <param name="ship">Current ship</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        StateCode TransferShipToGameField(IGamePlayer player, ushort tPosX, ushort tPosY,
            DirectionOfShipPosition direction, IStartField startField, IGameShip ship);

        /// <summary>
        /// Move <see cref="IGameShip"/> from one position to another on <see cref="IGameField"/>
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Current ship</param>
        /// <param name="tPosX">X coordinate of new position of the ship's stern</param>
        /// <param name="tPosY">Y coordinate of new position of the ship's stern</param>
        /// <param name="direction">The direction of placement ship</param>
        /// <param name="field">Game field</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        StateCode MoveShip(IGamePlayer player, IGameShip ship, ushort tPosX, ushort tPosY,
            DirectionOfShipPosition direction, IGameField field);

        /// <summary>
        /// Get <see cref="ICollection{T}"/> of <see cref="IGameShip"/> on distance of action.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Current ship</param>
        /// <param name="field">Game field</param>
        /// <param name="action">Type of possible actions (<see cref="ActionType.Attack"/>, <see cref="ActionType.Repair"/>)</param>
        /// <returns><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGameShip"/></returns>
        /// <exception cref="ArgumentException">Wrong player</exception>
        /// <exception cref="InvalidEnumArgumentException">Used action not planned by the game</exception>
        ICollection<IGameShip> GetVisibleTargetsForShip(IGamePlayer player, IGameShip ship, IGameField field,
            ActionType action);

        /// <summary>
        /// Get in <see cref="string"/> format sorted collection of the ship by distance to the center of the field.
        /// </summary>
        /// <param name="field">Game field</param>
        /// <param name="player">Current player for filtering <see cref="IGameShip"/>, if null - get all ships without filter.</param>
        /// <returns><see cref="IDictionary{TKey,TValue}"/> whose generic key argument is <see cref="IGameShip"/>, generic type argument
        /// is <see cref="ICollection{T}"/> whose generic type argument is (<see cref="ushort"/>, <see cref="ushort"/>) coordinates (X,Y)</returns>
        IDictionary<IGameShip, ICollection<(ushort, ushort)>> GetFieldWithShips(IGameField field, IGamePlayer player = null);

        /// <summary>
        /// Attack cell by ship
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Current ship</param>
        /// <param name="tPosX">X coordinate of target cell</param>
        /// <param name="tPosY">Y coordinate of target cell</param>
        /// <param name="field">Game field</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        StateCode AttackShip(IGamePlayer player, IGameShip ship, ushort tPosX, ushort tPosY, IGameField field);

        /// <summary>
        /// Repair cell by ship
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Current ship</param>
        /// <param name="tPosX">X coordinate of target cell</param>
        /// <param name="tPosY">Y coordinate of target cell</param>
        /// <param name="field">Game field</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        StateCode RepairShip(IGamePlayer player, IGameShip ship, ushort tPosX, ushort tPosY, IGameField field);

        /// <summary>
        /// Repair all friendly ship on distance
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Current ship</param>
        /// <param name="field">Game field</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        StateCode RepairAllShip(IGamePlayer player, IGameShip ship, IGameField field);
    }
}