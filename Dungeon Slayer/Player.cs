using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon_Slayer
{



    class Player
    {
        const int PLAYER_MAX_HEALTH = 100;

        readonly Random dice = new Random();

        private int _hp;
        private int _luck;
        private int _agility;
        private int _power;

        private string _name;
        private bool _isAlive;
        private int _level;

        private Vector2DInt _position;

        public Player(Vector2DInt position, Stats stats, string name)
        {
            _hp = PLAYER_MAX_HEALTH;
            _position.x = position.x;
            _position.y = position.y;
            _luck = stats.luck;
            _agility = stats.agility;
            _power = stats.power;
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
            hitEffect = ThrowDice();
            //algorithm

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

            ChangeHP(hpChange);
        }

        public void FullHeal()
        {
            ChangeHP(1000);
        }

        public void Attack(Goblin goblin)
        {
            //
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
        
        public void LevelUp()
        {
            _level++;
        }

        public void RenderPlayer()
        {
            Console.SetCursorPosition(_position.x, _position.y);
            Console.Write("@");
        }
//


    }
}
