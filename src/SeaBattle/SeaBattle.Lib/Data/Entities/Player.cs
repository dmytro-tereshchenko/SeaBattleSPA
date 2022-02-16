using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Common player (user)
    /// </summary>
    public class Player : IPlayer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Player(){}

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class
        /// </summary>
        /// <param name="id">Player's id</param>
        /// <param name="name">Player's name</param>
        public Player(int id, string name) : this(name) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class
        /// </summary>
        /// <param name="name">Player's name</param>
        public Player(string name)
        {
            Name = name;
        }
    }
}
