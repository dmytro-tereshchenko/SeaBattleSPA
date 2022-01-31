using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Managers;
using SeaBattle.Lib.Responses;

namespace SeaBattle.UIConsole
{
    public class GameUI
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
            /*CreateGame();
            CreateGameField();
            CreatePlayers();*/

            //testing
            _manager.CreateGame(2);
            _manager.CreateGameField(10, 10);
            _players = new List<IGamePlayer>();
            _players.Add(_manager.AddGamePlayer("player 1").Value);
            _players.Add(_manager.AddGamePlayer("player 2").Value);

            CreateStartFields();
            
            Console.ReadKey();
        }

        private void CreateGame()
        {
            bool isGetData = true;
            string message = "Input number of players.";

            do
            {
                StateCode state = _manager.CreateGame(_presenter.GetData<byte>(message));

                switch (state)
                {
                    case StateCode.ErrorInitialization:
                        _presenter.ShowMessage("Game already created");
                        isGetData = false;
                        break;
                    case StateCode.ExceededMaxNumberOfPlayers:
                        message =
                            $"The number of players should be equal to or less than {_manager.GetMaxNumberOfPlayers()}\nInput number of players.";
                        break;
                    case StateCode.Success:
                        isGetData = false;
                        break;
                };

            } while (isGetData);
        }

        private void CreateGameField()
        {
            bool isGetData = true;
            string message = "";
            ushort sizeX, sizeY;
            LimitSize sizes = _manager.GetLimitSizeField();

            do
            {
                sizeX = _presenter.GetData<ushort>(message + "Input sizeX");
                sizeY = _presenter.GetData<ushort>(message + "Input sizeY");

                StateCode state = _manager.CreateGameField(sizeX, sizeY);

                switch (state)
                {
                    case StateCode.ErrorInitialization:
                        _presenter.ShowMessage("Game field already created or no game yet");
                        isGetData = false;
                        break;
                    case StateCode.InvalidFieldSize:
                        message =
                            $"Size X should be between {sizes.MinSizeX} and {sizes.MaxSizeX}\n" +
                            $"Size Y should be between {sizes.MinSizeY} and {sizes.MaxSizeY}\n";
                        break;
                    case StateCode.Success:
                        isGetData = false;
                        break;
                };

            } while (isGetData);
        }

        private void CreatePlayers()
        {
            _players = new List<IGamePlayer>();

            for (int i = 0; i < _manager.GetMaxNumberOfPlayers(); i++)
            {
                IResponseGamePlayer response =
                    _manager.AddGamePlayer(_presenter.GetString($"Input name for player {i + 1}"));

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

        private void InitializeField(IStartField startField)
        {
            //testing
            IGameShip ship = new GameShip(new Ship(ShipType.Military, 2, 100, 10), _players.ElementAt(0), 50);
            IGameShip ship2 = new GameShip(new Ship(ShipType.Military, 2, 100, 10), _players.ElementAt(1), 50);
            startField.GameField[2, 2] = ship;
            startField.GameField[2, 3] = ship;
            startField.GameField[9, 9] = ship2;
            startField.GameField[8, 9] = ship2;

            int choice = 0;
            while (choice != -1)
            {
                List<string> options = startField.Ships.Select(ship =>
                        $"{ship.Ship.Type}, size: {ship.Size}, weapons: {ship.Weapons.Count}, repairs: {ship.Repairs.Count}")
                    .ToList();
                options.InsertRange(0, new string[]{"Ready", "Buy ship", "Remove ship from field"});
                choice = _presenter.MenuMultipleChoice(true, "Choose ship or buy:", () =>
                {
                    _presenter.ShowGameField(startField.GameField, _players, startField.FieldLabels, startField.GamePlayer);
                    _presenter.ShowMessage($"Player: {startField.GamePlayer.Name}, Points: {startField.Points}", false, false);
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
                        $"{ship.Item1.Type}, size: {ship.Item1.Size}, max hp: {ship.Item1.MaxHp}, speed: {ship.Item1.Speed}, cost: {ship.Item2}")
                    .ToList();
                options.Insert(0, "Quite");
                choice = _presenter.MenuMultipleChoice(true, $"{message}Choose ship to buy:", () =>
                {
                    _presenter.ShowGameField(startField.GameField, _players, startField.FieldLabels, startField.GamePlayer);
                    _presenter.ShowMessage($"Player: {startField.GamePlayer.Name}, Points: {startField.Points}", false, false);
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
                            StateCode.PointsShortage => "Not enough points\n",
                            _ => "Some error\n"
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
                    _presenter.ShowMessage($"Player: {startField.GamePlayer.Name}, Points: {startField.Points}", false,
                        false);
                    _presenter.ShowMessage($"Choose ship for removing", false, false);
                }, startField.GamePlayer);
            if (point.isSelected)
            {
                _manager.RemoveShipFromField(startField.GamePlayer, point.X, point.Y);
            }
        }

        private void ShipManage(IStartField startField, IGameShip ship)
        {
            int choice = 0;
            while (choice != -1)
            {
                choice = _presenter.MenuMultipleChoice(true, "Choose action:", () =>
                    {
                        _presenter.ShowGameField(startField.GameField, _players, startField.FieldLabels,
                            startField.GamePlayer);
                        _presenter.ShowMessage($"Player: {startField.GamePlayer.Name}, Points: {startField.Points}",
                            false, false);
                        _presenter.ShowMessage(
                            $"{ship.Ship.Type}, size: {ship.Size}, weapons: {ship.Weapons.Count}, repairs: {ship.Repairs.Count}",
                            false, false);
                    },
                    new string[]
                    {
                        "Set ship on the field", "Add weapon", "Add repair", "Remove weapon", "Remove repair",
                        "Sell ship", "Quit"
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

        private void SetShip(IStartField startField, IGameShip ship)
        {
            (ushort X, ushort Y, bool isSelected) point = _presenter.SelectCell(startField.GameField, _players,
                GetCenterOfStartField(startField),
                startField.FieldLabels,
                () =>
                {
                    _presenter.ShowMessage($"Player: {startField.GamePlayer.Name}, Points: {startField.Points}", false,
                        false);
                    _presenter.ShowMessage(
                        $"{ship.Ship.Type}, size: {ship.Size}, weapons: {ship.Weapons.Count}, repairs: {ship.Repairs.Count}",
                        false, false);
                    _presenter.ShowMessage($"Select position of the ship's stern", false, false);
                }, startField.GamePlayer);
            if (point.isSelected)
            {
                int choice = _presenter.MenuMultipleChoice(true, "Choose direction:", () =>
                    {
                        _presenter.ShowGameField(startField.GameField, _players, startField.FieldLabels,
                            startField.GamePlayer);
                        _presenter.ShowMessage($"Player: {startField.GamePlayer.Name}, Points: {startField.Points}",
                            false, false);
                        _presenter.ShowMessage(
                            $"{ship.Ship.Type}, size: {ship.Size}, weapons: {ship.Weapons.Count}, repairs: {ship.Repairs.Count}",
                            false, false);
                    },
                    new string[]
                    {
                        "Up", "Right", "Down", "Left", "Cancel"
                    });

                DirectionOfShipPosition direction= direction = DirectionOfShipPosition.XDec; //case 0:

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

                if (choice >= 0 && choice < 4)
                {
                    _manager.PutShipOnField(startField.GamePlayer, ship, point.X, point.Y, direction);
                }
            }
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
    }
}