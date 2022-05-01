using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Incline.addons.PEWD.Bullet
{
    public class BulletResourceSimple : Resource
    {
        [Export]
        public float _damage = 0.0f;
        [Export]
        public float _speed = 0.0f;
        [Export]
        public float _range = 0.0f;

        public float Dmg => _damage;
        public float Spd => _speed;
        public float Rng => _range;

        public void AugmentValues(float dmg, float spd, float rng)
        {
            _damage += dmg;
            _speed += spd;
            _range += rng;
        }
        public void MultiplyValues(float dmg, float spd, float rng)
        {
            _damage *= dmg;
            _speed *= spd;
            _range *= rng;
        }
    }
}
