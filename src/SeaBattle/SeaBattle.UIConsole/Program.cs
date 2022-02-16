using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Managers;

namespace SeaBattle.UIConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            IGameFieldActionUtility utility = new GameFieldActionUtility();
            IAbstractGameFactory factory = new AbstractGameFactory(2, 1000, 5, 1000, 5, utility);
            IGameManager manager = new GameManager(factory);
            IPresenter presenter = new Presenter();

            IGameUI game = new GameUI(manager, presenter);

            //game.Start();
        }
    }


}
