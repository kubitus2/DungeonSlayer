using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon_Slayer
{
    
    struct Vector2DInt
    {
        public int x;
        public int y;
    }

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

        private Vector2DInt _position;

        public Player(Vector2DInt position, int luck, int agility, int power)
        {
            _hp = PLAYER_MAX_HEALTH;
            _position.x = position.x;
            _position.y = position.y;
            _luck = luck;
            _agility = agility;
            _power = power;
            _isAlive = true;
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

        public void UpdatePosition(int x, int y)
        {
            _position.x = x;
            _position.y = y;
        }

        public int GetHP()
        {
            return _hp;
        }

        public void RenderPlayer()
        {
            Console.SetCursorPosition(_position.x, _position.y);
            Console.Write("@");
        }
//


    }
}
