namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Possible states of the game.
    /// </summary>
    public enum GameState
    {
        Created = 1, //Game was created
        SearchPlayers, //Waiting for searching and connecting players, after connect amount of maxPlayers next state Init
        Init, //Initializing game
        Process, //Game in process
        Finished //Game was finished
    }
}
