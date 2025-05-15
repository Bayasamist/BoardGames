using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGames.Core;

namespace BoardGames.Core
{
    public abstract class Player
    {
        public string Name { get; set; }
        public object Symbol { get; set; }
        public bool IsComputer { get; protected set; }

        protected Player(string name, object symbol, bool isComputer)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "Player name cannot be null.");
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol), "Player symbol cannot be null.");
            IsComputer = isComputer;
        }

        public abstract Move GetMove(Game game);

        public override string ToString()
        {
            return Name;
        }
    }
}

