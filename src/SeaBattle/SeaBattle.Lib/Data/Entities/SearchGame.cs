using System.ComponentModel.DataAnnotations;

namespace SeaBattle.Lib.Entities
{
    public class SearchGame : ISearchGame
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GameId { get; set; }

        public Game Game { get; set; }
    }
}
