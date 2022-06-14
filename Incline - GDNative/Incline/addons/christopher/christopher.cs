# if TOOLS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

[Tool]
public class christopher : EditorPlugin
{
    private const string PATH = "res://addons/christopher/";

    private List<string> types = new List<string>();

    private void add_type(string type, string @base, string script, string icon)
    {
        AddCustomType(type, @base, GD.Load<Script>(script), GD.Load<Texture>(icon));
        types.Add(type);
    }

    public override void _EnterTree()
    {
        add_type("BehaviorPath", "Node", PATH + "tree/BehaviourPath.cs", null);
        add_type("Behavior", "Resource", PATH + "tree/Behaviour.cs", null);
    }

    public override void _ExitTree()
    {
        foreach (string type in types)
        {
            RemoveCustomType(type);
        }
        types.Clear();
    }
}
# endif
