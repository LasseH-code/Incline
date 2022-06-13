using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Incline.addons.PEWD.Health;

namespace Incline.addons.PEWD.Bullet.New
{
    public class PRBullet : Bullet
    {

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

            /*Vector3 t = this.Translation;
            Vector3 n = t.Normalized();*/
            this.Translation += (-this.GlobalTransform.basis.z) * ((_res.Spd + _speed_addition) * delta);//.Rotated(Vector3.Up, Mathf.Deg2Rad(90));
        }
    }
}
