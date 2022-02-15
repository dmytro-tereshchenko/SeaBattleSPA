using System;
using System.Collections.Generic;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Managers;
using Xunit;

namespace SeaBattle.Tests
{
    public class GameFieldActionUtilityTests
    {
        private IGameFieldActionUtility _utility;

        public GameFieldActionUtilityTests()
        {
            _utility = new GameFieldActionUtility();
        }

        [Fact]
        public void CheckFreeAreaAroundShipReturnFalse()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            IGameField field = new GameField(sizeX, sizeY, 1);

            IGamePlayer player1 = new GamePlayer(1, "Player 1");
            IGamePlayer player2 = new GamePlayer(2, "Player 2");

            IGameShip ship = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);
            IGameShip ship2 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);
            IGameShip ship3 = new GameShip(new Ship(ShipType.Military, 4, 400, 1), player2, 50);
            IGameShip ship4 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);

            field[3, 3] = ship;
            field[4, 3] = ship;
            field[9, 9] = ship2;
            field[8, 9] = ship2;
            field[1, 10] = ship3;
            field[2, 10] = ship3;
            field[3, 10] = ship3;
            field[4, 10] = ship3;
            field[5, 5] = ship4;
            field[5, 6] = ship4;

            List<(ushort X, ushort Y)> coordinates = new List<(ushort X, ushort Y)>()
            {
                (9, 9),//another ship
                (9, 10),//near another ship and border
                (2, 3),//up of the ship
                (2, 2),//up left diagonal of the ship
                (2, 4),//up right diagonal of the ship
                (3, 2),//left of the ship
                (3, 4),//right of the ship
                (5, 3),//down of the ship
                (5, 2),//down left diagonal of the ship
                (5, 4)//down right diagonal of the ship
            };

            // Assert
            Assert.All(coordinates,
                ((ushort X, ushort Y) position) =>
                    Assert.False(_utility.CheckFreeAreaAroundShip(field, position.X, position.Y, ship4)));
        }

        [Fact]
        public void CheckFreeAreaAroundShipReturnTrue()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            IGameField field = new GameField(sizeX, sizeY, 1);

            IGamePlayer player1 = new GamePlayer(1, "Player 1");
            IGamePlayer player2 = new GamePlayer(2, "Player 2");

            IGameShip ship = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);
            IGameShip ship2 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);
            IGameShip ship3 = new GameShip(new Ship(ShipType.Military, 4, 400, 1), player2, 50);
            IGameShip ship4 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);

            field[3, 3] = ship;
            field[4, 3] = ship;
            field[9, 9] = ship2;
            field[8, 9] = ship2;
            field[1, 10] = ship3;
            field[2, 10] = ship3;
            field[3, 10] = ship3;
            field[4, 10] = ship3;
            field[5, 5] = ship4;
            field[5, 6] = ship4;

            List<(ushort X, ushort Y)> coordinates = new List<(ushort X, ushort Y)>()
            {
                (10, 5),//down border
                (6, 10),//right border
                (1, 5),//top border
                (5, 1),//left border
                (1, 1),//corner
                (5, 5),//current ship
                (5, 7)//near of current ship
            };

            // Assert
            Assert.All(coordinates, ((ushort X, ushort Y) position) =>
                Assert.True(_utility.CheckFreeAreaAroundShip(field, position.X, position.Y, ship4)));
        }

        [Fact]
        public void CheckFreeAreaAroundShipArgumentOutOfRangeException()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            IGameField field = new GameField(sizeX, sizeY, 1);

            IGamePlayer player1 = new GamePlayer(1, "Player 1");
            IGamePlayer player2 = new GamePlayer(2, "Player 2");

            IGameShip ship = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);
            IGameShip ship2 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);
            IGameShip ship3 = new GameShip(new Ship(ShipType.Military, 4, 400, 1), player2, 50);
            IGameShip ship4 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);

            field[3, 3] = ship;
            field[4, 3] = ship;
            field[9, 9] = ship2;
            field[8, 9] = ship2;
            field[1, 10] = ship3;
            field[2, 10] = ship3;
            field[3, 10] = ship3;
            field[4, 10] = ship3;
            field[5, 5] = ship4;
            field[5, 6] = ship4;

            List<(ushort X, ushort Y)> coordinates = new List<(ushort X, ushort Y)>()
            {
                (11, 5),//down border
                (5, 11),//right border
                (0, 5),//top border
                (5, 0),//left border
            };

            // Assert
            Assert.All(coordinates,
                ((ushort X, ushort Y) position) => Assert.Throws<ArgumentOutOfRangeException>(() =>
                    _utility.CheckFreeAreaAroundShip(field, position.X, position.Y, ship4)));
        }

        [Fact]
        public void GetCoordinatesShipByPositionEqualsXInc()
        {
            // Arrange
            byte shipSize = 3;
            ushort posX = 1;
            ushort posY = 5;
            DirectionOfShipPosition direction = DirectionOfShipPosition.XInc;
            ICollection<(ushort, ushort)> expectedTuples = new List<(ushort, ushort)>()
            {
                (1, 5),
                (2, 5),
                (3, 5)
            };

            // Act
            var result = _utility.GetCoordinatesShipByPosition(shipSize, posX, posY, direction);

            // Assert
            Assert.Equal(expectedTuples, result);
        }

        [Fact]
        public void GetCoordinatesShipByPositionEqualsYDec()
        {
            // Arrange
            byte shipSize = 3;
            ushort posX = 2;
            ushort posY = 5;
            DirectionOfShipPosition direction = DirectionOfShipPosition.YDec;
            ICollection<(ushort, ushort)> expectedTuples = new List<(ushort, ushort)>()
            {
                (2, 5),
                (2, 4),
                (2, 3)
            };

            // Act
            var result = _utility.GetCoordinatesShipByPosition(shipSize, posX, posY, direction);

            // Assert
            Assert.Equal(expectedTuples, result);
        }

        [Fact]
        public void GetShipCoordinates()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            IGameField field = new GameField(sizeX, sizeY, 1);

            IGamePlayer player1 = new GamePlayer(1, "Player 1");
            IGamePlayer player2 = new GamePlayer(2, "Player 2");

            IGameShip ship = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);
            IGameShip ship4 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);

            field[3, 3] = ship;
            field[4, 3] = ship;
            field[7, 9] = ship4;
            field[8, 9] = ship4;

            ICollection<(ushort, ushort)> expectedTuples = new List<(ushort, ushort)>()
            {
                (7, 9),
                (8, 9)
            };

            // Act
            var result = _utility.GetShipCoordinates(ship4, field);

            // Assert
            Assert.Equal(expectedTuples, result);
        }

        [Fact]
        public void GetGeometricCenterOfShip()
        {
            // Arrange
            ICollection<(ushort, ushort)> ship = new List<(ushort, ushort)>()
            {
                (5, 5),
                (5, 6)
            };
            (float, float) expected = (4.5f, 5f);

            // Act
            var result = _utility.GetGeometricCenterOfShip(ship);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetDistanceBetween2Points()
        {
            // Arrange
            (float, float) point1 = (2.5f, 4.5f);
            (float, float) point2 = (3.5f, 1.5f);
            float expected = 3.162278f;

            // Act
            var result = _utility.GetDistanceBetween2Points(point1, point2);

            // Assert
            Assert.Equal(expected, result, 5);
        }

        [Fact]
        public void GetAllShipsCoordinatesAllEqual()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;
            IGameField field = new GameField(sizeX, sizeY, 1);
            IGamePlayer player1 = new GamePlayer(1, "Player 1");
            IGamePlayer player2 = new GamePlayer(2, "Player 2");
            IGameShip ship = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);
            IGameShip ship2 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);
            IGameShip ship3 = new GameShip(new Ship(ShipType.Military, 4, 400, 1), player2, 50);
            field[3, 3] = ship;
            field[4, 3] = ship;
            field[9, 9] = ship2;
            field[8, 9] = ship2;
            field[1, 10] = ship3;
            field[2, 10] = ship3;
            field[3, 10] = ship3;
            field[4, 10] = ship3;

            IGameShip ship4 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);

            field[5, 5] = ship4;
            field[5, 6] = ship4;
            IDictionary<IGameShip, ICollection<(ushort, ushort)>> expected =
                new Dictionary<IGameShip, ICollection<(ushort, ushort)>>();
            expected[ship3] = new List<(ushort, ushort)>() {(1, 10), (2, 10), (3, 10), (4, 10)};
            expected[ship] = new List<(ushort, ushort)>() {(3, 3), (4, 3)};
            expected[ship4] = new List<(ushort, ushort)>() {(5, 5), (5, 6)};
            expected[ship2] = new List<(ushort, ushort)>() {(8, 9), (9, 9)};

            // Act
            var result = _utility.GetAllShipsCoordinates(field);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetAllShipsCoordinatesAllNotEqual()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;
            IGameField field = new GameField(sizeX, sizeY, 1);
            IGamePlayer player1 = new GamePlayer(1, "Player 1");
            IGamePlayer player2 = new GamePlayer(2, "Player 2");
            IGameShip ship = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);
            IGameShip ship2 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);
            IGameShip ship3 = new GameShip(new Ship(ShipType.Military, 4, 400, 1), player2, 50);
            field[3, 3] = ship;
            field[4, 3] = ship;
            field[9, 9] = ship2;
            field[8, 9] = ship2;
            field[1, 10] = ship3;
            field[2, 10] = ship3;
            field[3, 10] = ship3;
            field[4, 10] = ship3;

            IGameShip ship4 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);

            field[5, 5] = ship4;
            field[5, 6] = ship4;
            IDictionary<IGameShip, ICollection<(ushort, ushort)>> expected =
                new Dictionary<IGameShip, ICollection<(ushort, ushort)>>();
            expected[ship3] = new List<(ushort, ushort)>() { (1, 10), (2, 10), (3, 10), (4, 10) };
            expected[ship] = new List<(ushort, ushort)>() { (3, 3), (4, 3) };
            expected[ship4] = new List<(ushort, ushort)>() { (5, 5), (5, 6) };

            // Act
            var result = _utility.GetAllShipsCoordinates(field);

            // Assert
            Assert.NotEqual(expected, result);
        }

        [Fact]
        public void GetAllShipsCoordinatesPlayerEqual()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;
            IGameField field = new GameField(sizeX, sizeY, 1);
            IGamePlayer player1 = new GamePlayer(1, "Player 1");
            IGamePlayer player2 = new GamePlayer(2, "Player 2");
            IGameShip ship = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);
            IGameShip ship2 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);
            IGameShip ship3 = new GameShip(new Ship(ShipType.Military, 4, 400, 1), player2, 50);
            field[3, 3] = ship;
            field[4, 3] = ship;
            field[9, 9] = ship2;
            field[8, 9] = ship2;
            field[1, 10] = ship3;
            field[2, 10] = ship3;
            field[3, 10] = ship3;
            field[4, 10] = ship3;

            IGameShip ship4 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);

            field[5, 5] = ship4;
            field[5, 6] = ship4;
            IDictionary<IGameShip, ICollection<(ushort, ushort)>> expected =
                new Dictionary<IGameShip, ICollection<(ushort, ushort)>>();
            expected[ship] = new List<(ushort, ushort)>() { (3, 3), (4, 3) };
            expected[ship4] = new List<(ushort, ushort)>() { (5, 5), (5, 6) };

            // Act
            var result = _utility.GetAllShipsCoordinates(field, player1);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetAllShipsCoordinatesPlayerNotEqual()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;
            IGameField field = new GameField(sizeX, sizeY, 1);
            IGamePlayer player1 = new GamePlayer(1, "Player 1");
            IGamePlayer player2 = new GamePlayer(2, "Player 2");
            IGameShip ship = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);
            IGameShip ship2 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);
            IGameShip ship3 = new GameShip(new Ship(ShipType.Military, 4, 400, 1), player2, 50);
            field[3, 3] = ship;
            field[4, 3] = ship;
            field[9, 9] = ship2;
            field[8, 9] = ship2;
            field[1, 10] = ship3;
            field[2, 10] = ship3;
            field[3, 10] = ship3;
            field[4, 10] = ship3;

            IGameShip ship4 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);

            field[5, 5] = ship4;
            field[5, 6] = ship4;
            IDictionary<IGameShip, ICollection<(ushort, ushort)>> expected =
                new Dictionary<IGameShip, ICollection<(ushort, ushort)>>();
            expected[ship] = new List<(ushort, ushort)>() { (3, 3), (4, 3) };
            expected[ship4] = new List<(ushort, ushort)>() { (5, 5), (5, 6) };
            expected[ship3] = new List<(ushort, ushort)>() { (1, 10), (2, 10), (3, 10), (4, 10) };

            // Act
            var result = _utility.GetAllShipsCoordinates(field, player1);

            // Assert
            Assert.NotEqual(expected, result);
        }

        [Fact]
        public void RemoveShipFromFieldResult()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;
            IGameField field = new GameField(sizeX, sizeY, 1);
            IGamePlayer player1 = new GamePlayer(1, "Player 1");
            IGamePlayer player2 = new GamePlayer(2, "Player 2");
            IGameShip ship = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);
            IGameShip ship2 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);
            field[3, 3] = ship;
            field[4, 3] = ship;
            field[9, 9] = ship2;
            field[8, 9] = ship2;

            var expected = StateCode.Success;

            // Act
            var result = _utility.RemoveShipFromField(ship, field);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void RemoveShipFromFieldRemovedShip()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;
            IGameField field = new GameField(sizeX, sizeY, 1);
            IGamePlayer player1 = new GamePlayer(1, "Player 1");
            IGamePlayer player2 = new GamePlayer(2, "Player 2");
            IGameShip ship = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);
            IGameShip ship2 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);
            field[3, 3] = ship;
            field[4, 3] = ship;
            field[9, 9] = ship2;
            field[8, 9] = ship2;

            var expectedCoordinates = new List<(ushort X, ushort Y)>() {(3, 3), (4, 3)};

            // Act
            var result = _utility.RemoveShipFromField(ship, field);

            // Assert
            Assert.All(expectedCoordinates, ((ushort X, ushort Y) coord) => Assert.Null(field[coord.X, coord.Y]));
        }

        [Fact]
        public void PutShipOnFieldResultSucces()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            IGameField field = new GameField(sizeX, sizeY, 1);

            IGamePlayer player1 = new GamePlayer(1, "Player 1");
            IGamePlayer player2 = new GamePlayer(2, "Player 2");

            IGameShip ship = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);
            IGameShip ship2 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);
            IGameShip ship3 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);

            field[3, 3] = ship;
            field[4, 3] = ship;
            field[9, 9] = ship2;
            field[8, 9] = ship2;

            ushort posX = 6;
            ushort posY = 2;

            DirectionOfShipPosition direction = DirectionOfShipPosition.YInc;

            var expected = StateCode.Success;

            // Act
            var result = _utility.PutShipOnField(player2, ship3, posX, posY, direction, field);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PutShipOnFieldResultInvalidPlayer()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            IGameField field = new GameField(sizeX, sizeY, 1);

            IGamePlayer player1 = new GamePlayer(1, "Player 1");
            IGamePlayer player2 = new GamePlayer(2, "Player 2");

            IGameShip ship3 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);

            ushort posX = 6;
            ushort posY = 2;

            DirectionOfShipPosition direction = DirectionOfShipPosition.YInc;

            var expected = StateCode.InvalidPlayer;

            // Act
            var result = _utility.PutShipOnField(player1, ship3, posX, posY, direction, field);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PutShipOnFieldResultInvalidPositionShip()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            IGameField field = new GameField(sizeX, sizeY, 1);

            IGamePlayer player1 = new GamePlayer(1, "Player 1");
            IGamePlayer player2 = new GamePlayer(2, "Player 2");

            IGameShip ship = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);
            IGameShip ship2 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);
            IGameShip ship3 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);

            field[3, 3] = ship;
            field[4, 3] = ship;
            field[9, 9] = ship2;
            field[8, 9] = ship2;

            ushort posX = 1;
            ushort posY = 3;

            DirectionOfShipPosition direction = DirectionOfShipPosition.XInc;

            var expected = StateCode.InvalidPositionShip;

            // Act
            var result = _utility.PutShipOnField(player2, ship3, posX, posY, direction, field);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PutShipOnFieldResultAddShip()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            IGameField field = new GameField(sizeX, sizeY, 1);

            IGamePlayer player1 = new GamePlayer(1, "Player 1");
            IGamePlayer player2 = new GamePlayer(2, "Player 2");

            IGameShip ship = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player1, 50);
            IGameShip ship2 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);
            IGameShip ship3 = new GameShip(new Ship(ShipType.Mixed, 2, 200, 3), player2, 50);

            field[3, 3] = ship;
            field[4, 3] = ship;
            field[9, 9] = ship2;
            field[8, 9] = ship2;

            ushort posX = 1;
            ushort posY = 8;

            DirectionOfShipPosition direction = DirectionOfShipPosition.XInc;

            var expectedCoordinates = new List<(ushort X, ushort Y)>() { (1, 8), (2, 8) };

            // Act
            var result = _utility.PutShipOnField(player2, ship3, posX, posY, direction, field);

            // Assert
            Assert.All(expectedCoordinates,
                ((ushort X, ushort Y) coord) => Assert.Equal(ship3, field[coord.X, coord.Y]));
        }
    }
}
