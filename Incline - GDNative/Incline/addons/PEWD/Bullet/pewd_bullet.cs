using Godot;
using System;

public class pewd_bullet : Spatial
{
	// NODE

	private PackedScene bullet;

	private Timer timer;

	public override void _EnterTree()
	{
		base._EnterTree();
		bullet = ResourceLoader.Load("res://addons/PEWD/Bullet/Holder.tscn") as PackedScene;
		AddChild(bullet.Instance());
		timer = new Timer();
	}

	// SCRIPT

	private Spatial holder;
	//private Spatial coll_particles;
	private Area area;

	[Export]
	private float damage = 1.0f;

	enum KillCrit : byte
	{
		None = 0,
		Dist = 1,
		Time = 2
	}

	[Export]
	private KillCrit criteria_type = KillCrit.Dist;

	[Export]
	private float distance = 20.0f;

	[Export]
	private float speed = 15.0f;

	[Export]
	private float despawn_timer = 1.0f;

	[Export]
	private NodePath[] hit_particles;

	[Export]
	private NodePath[] idle_particles;

	[Export]
	private NodePath[] hit_audio; // UNTESTED

	[Export]
	private NodePath[] idle_audio; // UNTESTED

	[Export]
	private bool do_custom_impact; // UNTESTED

	[Export]
	private Script custom_impact; // UNTESTED


	private Vector3 vel; // = new Vector3(0,0,-speed);
	private bool moving = true;
	private float start_time;
	private Vector3 start_pos;
	private float d_sq;

	// Called when the node enters the scene tree for the first time.

	public void Kill()
	{
		if (moving)
		{
			moving = false;
			area.Visible = false;

			foreach (NodePath p in hit_particles)
			{
				switch (GetNode(p))
				{
					case Particles particles:
						particles.Emitting = true;
						break;
					case CPUParticles particles:
						particles.Emitting = true;
						break;
				}
				/*var temp = GetNode(p);
				if (temp.GetType() is Particles)
				{
					((Particles)temp).Emitting = true;
				}
				else if (temp.GetType() is CPUParticles)
				{
					((CPUParticles)temp).Emitting = true;
				}*/

			}
			foreach (NodePath p in idle_particles)
			{
				Particles temp = GetNode<Particles>(p);
				temp.Emitting = false;
			}
			foreach (NodePath a in hit_audio)
			{
				AudioStreamPlayer3D temp = GetNode<AudioStreamPlayer3D>(a);
				temp.Play();
			}
			foreach (NodePath a in idle_audio)
			{
				AudioStreamPlayer3D temp = GetNode<AudioStreamPlayer3D>(a);
				temp.Stop();
			}

			timer.Start(despawn_timer);
		}
	}

	private void TestKillCriteria()
	{
		switch (criteria_type)
		{
			case KillCrit.Dist:
				if (holder.Translation.DistanceSquaredTo(start_pos) > d_sq) Kill();
				break;
			case KillCrit.Time:
				if (start_time + distance < OS.GetUnixTime()) Kill();
				break;
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);
		//if (moving) holder.Translate(vel * delta);
		//TestKillCriteria();
	}

	public override void _Ready()
	{
		base._Ready();
		vel = new Vector3(0, 0, -speed);
		d_sq = distance * distance;

		holder = GetNode<Spatial>("Holder");
		//coll_particles = GetNode<Spatial>("Particles/Collison")
		area = GetNode<Area>("Holder/Area");
		timer = GetNode<Timer>("Timer");

		switch (criteria_type)
		{
			case KillCrit.Dist:
				start_pos = holder.Translation;
				break;
			case KillCrit.Time:
				start_time = OS.GetUnixTime();
				break;
		}
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }

	private void _on_Area_body_entered(object body) // Maybe move a scope up??
	{
		if (!do_custom_impact)
		{
			Kill();
		}
		else
		{
			custom_impact.Call("action", this);
		}
	}

	private void _on_Timer_timeout()
	{
		QueueFree();
	}
}
