# if TOOLS
using Godot;
using System;

[Tool]
public class pewd : EditorPlugin
{
	private const string path = "res://addons/PEWD/"; // Base path for addon resources

	private Control pewd_edtor_panel; // Editor Panel

	// Fuck this shit

	public override void _EnterTree()
	{
		/// Type defnitions ///
		//AddCustomType("Gun", "Spatial", GD.Load<Script>(path+"Gun/Gun.cs"), GD.Load<Texture>(path+"Gun/Gun Icon Optimized.svg"));
		//AddCustomType("HitscanGun", "Spatial", GD.Load<Script>(path+"Gun/HitscanGun.cs"), GD.Load<Texture>(path+"Gun/HitscanOptimized.svg"));
		//AddCustomType("PhysicsGun", "Spatial", GD.Load<Script>(path+"Gun/PhysicsGun.cs"), GD.Load<Texture>(path+"Gun/PhysicsOptimized.svg"));
		//AddCustomType("Bullet", "Spatial", GD.Load<Script>(path+"Bullet/pewd_bullet.cs"), GD.Load<Texture>(path+"Bullet/BulletOptimized.svg"));

		AddCustomType("PRBullet", "RayCast", GD.Load<Script>(path + "Bullet/New/PRBullet.cs"), GD.Load<Texture>(path + "Bullet/BulletOptimized.svg"));
		
		AddCustomType("BulletResource", "Resource", GD.Load<Script>(path + "Bullet/BulletResource.cs"), GD.Load<Texture>(path + "Bullet/BulletOptimized.svg"));
		AddCustomType("BulletResourceSimple", "Resource", GD.Load<Script>(path + "Bullet/BulletResourceSimple.cs"), GD.Load<Texture>(path + "Bullet/BulletOptimized.svg"));
		AddCustomType("GunResource", "Resource", GD.Load<Script>(path + "Gun/GunResource.cs"), GD.Load<Texture>(path + "Gun/Gun Icon Optimized.svg"));

		//AddCustomType("DamagableKinematicBody", "KinematicBody", GD.Load<Script>(path + "Health/DamagablePhysicsBody.cs"), GD.Load<Texture>(null));

		/// Singeton Definitions ///
		//AddAutoloadSingleton("BulletServer", path+ "BulletServer/BulletServer.cs");
		//AddAutoloadSingleton("BulletManager", path + "Bullet/BulletManager.cs");

		/// Adding Editor panel ///
		pewd_edtor_panel = ((Control) GD.Load<PackedScene>("res://addons/PEWD/Gun/Creator/Gun Creator.tscn").Instance()); // Instanciating Edior Panel
		GetEditorInterface().GetEditorViewport().GetParent().AddChild(pewd_edtor_panel); // Adding Editor Panel
		MakeVisible(false); // Hiding Editor Panel
	}

	public override void _ExitTree()
	{
		/// Removing Custom Types ///
		//RemoveCustomType("Gun");
		//RemoveCustomType("HitscanGun");
		//RemoveCustomType("PhysicsGun");
		//RemoveCustomType("Bullet");

		RemoveCustomType("PRBullet");

		RemoveCustomType("BulletResource");
		RemoveCustomType("BulletResourceSimple");
		RemoveCustomType("GunResource");

		//RemoveCustomType("DamagableKinematicBody");

		/// Removing Autoload Singletons ///
		//RemoveAutoloadSingleton("BulletServer");
		//RemoveAutoloadSingleton("BulletManager");

		/// Deleting Editor Panel ///
		if (pewd_edtor_panel != null) pewd_edtor_panel.QueueFree();
	}

	public override void MakeVisible(bool visible) // Sets Visibility for the Editor Panel
	{
		if (pewd_edtor_panel != null) pewd_edtor_panel.Visible = visible;
	}

	public override bool HasMainScreen() { return true;  } // Tells Godot, to add A new Editor Tab. Returning false, disables the P.E.W.D. Tab

	public override string GetPluginName() { return "P.E.W.D."; } // Returns the addons Name

	public override Texture GetPluginIcon() { return GD.Load<Texture>(path+"Gun/Gun Icon Optimized.svg"); } // Returns the Addons Icon
}
# endif
