using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Incline.addons.PEWD.Health
{
    public class DamagablePhysicsBody : KinematicBody, IDamagable
    {
        [Export]
        public float _max_health;
        [Export]
        public float _health;
        [Export]
        public float _resistance;
        [Export]
        public Script effect;

        public float MaxHealth { get => _max_health; set => _max_health = value; }
        public float Health { get => _health; set => _health = value; }
        public float Resistance { get => _resistance; set => _resistance = value; }

        public void Damage(float hp)
        {
            PewdSingleton.Damage(hp, (IDamagable)this);
        }

        public void DamageEffect()
        {
            if (effect.HasMethod("action"))
            {
                effect.Call("action");
            }
            else if (effect.HasMethod("Action"))
            {
                effect.Call("Action");
            }
        }

        public void Kill()
        {
            this.QueueFree();
        }
    }
}
