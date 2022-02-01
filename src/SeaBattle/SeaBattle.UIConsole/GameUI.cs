using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Managers;
using SeaBattle.Lib.Responses;
using SeaBattle.UIConsole.Properties;

namespace SeaBattle.UIConsole
{
    /// <summary>
    /// Game "Sea battle"
    /// </summary>
    public class GameUI : IGameUI
    {
        private IGameManager _manager;

        private IPresenter _presenter;

        private ICollection<IGamePlayer> _players;

        public GameUI(IGameManager manager, IPresenter presenter)
        {
            _manager = manager;
            _presenter = presenter;
        }

        public void Start()
        {
            CreateGame();
            CreateGameField();
            CreatePlayers();
            CreateStartFields();
            GameProcessStart();
        }

        /// <summary>
        /// Create <see cref="IGame"/>
        /// </summary>
        private void CreateGame()
        {
            bool DataRetrieved = false;
            string message = Resources.InpNumPlayers;

            do
            {
                StateCode state = _manager.CreateGame(_presenter.GetData<byte>(message));

                switch (state)
                {
                    case StateCode.ErrorInitialization:
                        _presenter.ShowMessage(Resources.GameReadCr);
                        DataRetrieved = true;
                        break;
                    case StateCode.ExceededMaxNumberOfPlayers:
                        message =
                            $"{Resources.ExceededMaxNumberOfPlayers} {_manager.GetMaxNumberOfPlayers()}\n{Resources.InpNumPlayers}";
                        break;
                    case StateCode.Success:
                        DataRetrieved = true;
                        break;
                };

            } while (!DataRetrieved);
        }

        /// <summary>
        /// Create <see cref="IGameField"/>
        /// </summary>
        private void CreateGameField()
        {
            bool isGetData = true;
            string message = "";
            ushort sizeX, sizeY;

            LimitSize sizes = _manager.GetLimitSizeField();

            do
            {
                sizeX = _presenter.GetData<ushort>($"{message}{Resources.InputSizeX}");
                sizeY = _presenter.GetData<ushort>($"{message}{Resources.InputSizeY}");

                StateCode state = _manager.CreateGameField(sizeX, sizeY);

                switch (state)
                {
                    case StateCode.ErrorInitialization:
                        _presenter.ShowMessage(Resources.GameFieldCrOrNoGame);
                        isGetData = false;
                        break;
                    case StateCode.InvalidFieldSize:
                        message =
                            $"{Resources.SizeXBetween} {sizes.MinSizeX} {Resources.And} {sizes.MaxSizeX}\n" +
                            $"{Resources.SizeYBetween} {sizes.MinSizeY} {Resources.And} {sizes.MaxSizeY}\n";
                        break;
                    case StateCode.Success:
                        isGetData = false;
                        break;
                };
            } while (isGetData);
        }

        /// <summary>
        /// Create all <see cref="IGamePlayer"/> in the game
        /// </summary>
        private void CreatePlayers()
        {
            _players = new List<IGamePlayer>();

            for (int i = 0; i < _manager.GetMaxNumberOfPlayers(); i++)
            {
                IResponseGamePlayer response =
                    _manager.AddGamePlayer(_presenter.GetString($"{Resources.InpNamePlayers} {i + 1}"));

                if (response.State == StateCode.Success)
                {
                    _players.Add(response.Value);
                }
                else
                {
                    //all players was created StateCode.ExceededMaxNumberOfPlayers
                    return;
                }
            }
        }

        /// <summary>
        /// Create all <see cref="IStartField"/> in the game.
        /// </summary>
        private void CreateStartFields()
        {
            IResponseStartField response;

            foreach (var player in _players)
            {
                response = _manager.GetStartField(player);
                switch (response.State)
                {
                    case StateCode.Success:
                        InitializeField(response.Value);
                        break;
                    case StateCode.ExceededMaxNumberOfPlayers:
                        return;
                    case StateCode.InvalidPlayer:
                        continue;
                }
            }
        }

        /// <summary>
        /// Main process of game which consists of moves players (move ships, attack, repair)
        /// </summary>
        private void GameProcessStart()
        {
            StateCode state = StateCode.InvalidOperation;
            int choice = -1;
            IGameField gameField = null;
            string message = "";
            IGamePlayer player = null;
            IGameShip ship = null;

            while (state != StateCode.GameFinished)
            {
                state = StateCode.InvalidOperation;

                //move
                while (state != StateCode.GameFinished && state != StateCode.Success &&
                       state != StateCode.InvalidPlayer)
                {
                    player = _manager.CurrentGamePlayerMove;
                    gameField = _manager.GetGameField(player).Value;

                    //select ship or skip (escape)
                    var selectedShip = SelectShip(message);

                    if (!selectedShip.isSelected)
                    {
                        _manager.NextMove();
                        continue;
                    }
                    else
                    {
                        ship = selectedShip.Ship;
                    }

                    if (selectedShip.Ship == null || selectedShip.Ship.GamePlayer != player)
                    {
                        continue;
                    }

                    var selectedPlace =
                        SelectTarget(
                            $"{_presenter.GetShipStatus(ship)}\n{Resources.MovementPhase}. {Resources.SelectPlaceShip}");

                    if (!selectedPlace.isSelected)
                    {
                        continue;
                    }

                    choice = _presenter.MenuMultipleChoice(true, $"{Resources.ChooseDirection}:", () =>
                        {
                            _presenter.ShowGameField(gameField, _players);
                            _presenter.ShowMessage(_presenter.GetShipStatus(ship), false, false);
                            _presenter.ShowMessage($"{Resources.MovementPhase}. {Resources.Player}:", false, false, false);
                            _presenter.ShowMessage(player.Name, false, false, true,
                                (ConsoleColor) ((_players as IList<IGamePlayer>).IndexOf(player) + 1));
                        },
                        new string[]
                        {
                            Resources.Up, Resources.Right, Resources.Down, Resources.Left, Resources.Cancel
                        });

                    DirectionOfShipPosition direction = DirectionOfShipPosition.XDec; //case 0:

                    switch (choice)
                    {
                        case 1:
                            direction = DirectionOfShipPosition.YInc;
                            break;
                        case 2:
                            direction = DirectionOfShipPosition.XInc;
                            break;
                        case 3:
                            direction = DirectionOfShipPosition.YDec;
                            break;
                        case -1:
                        case 4:
                            state = StateCode.Success;
                            break;
                    }

                    if (choice is >= 0 and < 4)
                    {
                        state = _manager.PutShipOnField(player, ship, selectedPlace.X, selectedPlace.Y, direction);
                    }

                    message = InfoState(state);
                }

                state = StateCode.InvalidOperation;

                //action
                while (state != StateCode.GameFinished && state != StateCode.Success &&
                       state != StateCode.InvalidPlayer && state != StateCode.MissTarget &&
                       state != StateCode.TargetDestroyed)
                {
                    switch (SelectAction(ship, message))
                    {
                        case -1:
                            continue;
                        case 3:
                            state = StateCode.Success;
                            break;
                        case 0:
                            var selectedTargetAtack = SelectTarget($"{_presenter.GetShipStatus(ship)}\n{Resources.ActionPhase}. {Resources.SelectShipAttack}");

                            if (selectedTargetAtack.isSelected)
                            {
                                state = _manager.AttackShip(player, ship, selectedTargetAtack.X, selectedTargetAtack.Y);
                            }

                            break;
                        case 1:
                            var selectedTargetRepair = SelectTarget($"{_presenter.GetShipStatus(ship)}\n{Resources.ActionPhase}. {Resources.SelectShipRepair}");

                            if (selectedTargetRepair.isSelected)
                            {
                                state = _manager.RepairShip(player, ship, selectedTargetRepair.X, selectedTargetRepair.Y);
                            }

                            break;
                        case 2:
                            state=_manager.RepairAllShip(player, ship);
                            break;
                    }

                    message = InfoState(state);
                }

                _manager.NextMove();
            }

            _presenter.ShowGameField(gameField, _players);
            _presenter.ShowMessage($"{Resources.Winner}: {_manager.GetResultGame().Name}");
        }

        /// <summary>
        /// Create ships, allocate ships on the field for <see cref="IGamePlayer"/>
        /// </summary>
        /// <param name="startField">The field for storing the location of ships and points for buy ships by the player when initializing game.</param>
        private void InitializeField(IStartField startField)
        {
            int choice = 0;

            while (choice != -1)
            {
                List<string> options = startField.Ships.Select(ship =>
                        _presenter.GetShipStatus(ship))
                    .ToList();

                options.InsertRange(0, new string[] {Resources.Ready, Resources.BuyShip, Resources.RemoveShipFromField });

                choice = _presenter.MenuMultipleChoice(true, $"{Resources.ChooseOrBuyShip}:", () =>
                {
                    _presenter.ShowGameField(startField.GameField, _players, startField.FieldLabels,
                        startField.GamePlayer);
                    _presenter.ShowMessage(_presenter.GetPlayerStatus(startField), false, false);
                }, options.ToArray());

                switch (choice)
                {
                    case -1:
                    case 0:
                        //ready and exit
                        choice = -1;
                        break;
                    case 1:
                        BuyShip(startField);
                        break;
                    case 2:
                        RemoveShip(startField);
                        break;
                    default:
                        ShipManage(startField, startField.Ships.ElementAt(choice - 3));
                        break;
                }
            }

            _manager.ReadyPlayer(startField.GamePlayer);
        }

        /// <summary>
        /// Buy ship for points to list of ships in start field
        /// </summary>
        /// <param name="startField">The field for storing the location of ships and points for buy ships by the player when initializing game.</param>
        private void BuyShip(IStartField startField)
        {
            int choice = 0;
            string message = "";

            while (choice != -1)
            {
                ICollection<(ICommonShip, int)> commonShips = _manager.GetShips();

                List<string> options = commonShips.Select(ship =>
                        $"{ship.Item1.Type}, {Resources.Size}: {ship.Item1.Size}, {Resources.MaxHp}: {ship.Item1.MaxHp}, {Resources.Speed}: {ship.Item1.Speed}, {Resources.Cost}: {ship.Item2}")
                    .ToList();

                options.Insert(0, Resources.Exit);

                choice = _presenter.MenuMultipleChoice(true, $"{message}\n{Resources.ChooseOrBuyShip}:", () =>
                {
                    _presenter.ShowGameField(startField.GameField, _players, startField.FieldLabels, startField.GamePlayer);
                    _presenter.ShowMessage(_presenter.GetPlayerStatus(startField), false, false);
                }, options.ToArray());

                switch (choice)
                {
                    case -1:
                    case 0:
                        //ready and exit
                        choice = -1;
                        break;
                    default:
                        StateCode state = _manager.BuyShip(startField.GamePlayer, commonShips.ElementAt(choice - 1).Item1);
                        message = state switch
                        {
                            StateCode.Success => "",
                            StateCode.PointsShortage => $"{Resources.PointsShortage}\n",
                            _ => $"{Resources.SomeError}\n"
                        };
                        break;
                }
            }
        }

        /// <summary>
        /// Remove ship from game field to list of ships in start field
        /// </summary>
        /// <param name="startField">The field for storing the location of ships and points for buy ships by the player when initializing game.</param>
        private void RemoveShip(IStartField startField)
        {
            (ushort X, ushort Y, bool isSelected) point = _presenter.SelectCell(startField.GameField, _players,
                GetCenterOfStartField(startField),
                startField.FieldLabels,
                () =>
                {
                    _presenter.ShowMessage(_presenter.GetPlayerStatus(startField), false, false);
                    _presenter.ShowMessage(Resources.ChooseShipForRemove, false, false);
                }, startField.GamePlayer);

            if (point.isSelected)
            {
                _manager.RemoveShipFromField(startField.GamePlayer, point.X, point.Y);
            }
        }

        /// <summary>
        /// Manage ship during initializing.
        /// </summary>
        /// <param name="startField">The field for storing the location of ships and points for buy ships by the player when initializing game.</param>
        /// <param name="ship">Current ship</param>
        private void ShipManage(IStartField startField, IGameShip ship)
        {
            int choice = 0;
            while (choice != -1)
            {
                choice = _presenter.MenuMultipleChoice(true, $"{Resources.ChooseAction}:", () =>
                    {
                        _presenter.ShowGameField(startField.GameField, _players, startField.FieldLabels,
                            startField.GamePlayer);
                        _presenter.ShowMessage(_presenter.GetPlayerStatus(startField), false, false);
                        _presenter.ShowMessage(_presenter.GetShipStatus(ship), false, false);
                    },
                    new string[]
                    {
                        Resources.SetShipField, $"{Resources.Add} {Resources.Weapon}",
                        $"{Resources.Add} {Resources.RepairLCase}", $"{Resources.Remove} {Resources.Weapon}",
                        $"{Resources.Remove} {Resources.RepairLCase}",
                        Resources.SellShip, Resources.Exit
                    });

                switch (choice)
                {
                    case 6:
                    case -1:
                        //exit
                        choice = -1;
                        break;
                    case 0:
                        SetShip(startField, ship);
                        choice = -1;
                        break;
                    case 1:
                        _manager.AddWeapon(startField.GamePlayer, ship, ship.Weapons?.LastOrDefault());
                        break;
                    case 2:
                        _manager.AddRepair(startField.GamePlayer, ship, ship.Repairs?.LastOrDefault());
                        break;
                    case 3:
                        _manager.RemoveWeapon(startField.GamePlayer, ship, ship.Weapons?.LastOrDefault());
                        break;
                    case 4:
                        _manager.RemoveRepair(startField.GamePlayer, ship, ship.Repairs?.LastOrDefault());
                        break;
                    case 5:
                        _manager.SellShip(startField.GamePlayer, ship);
                        choice = -1;
                        break;
                }
            }
        }

        /// <summary>
        /// Put ship from list of ships in <see cref="IStartField"/> to <see cref="IGameField"/>
        /// </summary>
        /// <param name="startField">The field for storing the location of ships and points for buy ships by the player when initializing game.</param>
        /// <param name="ship">Current ship</param>
        private void SetShip(IStartField startField, IGameShip ship)
        {
            (ushort X, ushort Y, bool isSelected) point = _presenter.SelectCell(startField.GameField, _players,
                GetCenterOfStartField(startField),
                startField.FieldLabels,
                () =>
                {
                    _presenter.ShowMessage(_presenter.GetPlayerStatus(startField), false, false);
                    _presenter.ShowMessage(_presenter.GetShipStatus(ship), false, false);
                    _presenter.ShowMessage(Resources.SelectPosShipStern, false, false);
                }, startField.GamePlayer);

            if (point.isSelected)
            {
                int choice = _presenter.MenuMultipleChoice(true, $"{Resources.ChooseDirection}:", () =>
                    {
                        _presenter.ShowGameField(startField.GameField, _players, startField.FieldLabels,
                            startField.GamePlayer);
                        _presenter.ShowMessage(_presenter.GetPlayerStatus(startField), false, false);
                        _presenter.ShowMessage(_presenter.GetShipStatus(ship), false, false);
                    },
                    new string[]
                    {
                        Resources.Up, Resources.Right, Resources.Down, Resources.Left, Resources.Cancel
                    });

                DirectionOfShipPosition direction = DirectionOfShipPosition.XDec; //case 0:

                switch (choice)
                {
                    case 1:
                        direction = DirectionOfShipPosition.YInc;
                        break;
                    case 2:
                        direction = DirectionOfShipPosition.XInc;
                        break;
                    case 3:
                        direction = DirectionOfShipPosition.YDec;
                        break;
                }

                if (choice is >= 0 and < 4)
                {
                    _manager.PutShipOnField(startField.GamePlayer, ship, point.X, point.Y, direction);
                }
            }
        }

        /// <summary>
        /// Select action for ship
        /// </summary>
        /// <param name="ship">Selected ship</param>
        /// <returns><see cref="int"/> Number of action
        /// <para>-1, 3 - skip/quit</para><para>0 - attack</para><para>1 - repair</para><para>2 - repair all</para></returns>
        private int SelectAction(IGameShip ship, string message=null)
        {
            IGamePlayer player = _manager.CurrentGamePlayerMove;
            IGameField gameField = _manager.GetGameField(player).Value;

            return _presenter.MenuMultipleChoice(true, "", () =>
                {
                    _presenter.ShowGameField(gameField, _players);
                    _presenter.ShowMessage(_presenter.GetShipStatus(ship), false, false);
                    
                    if (string.IsNullOrEmpty(message))
                    {
                        _presenter.ShowMessage(message, false, false);
                    }

                    _presenter.ShowMessage($"{Resources.ActionPhase}. {Resources.Player}: ", false,
                        false, false);
                    _presenter.ShowMessage(player.Name, false, false,
                        false, (ConsoleColor) ((_players as IList<IGamePlayer>).IndexOf(player) + 1));
                    _presenter.ShowMessage($" {Resources.ChooseAction}:", false, false);
                },
                new string[]
                {
                    Resources.Attack, Resources.RepairUCase, $"{Resources.RepairUCase} {Resources.All}", Resources.Skip
                });
        }

        /// <summary>
        /// Select cell on game field
        /// </summary>
        /// <param name="message">Message for show</param>
        /// <returns>(<see cref="ushort"/> X, <see cref="ushort"/> Y, <see cref="bool"/> Select) where X, Y - selected coordinates, Select - true - isSelected, otherwise exit</returns>
        private (ushort X, ushort Y, bool isSelected) SelectTarget(string message = "")
        {
            IGamePlayer player = _manager.CurrentGamePlayerMove;
            IGameField gameField = _manager.GetGameField(player).Value;

            return _presenter.SelectCell(gameField, _players,
                new((ushort)(gameField.SizeX / 2), (ushort)(gameField.SizeY / 2)),
                null,
                () =>
                {
                    _presenter.ShowMessage($"{Resources.Player}: ", false, false, false);
                    _presenter.ShowMessage(player.Name, false,
                        false, true, (ConsoleColor)((_players as IList<IGamePlayer>).IndexOf(player) + 1));
                    _presenter.ShowMessage(message, false, false);
                });
        }

        

        /// <summary>
        /// Select ship on <see cref="IGameField"/>
        /// </summary>
        /// <param name="message">Message for show</param>
        /// <returns><see cref="IGameShip"/> otherwise null</returns>
        private (IGameShip Ship, bool isSelected) SelectShip(string message = "")
        {
            IGamePlayer player = _manager.CurrentGamePlayerMove;
            IGameField gameField = _manager.GetGameField(player).Value;

            (ushort X, ushort Y, bool isSelected) shipPoint = _presenter.SelectCell(gameField, _players,
                new((ushort) (gameField.SizeX / 2), (ushort) (gameField.SizeY / 2)),
                null,
                () =>
                {
                    _presenter.ShowMessage($"{Resources.Player}: ", false, false, false);
                    _presenter.ShowMessage(player.Name, false,
                        false, true, (ConsoleColor) ((_players as IList<IGamePlayer>).IndexOf(player) + 1));
                    _presenter.ShowMessage($"{message}\n{Resources.SelectShip}", false,
                        false);
                });

            return new(gameField[shipPoint.X, shipPoint.Y], shipPoint.isSelected);
        }

        /// <summary>
        /// Get coordinates of the center of the field for allocating player's ships
        /// </summary>
        /// <param name="startField">The field for storing the location of ships and points for buy ships by the player when initializing game.</param>
        /// <returns>(<see cref="ushort"/> X, <see cref="ushort"/> Y) Coordinates of center field for allocating player's ships</returns>
        private (ushort X, ushort Y) GetCenterOfStartField(IStartField startField)
        {
            ushort minX = (ushort) (startField.FieldLabels.GetLength(0) - 1);
            ushort minY = (ushort) (startField.FieldLabels.GetLength(1) - 1);

            ushort maxX = 0;
            ushort maxY = 0;

            for (ushort i = 0; i < startField.FieldLabels.GetLength(0); i++)
            {
                for (ushort j = 0; j < startField.FieldLabels.GetLength(1); j++)
                {
                    if (startField.FieldLabels[i, j])
                    {
                        minX = (i < minX) ? i : minX;
                        minY = (j < minY) ? j : minY;
                        maxX = (i > maxX) ? i : maxX;
                        maxY = (j > maxY) ? j : maxY;
                    }
                }
            }

            return new((ushort) ((minX + maxX) / 2), (ushort) ((minY + maxY) / 2));
        }

        /// <summary>
        /// Get message depends of <see cref="StateCode"/>
        /// </summary>
        /// <param name="code">State code of the game actions</param>
        /// <returns><see cref="string"/></returns>
        private string InfoState(StateCode code) => (code) switch
        {
            StateCode.InvalidPositionShip => Resources.InvalidPositionShip,
            StateCode.InvalidPlayer => Resources.InvalidPlayer,
            StateCode.InvalidShip => Resources.InvalidShip,
            StateCode.OutOfDistance => Resources.OutOfDistance,
            StateCode.MissTarget => Resources.MissTarget,
            StateCode.TargetDestroyed => Resources.TargetDestroyed,
            StateCode.GameFinished => Resources.GameFinished,
            StateCode.InvalidOperation => Resources.InvalidOperation,
            _ => ""
        };
    }
}