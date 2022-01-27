﻿using System;
using System.Collections.Generic;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Responses;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// Manager which response for game flow of "Sea battle"
    /// </summary>
    public class GameManager : IGameManager
    {
        /// <summary>
        /// Manager which response for creating and initializing start entities.
        /// </summary>
        /// <value><see cref="IInitializeManager"/></value>
        private readonly IInitializeManager _initializeManager;

        /// <summary>
        /// Manager which response for creating and change ships.
        /// </summary>
        /// <value><see cref="IShipManager"/></value>
        private readonly IShipManager _shipManager;

        /// <summary>
        /// Manager which response for the actions and changes of ships on the field during the game.
        /// </summary>
        /// <value><see cref="IActionManager"/></value>
        private readonly IActionManager _actionManager;

        /// <summary>
        /// Current game
        /// </summary>
        /// <value><see cref="IGame"/></value>
        private IGame _game;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeManager"/> class
        /// </summary>
        /// <param name="factory">Abstract factory for initializing</param>
        public GameManager(IAbstractGameFactory factory)
        {
            _initializeManager = factory.GetInitializeManager();
            _shipManager = factory.GetShipManager();
            _actionManager = factory.GetActionManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeManager"/> class
        /// </summary>
        /// <param name="game">Current game</param>
        /// <param name="factory">Abstract factory for initializing</param>
        public GameManager(IAbstractGameFactory factory, IGame game) : this(factory) => _game = game;

        /// <summary>
        /// Create <see cref="IGame"/>
        /// </summary>
        /// <param name="numberOfPlayers">Number of players in <see cref="IGame"/></param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public StateCode CreateGame(byte numberOfPlayers)
        {
            if (_game != null)
            {
                return StateCode.ErrorInitialization;
            }

            try
            {
                _game = _initializeManager.CreateGame(numberOfPlayers);
            }
            catch (ArgumentOutOfRangeException)
            {
                return StateCode.ExceededMaxNumberOfPlayers;
            }

            return StateCode.Success;
        }

        /// <summary>
        /// Create <see cref="IGameField"/>
        /// </summary>
        /// <param name="sizeX">SizeX of <see cref="IGameField"/></param>
        /// <param name="sizeY">SizeY of <see cref="IGameField"/></param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public StateCode CreateGameField(ushort sizeX, ushort sizeY)
        {
            if (_game == null || _game.Field != null)
            {
                return StateCode.ErrorInitialization;
            }

            IResponseGameField response = _initializeManager.CreateGameField(sizeX, sizeY);
            
            if (response.State == StateCode.Success)
            {
                _game.Field = response.Value;
            }

            return response.State;
        }

        /// <summary>
        /// Add player to the game
        /// </summary>
        /// <param name="playerName">Player's name</param>
        /// <returns><see cref="IResponseGamePlayer"/></returns>
        public IResponseGamePlayer AddGamePlayer(string playerName) =>
            _initializeManager.AddPlayerToGame(_game, playerName);

        /// <summary>
        /// Get start field by player and game. In case absence of starting fields, create them.
        /// </summary>
        /// <param name="player">The player who request start field</param>
        /// <returns><see cref="IResponseStartField"/></returns>
        public IResponseStartField GetStartField(IGamePlayer player) => _initializeManager.GetStartField(_game, player);

        /// <summary>
        /// Get actual game field
        /// </summary>
        /// <param name="player">The player who request game field</param>
        /// <returns><see cref="IResponseGameField"/></returns>
        public IResponseGameField GetGameField(IGamePlayer player) => _actionManager.GetGameField(player, _game);


    }
}
