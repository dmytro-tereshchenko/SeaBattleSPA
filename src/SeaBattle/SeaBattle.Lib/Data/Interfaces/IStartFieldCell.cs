namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// The field's cell for storing the location of ships when initializing game.
    /// </summary>
    public interface IStartFieldCell : IEntity
    {
        /// <summary>
        /// Coordinate X of game field
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort X { get; set; }

        /// <summary>
        /// Coordinate Y of game field
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort Y { get; set; }

        /// <summary>
        /// Foreign key Id <see cref="StartField"/>
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint StartFieldId { get; set; }

        /// <summary>
        /// Navigate property <see cref="StartField"/>
        /// </summary>
        /// <value><see cref="StartField"/></value>
        StartField StartField { get; set; }
    }
}
