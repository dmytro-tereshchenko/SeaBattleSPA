using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
