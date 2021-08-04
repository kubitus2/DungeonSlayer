using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon_Slayer
{
    //
    class Goblin
    {
        const int GOBLIN_MAX_HEALTH = 30;
        const int GOBLIN_ATTACK_POWER = 20;

        private int _hp;
        private int _attackPower;

        public int HP
        {
            get
            {
                return _hp;
            }
        }

        public Goblin()
        {
            _hp = GOBLIN_MAX_HEALTH;
            _attackPower = GOBLIN_ATTACK_POWER;
        }

        public void ReceiveHit(int hit)
        {
            _hp -= hit;
        }

        public void Attack(Player player)
        {
            player.ReceiveHit(_attackPower);
        }

    }
}
