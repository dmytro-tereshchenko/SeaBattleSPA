﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Data
{
    /// <summary>
    /// Common player (user)
    /// </summary>
    public class Player
    {
        public uint Id { get; set; }

        public string Name { get; protected set; }

        public Player(uint id, string name) : this(name) => Id = id;

        public Player(string name)
        {
            Name = name;
        }
    }
}