namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Simple entity for enums and little tables.
    /// </summary>
    public interface IShortEntity
    {
        /// <summary>
        /// Id of entity
        /// </summary>
        /// <value><see cref="short"/></value>
        short Id { get; set; }
    }
}
