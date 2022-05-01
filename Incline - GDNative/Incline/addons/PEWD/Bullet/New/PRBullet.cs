using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Incline.addons.PEWD.Bullet.New
{
    public class PRBullet : RayCast
    {
        [Export]
        public BulletResourceSimple _res;
        [Export]
        private float _collision_distance = .05f;

        private Vector3 _starting_position;

        private bool _kill_next_frame = false;

        public override void _Ready()
        {
            _starting_position = this.GlobalTransform.origin;
            this.CastTo = new Vector3(0,0,0);
            this.Enabled = true;
            //this.RotateY(180);
        }

        internal void impact(Spatial body)
        {
            if (body != null)
            {
                if (body.HasMethod("_damage"))
                {
                    body.Call("_damage", _res.Dmg);
                }
            }
            QueueFree();
        }

        public override void _PhysicsProcess(float delta)
        {
            this.CastTo = new Vector3(0,0,-((_res.Spd * delta) + _collision_distance)); // was on X before
            Spatial body = (Spatial)this.GetCollider();
            if (_kill_next_frame) impact(body);
            if (_starting_position.DistanceTo(this.GlobalTransform.origin) > _res.Rng) impact(null);
            if (body != null)
            {
                Vector3 p = this.GetCollisionPoint();
                if (p.DistanceTo(this.GlobalTransform.origin) < _collision_distance)
                {
                    impact((Spatial)this.GetCollider());
                }
                else
                {
                    _kill_next_frame = true;
                    //return;
                }
            }
            // Compact this

            Vector3 t = this.Translation;
            Vector3 n = t.Normalized();
            this.Translation += ((-this.GlobalTransform.basis.z) * (_res.Spd * delta));//.Rotated(Vector3.Up, Mathf.Deg2Rad(90));
        }
    }
}
