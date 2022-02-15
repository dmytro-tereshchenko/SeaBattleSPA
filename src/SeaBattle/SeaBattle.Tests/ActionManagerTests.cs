using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Managers;
using Xunit;

namespace SeaBattle.Tests
{
    public class ActionManagerTests
    {
        private IActionManager _manager;

        public ActionManagerTests()
        {
            _manager = new ActionManager(new GameFieldActionUtility());
        }

        [Fact]
        public void MoveShipEqualSuccess()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            IGameField field = new GameField(sizeX, sizeY);

            GamePlayer player1 = new GamePlayer(1, "Player 1");
            GamePlayer player2 = new GamePlayer(2, "Player 2");

            ShipType mixed = new ShipType() {Id = 1, Name = "Mixed"};

            GameShip ship = new GameShip(new Ship(mixed, 2, 200, 3), player1, 50);
            GameShip ship2 = new GameShip(new Ship(mixed, 2, 200, 3){Id=2}, player2, 50){Id=1};
            ship2.Weapons.Add(new BasicWeapon(1, 50, 10));
            ship2.Repairs.Add(new BasicRepair(1, 40, 10));
            GameShip ship3 = new GameShip(new Ship(mixed, 2, 200, 3), player2, 50);

            field[3, 3] = ship;
            field[4, 3] = ship;
            field[9, 9] = ship2;
            field[8, 9] = ship2;
            field[1, 2] = ship3;
            field[1, 3] = ship3;

            ushort posX = 7;
            ushort posY = 9;
            DirectionOfShipPosition direction = DirectionOfShipPosition.XDec;

            var expectedField = new GameField(sizeX, sizeY);

            expectedField[3, 3] = ship;
            expectedField[4, 3] = ship;
            expectedField[7, 9] = ship2;
            expectedField[6, 9] = ship2;
            expectedField[1, 2] = ship3;
            expectedField[1, 3] = ship3;

            // Act
            var result = _manager.MoveShip(player2, ship2, posX, posY, direction, field);

            // Assert
            Assert.Equal(expectedField, field);
        }
    }
}
