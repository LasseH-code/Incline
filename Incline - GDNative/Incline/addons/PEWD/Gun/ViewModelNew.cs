using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Incline.addons.PEWD.Bullet.New;

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
    public bool _do_reference_frame = true;
    [Export]
    public NodePath _ref_frame;
    public Spatial _ref_frame_node;

    [Export]
    public string _gun_name = "NULL";
    [Export]
    public PackedScene _bullet;
    [Export]
    public GunResource _gun;

    public override void _Ready()
    {
        _barrel_end = GetNode<Position3D>("BarrelEnd");
        _bullet_timer = GetNode<Timer>("BulletTimer");
        _bullet_timer.WaitTime = 1.0f / _gun.Spd;

        if (_do_ray_optimization) _ray_node = GetNode<RayCast>(_ray);
        if (_do_reference_frame && _ref_frame != null) _ref_frame_node = GetNode<Spatial>(_ref_frame);
        if (!_ref_frame_node.HasMethod("GetVel"))
        {
            _do_reference_frame = false;
            GD.Print("Reference Frame Disabled");
        }
    }

    public void ShootBullet()
    {
        if (_bullet_timer.TimeLeft == 0.0f)
        {
            PRBullet temp;
            temp = _bullet.Instance<PRBullet>();
            temp.GlobalTransform = _barrel_end.GlobalTransform;
            temp._res.MultiplyValues(_gun.DmgMul, _gun.SpdMul, _gun.RngMul);
            GetTree().Root.AddChild(temp);
            temp._speed_addition = ((Vector3)_ref_frame_node.Call("GetVel")).Length();
            if (_do_ray_optimization && _ray_node.IsColliding())
            {
                temp.LookAt((_ray_node.GetCollisionPoint()), Vector3.Up);
                Vector3 rot = temp.Rotation;
                rot.y = -rot.y;
                //temp.SetRotation(rot);
            }
            _bullet_timer.Start();
        }
    }
}
