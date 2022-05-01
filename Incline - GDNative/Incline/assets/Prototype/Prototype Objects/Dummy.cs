using Godot;
using Incline.addons.PEWD.Health;
using System;

public class Dummy : StaticBody, IDamagable
{
    [Export]
    public float _max_health;
    [Export]
    public float _health;
    [Export]
    public float _resistance;

    public float MaxHealth { get => _max_health; set => _max_health = value; }
    public float Health { get => _health; set => _health = value; }
    public float Resistance { get => _resistance; set => _resistance = value; }

    public override void _Ready()
    {
        
    }

    public void Kill()
    {
        //sIDamagable.Health = 0;
        this.QueueFree();
    }
}
