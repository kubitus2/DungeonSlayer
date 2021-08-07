using System;

namespace Dungeon_Slayer
{
    class Player
    {
        //Max player HP.
        const int PLAYER_MAX_HEALTH = 100;
        //Max level that needs to be reached.
        const int PLAYER_MAX_LEVEL = 8;
        //Utility for Loot() method.
        const int HP_ELIGIBLE_FOR_HEAL = 45;

        //Random gen.
        readonly Random dice = new Random();

        //HP
        private int _hp;

        //Player stats.
        private int _luck;
        private int _agility;
        private int _power;
        private int _level;

        //Name of the player.
        private string _name;

        //avatar
        private string _avatar;
 
        //Player's position (x, y).
        private Vector2DInt _position;

        //Constructor. Set stats to zero initially.
        public Player(Vector2DInt position, string name)
        {
            _hp = PLAYER_MAX_HEALTH;
            _position.x = position.x;
            _position.y = position.y;
            _luck = 0;
            _agility = 0;
            _power = 0;
            _level = 1;
            _name = name;
            _avatar = "@";
        }

        //k20 dice
        private int ThrowDice()
        {
            return dice.Next(1, 20);
        }

        //change HP
        private void ChangeHP(int amount)
        {
            _hp += amount;

            //clamp value between 0 and MAX_HEALTH
            if (_hp < 0)
                _hp = 0;
            if (_hp > PLAYER_MAX_HEALTH)
                _hp = PLAYER_MAX_HEALTH;
        }

        //Compute hit effect based on stats. Luck gives you chance of being missed.
        //Agility makes the hit you take less deadly.
        //It's still not balanced but it's good starting point.
        private int CalculateHitEffect(int hit)
        {
            int hitEffect;

            if (ThrowDice() < 10 + _luck)
                hitEffect = 0;
            else
                hitEffect = hit - 2* _agility - 5;
            
            return hitEffect;
        }

        //place symbol at given position
        private void WriteAt (Vector2DInt pos, string content)
        {
            Console.SetCursorPosition(pos.x, pos.y);
            Console.Write(content);
        }

        //Name setter.
        public void SetName(string name)
        {
            _name = name;
        }

        //Receive hit, change HP accordingly and monit what happened.
        public void ReceiveHit(int hit)
        {
            int hpChange;

            hpChange = CalculateHitEffect(hit);

            if(hpChange == 0)
            {
                Console.WriteLine("Goblin missed you.");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine("You've been hit and lost {0} HP", hpChange);
                Console.ReadKey(true);
            }

            ChangeHP(-hpChange);
        }

        //Full heal. Happens when healing potions drops from defeated goblin.
        //As ChangeHP clamps value, 1000 is passed to ensure maxing out the HP.
        public void FullHeal()
        {
            ChangeHP(1000);
        }

        //Attack the goblin. Luck determines whether you miss or not. Power determines amount of damage.
        public void Attack(ref Goblin goblin)
        {
            int attackPower;
            if (ThrowDice() < 10 - _luck)
            {
                attackPower = 0;
                Console.WriteLine("You've missed!");
                Console.ReadKey(true);
            }
            else
            {
                attackPower = 5 + _power;
                Console.WriteLine("Goblin received {0} hit points.", attackPower);
                Console.ReadKey(true);
            }     

            goblin.ReceiveHit(attackPower);   
        }

        //GETTERS AND SETTERs
        //Position getter.
        public Vector2DInt GetPosition()
        {
            return _position;
        }

        //Position setter.
        public void SetPosition(Vector2DInt target)
        {
            _position = target;
        }

        //HP getter.
        public int GetHP()
        {
            return _hp;
        }

        //Stats getter.
        public Stats Stats
        {
            get
            {
                return new Stats(_luck, _agility, _power); 
            }
        }

        //Name getter.
        public string Name
        {
            get 
            {
                return _name;
            }   
        }

        //Level getter.
        public int GetLevel()
        {
            return _level;
        }

        //LevelUp.
        public void LevelUp()
        {
            _level++;
        }

        //Render player on the map.
        public void RenderPlayer()
        {
            WriteAt(_position, _avatar);
        }

        //Stats update.
        public void UpdateStats(Stats stats)
        {
            _agility = stats.agility;
            _luck = stats.luck;
            _power = stats.luck;
        }

        //Loot goblin. If the HP is lower than given threshhold, there is a chance of looting heal potions. Chance dependant on luck.
        public void LootGoblin()
        {
            if(ThrowDice() < 10 - _luck && _hp < HP_ELIGIBLE_FOR_HEAL)
            {
                Console.Clear();
                Console.WriteLine("You have found powerful healing potion in goblin's carcass! \nFull HP restored!");
                FullHeal();
                Console.ReadKey(true);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Goblin did not have anything with him...");
                Console.ReadKey(true);
            }    
        }

        //Check if maximal level has been achieved.
        public bool IsLevelMaxed()
        {
            return _level >= PLAYER_MAX_LEVEL;
        }
    }
}
