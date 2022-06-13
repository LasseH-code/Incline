using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Incline.addons.PEWD.Health
{
    public interface IDamagable
    {
        [Export]
        public float MaxHealth { get; set; }
        [Export]
        public float Health { get; set; }
        [Export]
        public float Resistance { get; set; }

        public void Damage(float hp);

        public void DamageEffect();

        public void Kill();
    }
}
