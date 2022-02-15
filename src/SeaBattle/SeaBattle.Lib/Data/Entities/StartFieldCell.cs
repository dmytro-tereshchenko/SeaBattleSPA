using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// The field's cell for storing the location of ships when initializing game.
    /// </summary>
    public class StartFieldCell : IStartFieldCell
    {
        public uint Id { get; set; }

        public ushort X { get; set; }

        public ushort Y { get; set; }

        [JsonIgnore]
        public uint StartFieldId { get; set; }

        [JsonIgnore]
        public StartField StartField { get; set; }
    }
}
