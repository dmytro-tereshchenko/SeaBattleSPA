namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Common player (user)
    /// </summary>
    public interface IPlayer : IEntity
    {
        /// <summary>
        /// Players name
        /// </summary>
        /// <value><see cref="string"/></value>
        public string Name { get; }
    }
}
