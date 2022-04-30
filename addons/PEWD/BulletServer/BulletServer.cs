using Godot;
using System;
//using System.Windows;
//using System.Numerics;

public enum KillCrit : byte
{
	None = 0,
	Dist = 1,
	Time = 2
}	

public struct BulletData
{
	public float damage {get; set;}
	public KillCrit crit_type {get; set;}
	public float distance {get; set;}
	public float d_sq {get; set;}
	public float speed {get; set;}
	public float despawn_timer {get; set;}
	public Transform area_tm {get; set;}
}

public struct BulletInstanceData
{
	public Vector3 vel;
	public bool moving;
	public float start_time;
	public Vector3 start_pos;
	public string owner;
}

public class BulletServer : Spatial
{
	private Spatial meshes;
	private Spatial bullets;
	private Spatial primitives;

	public bool running = true;
	
	private Godot.Collections.Dictionary types = new Godot.Collections.Dictionary();
	private Godot.Collections.Dictionary data = new Godot.Collections.Dictionary();
	private Godot.Collections.Dictionary mmi = new Godot.Collections.Dictionary();
	private Godot.Collections.Dictionary areas = new Godot.Collections.Dictionary();
	private Godot.Collections.Dictionary primitiveBullets = new Godot.Collections.Dictionary();

	public int RegisterType(string name, float damage,
		KillCrit crit_type, float distance, float speed, float despawn_timer,
		Transform area_tm, Mesh m)
	{
		BulletData temp = new BulletData();
		temp.damage = damage;
		temp.crit_type = crit_type;
		temp.distance = distance;
		temp.speed = speed;
		temp.despawn_timer = despawn_timer;
		temp.area_tm = area_tm;
		return RegisterType(name, temp, m);
	}
	public int RegisterType(string name, BulletData bd, Mesh m)
	{
		if (types.Contains(name)) return (int) types[name];
		types.Add(name, types.Count);
		bd.d_sq = bd.distance*bd.distance;
		data.Add(types.Count-1, bd);
		MultiMeshInstance temp = new MultiMeshInstance();
		temp.Name = name;
		MultiMesh mm = new MultiMesh();
		mm.Mesh = m;
		temp.Multimesh = mm;
		mmi.Add(mmi.Count, temp);
		meshes.AddChild((MultiMeshInstance) mmi[mmi.Count-1]);
		Spatial temp2 = new Spatial();
		temp2.Name = name;
		areas.Add(areas.Count, temp2);
		bullets.AddChild((Spatial) areas[areas.Count-1]);
		return types.Count-1;
	}

	public int RegisterPrimitiveBullet(PackedScene b)
	{
		primitiveBullets.Add(primitiveBullets.Count, b);
		return primitiveBullets.Count-1;
	}

	public void SpawnPrimitiveBullet(int id)
	{
		primitives.AddChild((Spatial) primitiveBullets[id]);
	}

	public int RegisterSmartBullet(PackedScene b, Mesh m)
	{
		
	}

	public void CreateBullet(int bullet_id, string owner, Transform t)
	{
		MultiMesh mm = ((MultiMeshInstance)mmi[bullet_id]).Multimesh;
		mm.InstanceCount++;
		mm.SetInstanceTransform(mm.InstanceCount-1, t);
		((MultiMeshInstance)mmi[bullet_id]).Multimesh = mm;
	}

	public override void _Ready()
	{
		base._Ready();
		meshes = new Spatial();
		meshes.Name = "Meshes";
		AddChild(meshes);

		bullets = new Spatial();
		bullets.Name = "Bullets";
		AddChild(bullets);

		primitives = new Spatial();
		primitives.Name = "Primitives";
		AddChild(primitives);
	}
}