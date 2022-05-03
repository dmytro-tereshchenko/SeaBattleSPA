using Microsoft.Extensions.Configuration;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Managers;

namespace SeaBattle.GameResources.Utilites
{
    public static class Config
    {
        public static string GetConnectionString(IConfiguration config)
        {
            string connectionString;

            if (config["DBServer"] is null)
            {
                //in case of local db server
                connectionString = config.GetConnectionString("DefaultConnection");
            }
            else
            {
                //in case of containerization in docker
                string server = config["DBServer"] ?? config.GetValue<string>("ConnectionDb:Server");
                string port = config["DBPort"] ?? config.GetValue<string>("ConnectionDb:Port");
                string user = config["DBUser"] ?? config.GetValue<string>("ConnectionDb:User");
                string password = config["DBPassword"] ?? config.GetValue<string>("ConnectionDb:Password");
                string database = config["Database"] ?? config.GetValue<string>("ConnectionDb:Database");
                connectionString = $"Server={server},{port};Initial Catalog={database};User ID={user};Password={password}";
            }

            return connectionString;
        }

        public static GameConfig GetGameConfig(IConfiguration config)
        {
            ushort minFieldSizeX = ushort.Parse(config.GetValue<string>("GameConfig:MinFieldSizeX"));
            ushort maxFieldSizeX = ushort.Parse(config.GetValue<string>("GameConfig:MaxFieldSizeX"));
            ushort minFieldSizeY = ushort.Parse(config.GetValue<string>("GameConfig:MinFieldSizeY"));
            ushort maxFieldSizeY = ushort.Parse(config.GetValue<string>("GameConfig:MaxFieldSizeY"));
            byte maxNumberOfPlayers =
                byte.Parse(config.GetValue<string>("GameConfig:MaxNumberOfPlayers"));
            return new GameConfig(minFieldSizeX, maxFieldSizeX, minFieldSizeY, maxFieldSizeY, maxNumberOfPlayers);
        }

        public static ShipStorageUtility GetShipStorageUtility(IConfiguration config)
        {
            int shipCostCoefficient = ushort.Parse(config.GetValue<string>("GameConfig:ShipCostCoefficient"));
            return new ShipStorageUtility(shipCostCoefficient);
        }
    }
}
