using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon_Slayer
{
    interface IActor
    {
        void ReceiveHit(int hit);
        void Attack(Player player);
        void Attack(Goblin goblin);
    }
}
