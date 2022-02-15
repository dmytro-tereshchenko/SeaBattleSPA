using System.Collections.Generic;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Managers;
using Xunit;

namespace SeaBattle.Tests
{
    public class ShipManagerTests
    {
        private IShipManager _manager;

        public ShipManagerTests()
        {
            _manager = new ShipManager();
        }

        [Fact]
        public void BuyShipResultSucces()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            GameField field = new GameField(sizeX, sizeY, 1);

            ICollection<GamePlayer> players = new List<GamePlayer>(2);

            GamePlayer player1 = new GamePlayer(1, "Player 1");
            GamePlayer player2 = new GamePlayer(2, "Player 2");

            players.Add(player1);
            players.Add(player2);

            int points = 10000;

            StartField startField = new StartField(field, points, 1)
            {
                GamePlayer = player1
            };

            ShipType mixed = new ShipType() { Id = 1, Name = "Mixed" };

            IGameShip ship = new GameShip(new Ship(mixed, 2, 200, 3), player1, 50);

            var expected = StateCode.Success;

            // Act
            //var result = _manager.BuyShip(players, ship, startField);

            // Assert
            //Assert.Equal(expected, result);
        }

        [Fact]
        public void BuyShipResultInvalidPlayer()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            GameField field = new GameField(sizeX, sizeY, 1);

            ICollection<GamePlayer> players = new List<GamePlayer>(2);

            GamePlayer player1 = new GamePlayer(1, "Player 1");
            GamePlayer player2 = new GamePlayer(2, "Player 2");
            GamePlayer player3 = new GamePlayer(2, "Player 3");

            players.Add(player1);
            players.Add(player2);

            int points = 10000;

            StartField startField = new StartField(field, points, 1)
            {
                GamePlayer = player3
            };

            ShipType mixed = new ShipType() { Id = 1, Name = "Mixed" };

            IGameShip ship = new GameShip(new Ship(mixed, 2, 200, 3), player2, 50);

            var expected = StateCode.InvalidPlayer;

            // Act
            //var result = _manager.BuyShip(players, ship, startField);

            // Assert
            //Assert.Equal(expected, result);
        }

        [Fact]
        public void BuyShipResultPointsShortage()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            GameField field = new GameField(sizeX, sizeY, 1);

            ICollection<GamePlayer> players = new List<GamePlayer>(2);

            GamePlayer player1 = new GamePlayer(1, "Player 1");
            GamePlayer player2 = new GamePlayer(2, "Player 2");

            players.Add(player1);
            players.Add(player2);

            int points = 1000;

            StartField startField = new StartField(field, points, 1)
            {
                GamePlayer = player1
            };

            ShipType mixed = new ShipType() { Id = 1, Name = "Mixed" };

            GameShip ship = new GameShip(new Ship(mixed, 2, 200, 3), player2, 2000);

            var expected = StateCode.PointsShortage;

            // Act
            //var result = _manager.BuyShip(players, ship, startField);

            // Assert
            //Assert.Equal(expected, result);
        }

        [Fact]
        public void BuyShipAddShip()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            GameField field = new GameField(sizeX, sizeY, 1);

            ICollection<IGamePlayer> players = new List<IGamePlayer>(2);

            GamePlayer player1 = new GamePlayer(1, "Player 1");
            GamePlayer player2 = new GamePlayer(2, "Player 2");

            players.Add(player1);
            players.Add(player2);

            ShipType mixed = new ShipType() { Id = 1, Name = "Mixed" };

            GameShip ship = new GameShip(new Ship(mixed, 2, 200, 3), player2, 2000);
            GameShip ship2 = new GameShip(new Ship(mixed, 2, 200, 3), player2, 2000);

            int points = 10000;

            bool[,] fieldLabels = new bool[sizeX, sizeY];

            ICollection<GameShip> gameShips = new List<GameShip>() {ship2};

            //IStartField startField = new StartField(field, fieldLabels, player1, points, gameShips, 1);

            var expected = new List<IGameShip>() {ship2, ship};

            // Act
            //var result = _manager.BuyShip(players, ship, startField);

            // Assert
            //Assert.Equal(expected, startField.Ships);
        }

        [Fact]
        public void BuyShipRemovePoints()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            GameField field = new GameField(sizeX, sizeY, 1);

            ICollection<GamePlayer> players = new List<GamePlayer>(2);

            GamePlayer player1 = new GamePlayer(1, "Player 1");
            GamePlayer player2 = new GamePlayer(2, "Player 2");

            players.Add(player1);
            players.Add(player2);

            ShipType mixed = new ShipType() { Id = 1, Name = "Mixed" };

            GameShip ship = new GameShip(new Ship(mixed, 2, 200, 3), player2, 2000);
            GameShip ship2 = new GameShip(new Ship(mixed, 2, 200, 3), player2, 2000);

            int points = 10000;

            bool[,] fieldLabels = new bool[sizeX, sizeY];

            ICollection<IGameShip> gameShips = new List<IGameShip>() { ship2 };

            //StartField startField = new StartField(field, fieldLabels, player1, points, gameShips, 1);

            var expected = 8000;

            // Act
            //var result = _manager.BuyShip(players, ship, startField);

            // Assert
            //Assert.Equal(expected, startField.Points);
        }

        [Fact]
        public void SellShipResultSucces()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            GameField field = new GameField(sizeX, sizeY, 1);

            ICollection<IGamePlayer> players = new List<IGamePlayer>(2);

            GamePlayer player1 = new GamePlayer(1, "Player 1");
            GamePlayer player2 = new GamePlayer(2, "Player 2");

            players.Add(player1);
            players.Add(player2);

            ShipType mixed = new ShipType() { Id = 1, Name = "Mixed" };

            GameShip ship = new GameShip(new Ship(mixed, 2, 200, 3), player2, 2000);
            GameShip ship2 = new GameShip(new Ship(mixed, 2, 200, 3), player2, 2000);

            int points = 10000;

            bool[,] fieldLabels = new bool[sizeX, sizeY];

            ICollection<GameShip> gameShips = new List<GameShip>() { ship2, ship };

            //StartField startField = new StartField(field, fieldLabels, player1, points, gameShips, 1);

            var expected = StateCode.Success;

            // Act
            //var result = _manager.SellShip(players, ship2, startField);

            // Assert
            //Assert.Equal(expected, result);
        }

        [Fact]
        public void SellShipResultInvalidPlayer()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            GameField field = new GameField(sizeX, sizeY, 1);

            ICollection<GamePlayer> players = new List<GamePlayer>(2);

            GamePlayer player1 = new GamePlayer(1, "Player 1");
            GamePlayer player2 = new GamePlayer(2, "Player 2");
            GamePlayer player3 = new GamePlayer(2, "Player 3");

            players.Add(player1);
            players.Add(player2);

            ShipType mixed = new ShipType() { Id = 1, Name = "Mixed" };

            GameShip ship = new GameShip(new Ship(mixed, 2, 200, 3), player2, 2000);
            GameShip ship2 = new GameShip(new Ship(mixed, 2, 200, 3), player2, 2000);

            int points = 10000;

            bool[,] fieldLabels = new bool[sizeX, sizeY];

            ICollection<GameShip> gameShips = new List<GameShip>() { ship2, ship };

            //StartField startField = new StartField(field, fieldLabels, player3, points, gameShips, 1);

            var expected = StateCode.InvalidPlayer;

            // Act
            //var result = _manager.SellShip(players, ship2, startField);

            // Assert
            //Assert.Equal(expected, result);
        }

        [Fact]
        public void SellShipRemoveShip()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            GameField field = new GameField(sizeX, sizeY, 1);

            ICollection<GamePlayer> players = new List<GamePlayer>(2);

            GamePlayer player1 = new GamePlayer(1, "Player 1");
            GamePlayer player2 = new GamePlayer(2, "Player 2");

            players.Add(player1);
            players.Add(player2);

            ShipType mixed = new ShipType() { Id = 1, Name = "Mixed" };

            GameShip ship = new GameShip(new Ship(mixed, 2, 200, 3), player2, 2000);
            GameShip ship2 = new GameShip(new Ship(mixed, 2, 200, 3), player2, 2000);

            int points = 10000;

            bool[,] fieldLabels = new bool[sizeX, sizeY];

            ICollection<GameShip> gameShips = new List<GameShip>() { ship2, ship };

            //StartField startField = new StartField(field, fieldLabels, player1, points, gameShips, 1);

            var expected = new List<GameShip>() { ship };

            // Act
            //var result = _manager.SellShip(players, ship2, startField);

            // Assert
            //Assert.Equal(expected, startField.Ships);
        }

        [Fact]
        public void SellShipAddPoints()
        {
            // Arrange
            const ushort sizeX = 10;
            const ushort sizeY = 10;

            GameField field = new GameField(sizeX, sizeY, 1);

            ICollection<GamePlayer> players = new List<GamePlayer>(2);

            GamePlayer player1 = new GamePlayer(1, "Player 1");
            GamePlayer player2 = new GamePlayer(2, "Player 2");

            players.Add(player1);
            players.Add(player2);

            ShipType mixed = new ShipType() { Id = 1, Name = "Mixed" };

            GameShip ship = new GameShip(new Ship(mixed, 2, 200, 3), player2, 2000);
            GameShip ship2 = new GameShip(new Ship(mixed, 2, 200, 3), player2, 2000);

            int points = 10000;

            bool[,] fieldLabels = new bool[sizeX, sizeY];

            ICollection<GameShip> gameShips = new List<GameShip>() { ship2, ship };

            //StartField startField = new StartField(field, fieldLabels, player1, points, gameShips, 1);

            var expected = 12000;

            // Act
            //var result = _manager.SellShip(players, ship2, startField);

            // Assert
            //Assert.Equal(expected, startField.Points);
        }
    }
}
