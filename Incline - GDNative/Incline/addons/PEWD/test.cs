using Godot;
using System;

public class test : Button
{
	public override void _EnterTree()
	{
		base._EnterTree();
		Connect("pressed", this, "_on_pressed");
	}

	public void _on_pressed()
	{
		GD.Print("You clicked me!");
	}
}
