using Godot;
using Incline.addons.PEWD.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incline.addons.PEWD
{
    public class PewdSingleton
    {
        public static void Damage(float hp, IDamagable subj)
        {
            subj.Health -= hp * (1 - subj.Resistance);
            GD.Print(subj.Health + hp + "->" + subj.Health);
            if (subj.Health <= 0) subj.Kill();
            subj.DamageEffect();
        }

        public static void Damage(float hp, Node subj)
        {
            if (subj.Get("Resistance") == null || subj.Get("Health") == null
                || !subj.HasMethod("Kill") || !subj.HasMethod("DamageEffect")) return;

            float h, r;
            h = (float)subj.Get("Health");
            r = (float)subj.Get("Resistance");

            h -= hp * (1 - r);
            GD.Print(h + hp + "->" + h);
            if (h <= 0) subj.Call("Kill");
            subj.Call("DamageEffect");

            subj.Set("Health", h);
        }
    }
}
