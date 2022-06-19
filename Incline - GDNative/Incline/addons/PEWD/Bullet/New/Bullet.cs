using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Incline.addons.PEWD.Health;

namespace Incline.addons.PEWD.Bullet.New
{
    public class Bullet : RayCast
    {
        [Export]
        public BulletResourceSimple _res;
        [Export]
        protected float _collision_distance = .05f;

        public float _speed_addition = 0.0f;

        protected Vector3 _starting_position;

        protected bool _kill_next_frame = false;

        public override void _Ready()
        {
            _starting_position = this.GlobalTransform.origin;
            this.CastTo = new Vector3(0, 0, 0);
            this.Enabled = true;
            //this.RotateY(180);
        }

        protected virtual void impact(PhysicsBody body)
        {
            if (body != null)
            {
                if (body is IDamagable damagable/* && body.GetScript().HasMethod("Damage")*/)
                {
                    //GD.Print("IDamagable hit");
                    damagable.Damage(_res.Dmg);
                }
                else if (body.HasMethod("Damage"))
                {
                    body.Call("Damage", _res.Dmg);
                }
                /*else
                {
                    GD.Print(body.Name + " does not implement IDamagable");
                    GD.Print("body.GetScript() = " + body.GetScript());
                }*/
            }
            QueueFree();
        }
    }
}
