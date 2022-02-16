namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Active games, which search game players.
    /// </summary>
    public interface ISearchGame : IEntity
    {
        /// <summary>
        /// Game Id
        /// </summary>
        /// <value><see cref="int"/></value>
        int GameId { get; set; }

        /// <summary>
        /// Navigation property to game, which search game players.
        /// </summary>
        Game Game { get; set; }
    }
}
