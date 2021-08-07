namespace Dungeon_Slayer
{
    class Goblin
    {
        //Max HP of a goblin.
        const int GOBLIN_MAX_HEALTH = 30;
        //Goblin attack power.
        const int GOBLIN_ATTACK_POWER = 20;

        //HP and attack power.
        private int _hp;
        private int _attackPower;

        //HP getter.
        public int HP
        {
            get
            {
                return _hp;
            }
        }

        //Constructor.
        public Goblin()
        {
            _hp = GOBLIN_MAX_HEALTH;
            _attackPower = GOBLIN_ATTACK_POWER;
        }

        //Receive hit and decrease HP. No fancy algorithm here unlike player.
        public void ReceiveHit(int hit)
        {
            _hp -= hit;
        }

        //Attack player. Pass hit points.
        public void Attack(ref Player player)
        {
            player.ReceiveHit(_attackPower);
        }
    }
}
