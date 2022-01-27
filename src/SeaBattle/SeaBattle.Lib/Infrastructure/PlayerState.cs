namespace SeaBattle.Lib.Infrastructure
{
    public enum PlayerState
    {
        Created, //created (initial)
        InitializeField, //generate start team of ship, initialize field
        Ready, //wait for another player
        Process //in process of game
    }
}
