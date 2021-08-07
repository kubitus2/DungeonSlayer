using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon_Slayer
{



    class Player
    {
        const int PLAYER_MAX_HEALTH = 100;
        const int PLAYER_MAX_LEVEL = 2;

        readonly Random dice = new Random();

        private int _hp;
        private int _luck;
        private int _agility;
        private int _power;

        private string _name;
        private bool _isAlive;
        private int _level;

        private Vector2DInt _position;

        public Player(Vector2DInt position, string name)
        {
            _hp = PLAYER_MAX_HEALTH;
            _position.x = position.x;
            _position.y = position.y;
            _luck = 0;
            _agility = 0;
            _power = 0;
            _isAlive = true;
            _level = 1;
            _name = name;
        }


        private int ThrowDice()
        {
            return dice.Next(1, 20);
        }

        private void ChangeHP(int amount)
        {
            _hp += amount;

            if (_hp < 0)
                _hp = 0;
            if (_hp > PLAYER_MAX_HEALTH)
                _hp = PLAYER_MAX_HEALTH;
        }

        private int CalculateHitEffect(int hit)
        {
            int hitEffect;
            hitEffect = 0;

            if (ThrowDice() < 10 + _luck)
                hitEffect = 0;
            else
                hitEffect = hit - 2* _agility - 5;
            

            return hitEffect;
        }

        public void SetName(string name)
        {
            _name = name;
        }
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

        public void FullHeal()
        {
            ChangeHP(1000);
        }

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

        public void MovePlayer(Vector2DInt a)
        {
            _position += a;
        }
        public Vector2DInt GetPosition()
        {
            return _position;
        }
        public void SetPosition(Vector2DInt target)
        {
            _position = target;
        }
        public int GetHP()
        {
            return _hp;
        }

        public Stats Stats
        {
            get
            {
                return new Stats(_luck, _agility, _power); 
            }
        }
        public string Name
        {
            get 
            {
                return _name;
            }
            
        }
        
        public void LevelUp()
        {
            _level++;
        }
        public int GetLevel()
        {
            return _level;
        }

        public void RenderPlayer()
        {
            Console.SetCursorPosition(_position.x, _position.y);
            Console.Write("@");
        }

        public void UpdateStats(Stats stats)
        {
            _agility = stats.agility;
            _luck = stats.luck;
            _power = stats.luck;
        }

        public void LootGoblin()
        {
            if(ThrowDice() < 10 - _luck && _hp < 30)
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

        public bool IsLevelMaxed()
        {
            return _level >= PLAYER_MAX_LEVEL;
        }
//


    }
}
