using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Incline.addons.PEWD.Bullet
{
    public class BulletResource : Resource
    {
        /*[Signal]
        public delegate void IdChanged(int id);     // Untested.. Weapon has to connnect this to itself!*/

        [Export]
        public string name = "";
        [Export]
        public float damage = 0.0f;
        [Export]
        public float speed = 0.0f;
        [Export]
        public float max_distance = 0.0f;
        [Export]
        public Mesh m = null;
        [Export]
        public Script[] custom_actions = null;

        internal int _typeId = -1;
        public bool SetTypeId(int id, bool forceChange = true)
        {
            if (!forceChange && _typeId != -1) return false;
            _typeId = id;
            //EmitSignal("IdChanged", _typeId);
            return true;
        }

        public override string ToString()
        {
            return String.Format("{0}name = {1}, damage = {2}f, speed = {3}f, max_distance = {4}, m = {5}, custom_action = {6}{7}",
                '{', name, damage, speed, max_distance, m.ToString(), custom_actions.ToString());
        }
    }
}
