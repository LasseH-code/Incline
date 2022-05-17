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

    private MeshInstance mesh;
    private ShaderMaterial shader;
    private Timer flashTimer;
    private Tween t;

    public override void _Ready()
    {
        mesh = GetNode<MeshInstance>("MeshInstance");
        shader = (ShaderMaterial)mesh.Mesh.SurfaceGetMaterial(0);
        flashTimer = GetNode<Timer>("FlashTimer");
        flashTimer.Connect("timeout", this, "_on_FlashTimer_timeout");
        t = GetNode<Tween>("Flash");
        //shader = (ShaderMaterial) shader.Duplicate();
    }

    public void Kill()
    {
        //sIDamagable.Health = 0;
        this.QueueFree();
    }

    public void DamageEffect()
    {
        flashTimer.Stop();
        t.InterpolateProperty(shader, "shader_param/flash_state", 0, 1, (float)flashTimer.WaitTime, Tween.TransitionType.Linear, Tween.EaseType.InOut);
        t.Start();
        flashTimer.Start(flashTimer.WaitTime);
    }

    public void _on_FlashTimer_timeout()
    {
        flashTimer.Stop();
        t.InterpolateProperty(shader, "shader_param/flash_state", 1, 0, (float)flashTimer.WaitTime, Tween.TransitionType.Linear, Tween.EaseType.InOut);
        t.Start();
    }
}
