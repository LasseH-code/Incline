using Godot;
using System;
using System.Collections.Generic;
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
	public string name {get; set;}
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

public struct SmartBullet
{
	public int mesh_id;
	public int primitive_id;
}

public class BulletServerShittyAndBroken : Spatial
{
	/*
	private Spatial meshes; // multimesh instances for server-handled and smart bullets
	private Spatial bullets; // bullet instances for server-handled and smart bullets
	private Spatial primitives; // holder for instances of primitive bullets

	public bool running = true;

	// Type for server-handled bullets
	List<BulletData> types = new List<BulletData>();
	// data for server-handled bullets
	private Godot.Collections.Dictionary data = new Godot.Collections.Dictionary();
	// meshes for server-handled and smart bullets
	private Godot.Collections.Dictionary mmi = new Godot.Collections.Dictionary();
	// areas for server-handled bullets - MAY REMOVE LATER
	private Godot.Collections.Dictionary areas = new Godot.Collections.Dictionary();
	// primitive bullet types
	private Godot.Collections.Dictionary primitiveBullets = new Godot.Collections.Dictionary();
	// smart bullet types
	private Godot.Collections.Dictionary smartBullets = new Godot.Collections.Dictionary();


	//List<PackedScene> test = new List<PackedScene>();

	// TODO: REDO server handled bullets:
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
		data.Add(bd);
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
	// server handled bullets

	public int RegisterPrimitiveBullet(PackedScene b)
	{
		//primitiveBullets.Add(primitiveBullets.Count, (PackedScene)ResourceLoader.Load(b));
		primitiveBullets.Add(primitiveBullets.Count, (PackedScene)ResourceLoader.Load(b));
		return primitiveBullets.Count-1;
	}

	public void SpawnPrimitiveBullet(int id, Transform t)
	{
		var temp = (Spatial)primitiveBullets[id].Instance();
		temp.Transform = t;
		primitives.AddChild(temp);
	}

	public SmartBullet RegisterSmartBullet(PackedScene b, Mesh m)
	{
		smartBullets.Add(primitiveBullets.Count, b);
		SmartBullet data = new SmartBullet();
		data.primitive_id = smartBullets.Count-1;
		MultiMesh mm = new MultiMesh();
		mm.Mesh = m;
		MultiMeshInstance temp = new MultiMeshInstance();
		temp.MultiMesh = mm;
		temp.name = mmi.Count;
		mmi.Add(mmi.Count, temp);
		data.mesh_id = mmi.Count-1;
		return data;
		//return new SmartBullet();
	}

	public void SpawnSmartBullet(SmartBullet sb, Transform t)
	{
		//meshes[sb.mesh_id].InstanceCount++;
		//meshes[sb.mesh_id].SetInstanceTransform
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

	/*
		root
		>	BulletServer
			|>
	*/
}
