using System;
using System.Collections.Generic;
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
            CreateGame();
            CreateGameField();
            CreatePlayers();
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
            /*IGameShip ship = new GameShip(new Ship(ShipType.Military, 2, 100, 10), _players.ElementAt(0), 50); //testing
            IGameShip ship2 = new GameShip(new Ship(ShipType.Military, 2, 100, 10), _players.ElementAt(1), 50);
            startField.GameField[2, 2] = ship;
            startField.GameField[2, 3] = ship;
            startField.GameField[9, 9] = ship2;
            startField.GameField[10, 9] = ship2;*/
            _presenter.ShowGameField(startField.GameField, _players, startField.GamePlayer);
            Console.ReadKey();
        }
    }
}