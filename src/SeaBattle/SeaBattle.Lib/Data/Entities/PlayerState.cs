namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Player's state in game
    /// </summary>
    public enum PlayerState
    {
        Created = 1, //created (initial)
        InitializeField, //generate start team of ship, initialize field
        Ready, //wait for another player
        Process //in process of game
    }
}
