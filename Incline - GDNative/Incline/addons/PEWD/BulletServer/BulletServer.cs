using Godot;
using System;
using System.Collections.Generic;
//using System.Windows;
//using System.Numerics;

public class BulletServer : Spatial
{
  internal struct SmartBulletType
  {
    public string _name;
    public Mesh _m;
    public float _max_distance;
    public float _damage;
    public float _speed;
    public Script[] _custom_actions;
  }
  internal struct BulletMod
  {
    public float _damage_factor;
    public float _speed_factor;
    public float _max_distance_factor;
    public Script[] _custom_actions_mods;
  }
  internal struct SmartBulletData
  {
    public Vector3 _translation;
    public Vector3 _start_translation;
  }

}
