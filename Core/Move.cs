using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGames.Core;

namespace BoardGames.Core
{
    public abstract class Move
    {
        public Player Player { get; set; }

        protected Move(Player player)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player), "Player cannot be null in Move.");
        }

        public abstract override string ToString();
    }
}

