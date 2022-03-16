using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// A field where ships are allocated
    /// </summary>
    public class GameField : IGameField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public ushort SizeX { get; set; }

        [Required]
        public ushort SizeY { get; set; }

        [Required]
        public int GameId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; }

        public ICollection<StartField> StartFields { get; set; }

        public ICollection<GameFieldCell> GameFieldCells { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GameField()
        {
            GameFieldCells = new List<GameFieldCell>();
            StartFields = new List<StartField>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameField"/> class
        /// </summary>
        /// <param name="sizeX">Size X of game field</param>
        /// <param name="sizeY">Size Y of game field</param>
        /// <param name="id">Id of game field</param>
        public GameField(ushort sizeX, ushort sizeY, int id) : this(sizeX, sizeY) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameField"/> class
        /// </summary>
        /// <param name="sizeX">Size X of game field</param>
        /// <param name="sizeY">Size Y of game field</param>
        public GameField(ushort sizeX, ushort sizeY) : this()
        {
            SizeX = sizeX;
            SizeY = sizeY;
        }

        public GameShip this[ushort x, ushort y]
        {
            get
            {
                if (x < 1 || x > SizeX || y < 1 || y > SizeY)
                {
                    throw new IndexOutOfRangeException(
                        $"[{x},{y}] out of range [1, 1]:[{SizeX},{SizeY}] in {nameof(GameField)}");
                }

                foreach (var cell in GameFieldCells)
                {
                    if (cell.X == x && cell.Y == y)
                    {
                        return cell.GameShip;
                    }
                }

                return null;
            }
            set
            {
                if (x < 1 || x > SizeX || y < 1 || y > SizeY)
                {
                    throw new IndexOutOfRangeException(
                        $"[{x},{y}] out of range [1, 1]:[{SizeX},{SizeY}] in {nameof(GameField)}");
                }

                GameFieldCell fieldCell = null;

                foreach (var cell in GameFieldCells)
                {
                    if (cell.X == x && cell.Y == y)
                    {
                        if (value is null)
                        {
                            GameFieldCells.Remove(cell);
                            return;
                        }
                        else
                        {
                            fieldCell = cell;
                            break;
                        }
                    }
                }

                if (fieldCell is null)
                {
                    fieldCell = new GameFieldCell()
                    {
                        X = x,
                        Y = y,
                        GameFieldId = Id,
                        GameField = this
                    };
                    GameFieldCells.Add(fieldCell);
                }

                fieldCell.GameShip = value;
                fieldCell.GameShipId = value.Id;
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj is not GameField)
            {
                return false;
            }
            GameField ob = obj as GameField;

            if (this.Id.Equals(ob.Id) && this.SizeX.Equals(ob.SizeX) && this.SizeY.Equals(ob.SizeY))
            {
                for (ushort i = 1; i <= SizeX; i++)
                {
                    for (ushort j = 1; j <= SizeY; j++)
                    {
                        if ((ob[i, j] == null && this[i, j] != null) ||
                            (ob[i, j] != null && this[i, j] == null) ||
                            (ob[i, j] != null && this[i, j] != null &&
                             !ob[i, j].Equals(this[i, j])))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerator GetEnumerator() => GameFieldCells.GetEnumerator();

    }
}
