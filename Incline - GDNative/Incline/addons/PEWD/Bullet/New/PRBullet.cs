using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Incline.addons.PEWD.Health;

namespace Incline.addons.PEWD.Bullet.New
{
    public class PRBullet : RayCast
    {
        [Export]
        public BulletResourceSimple _res;
        [Export]
        private float _collision_distance = .05f;

        public float _speed_addition = 0.0f;

        private Vector3 _starting_position;

        private bool _kill_next_frame = false;

        public override void _Ready()
        {
            _starting_position = this.GlobalTransform.origin;
            this.CastTo = new Vector3(0,0,0);
            this.Enabled = true;
            //this.RotateY(180);
        }

        private void impact(PhysicsBody body)
        {
            if (body != null)
            {
                if (body is IDamagable damagable/* && body.GetScript().HasMethod("Damage")*/)
                {
                    GD.Print("IDamagable hit");
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

        public override void _PhysicsProcess(float delta)
        {
            this.CastTo = new Vector3(0,0,-(((_res.Spd + _speed_addition) * delta) + _collision_distance)); // was on X before
            PhysicsBody body = (PhysicsBody)this.GetCollider();
            if (_kill_next_frame) impact(body);
            if (_starting_position.DistanceTo(this.GlobalTransform.origin) > _res.Rng) impact(null);
            if (body != null)
            {
                Vector3 p = this.GetCollisionPoint();
                if (p.DistanceTo(this.GlobalTransform.origin) < _collision_distance)
                {
                    impact((PhysicsBody)this.GetCollider());
                }
                else
                {
                    _kill_next_frame = true;
                    return;
                }
            }
            // Compact this

            Vector3 t = this.Translation;
            Vector3 n = t.Normalized();
            this.Translation += ((-this.GlobalTransform.basis.z) * ((_res.Spd + _speed_addition) * delta));//.Rotated(Vector3.Up, Mathf.Deg2Rad(90));
        }
    }
}
