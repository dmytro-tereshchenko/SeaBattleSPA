using System;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Managers;

namespace SeaBattle.UIConsole
{
    public class GameUI
    {
        private IGameManager _manager;

        private IPresenter _presenter;

        public GameUI(IGameManager manager, IPresenter presenter)
        {
            _manager = manager;
            _presenter = presenter;
        }

        public void Start()
        {
            CreateGame();

            Console.ReadKey();
        }

        private void CreateGame()
        {
            bool isGetData = true;
            string message = "Enter number of players.";

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
                            $"The number of players should be equal to or less than {_manager.GetMaxNumberOfPlayers()}\nEnter number of players.";
                        break;
                    case StateCode.Success:
                        isGetData = false;
                        break;
                };

            } while (isGetData);
        }

        
    }
}