using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Incline.addons.PEWD.Bullet.New
{
    public class CustomBullet : Bullet
    {
        [Export]
        protected bool do_ready = true;
        /*[Export]
        protected bool do_custom_behavior = true;
        [Export]
        protected NodePath custom_beahvior;*/
        [Export]
        protected bool disable_impact = false;
        [Export]
        protected bool override_impact = false;
        [Export]
        protected NodePath impact_path;

        public override void _Ready()
        {
            if (do_ready) base._Ready();
            //if (do_custom_behavior) GetNode(custom_beahvior).Call("Action", this);
        }
        
        protected new void impact(PhysicsBody body)
        {
            if (!disable_impact) base.impact(body);
            if (override_impact) GetNode(impact_path).Call("Action", this);
        }
    }
}
