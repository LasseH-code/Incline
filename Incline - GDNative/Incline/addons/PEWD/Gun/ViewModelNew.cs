using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Incline.addons.PEWD.Bullet.New;
using Incline.addons.PEWD.Gun;

public class ViewModelNew : Spatial
{
    public Position3D _barrel_end;
    public Timer _bullet_timer;

    [Export]
    public bool _do_ray_optimization = true;
    [Export]
    public NodePath _ray;
    public RayCast _ray_node;
    [Export]
    public float _ray_optimization_min_dist = 2.0f;
    [Export]
    public bool _turn_gun = false; 
    [Export]
    public bool _do_reference_frame = true;
    [Export]
    public NodePath _ref_frame;
    public Spatial _ref_frame_node;

    [Export]
    public string _gun_name = "NULL";
    [Export]
    public PackedScene _bullet;
    [Export]
    public bool _override_shooting = false;
    [Export]
    public NodePath _override;
    public Node _override_node;
    [Export]
    public NodePath _override_holder_body;
    [Export]
    public GunResource _gun;
    [Export]
    public float damage_multiplier = 1.0f;
    [Export]
    public float range_multiplier = 1.0f;
    [Export]
    public float speed_multiplier = 1.0f;

    public override void _Ready()
    {
        _barrel_end = GetNode<Position3D>("BarrelEnd");
        _bullet_timer = GetNode<Timer>("BulletTimer");
        if (_override_shooting)
        {
            GD.Print(_override);
            _override_node = GetNode<Node>(_override);
        }
        _bullet_timer.WaitTime = 1.0f / _gun.Spd * _gun.SpdMul * speed_multiplier;

        if (_do_ray_optimization) _ray_node = GetNode<RayCast>(_ray);
        if (_do_reference_frame && _ref_frame != null) _ref_frame_node = GetNode<Spatial>(_ref_frame);
        if (!_do_reference_frame || !_ref_frame_node.HasMethod("GetVel"))
        {
            _do_reference_frame = false;
            GD.Print("Reference Frame Disabled");
        }
    }

    public void ShootBullet()
    {
        if (_bullet_timer.TimeLeft == 0.0f)
        {
            _bullet_timer.WaitTime = 1.0f / _gun.Spd*_gun.SpdMul*speed_multiplier;
            if (!_override_shooting)
            {
                PRBullet temp;
                temp = _bullet.Instance<PRBullet>();
                temp.GlobalTransform = _barrel_end.GlobalTransform;
                temp._res.MultiplyValues(_gun.DmgMul, _gun.SpdMul, _gun.RngMul);
                GetTree().Root.AddChild(temp);
                if (_do_reference_frame) temp._speed_addition = ((Vector3)_ref_frame_node.Call("GetVel")).Length();
                if (_do_ray_optimization && _ray_node.IsColliding())
                {
                    if (_do_ray_optimization &&
                        _ray_node.GetCollisionPoint().DistanceTo(this.GlobalTransform.origin) > _ray_optimization_min_dist &&
                        !_turn_gun) temp.LookAt((_ray_node.GetCollisionPoint()), Vector3.Up);
                    Vector3 rot = temp.Rotation;
                    rot.y = -rot.y;
                    //temp.SetRotation(rot);
                }
            }
            else
            {
                if (/*_override_node.HasMethod("Damage")*/true)
                {
                    //GD.Print("sdfsdfs");
                    _override_node.Call("Action",GetNode(_override_holder_body));
                }
                else GD.Print("No Valid Script Attached");
            }
            _bullet_timer.Start();
        }
    }

    public override void _Process(float _delta)
    {
        if (_do_ray_optimization && 
            _ray_node.GetCollisionPoint().DistanceTo(this.GlobalTransform.origin) > _ray_optimization_min_dist &&
            _turn_gun) this.LookAt((_ray_node.GetCollisionPoint()), Vector3.Up);

    }
}
