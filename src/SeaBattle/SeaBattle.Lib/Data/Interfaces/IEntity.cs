using SeaBattle.Lib.Repositories;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Common Entity used in <see cref="IRepository{T}"/>
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Id of entity
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint Id { get; set; }
    }
}
