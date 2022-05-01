using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

public class GunResource : Resource
{
    [Export]
    public float _shooting_speed = 10;
    [Export]
    public float _damage_multiplier = 1;
    [Export]
    public float _range_multiplier = 1;
    [Export]
    public float _speed_multiplier = 1;

    public float Spd => _shooting_speed;
    public float DmgMul => _damage_multiplier;
    public float RngMul => _range_multiplier;
    public float SpdMul => _speed_multiplier;
}
