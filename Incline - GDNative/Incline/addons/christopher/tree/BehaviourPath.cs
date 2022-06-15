using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Incline.addons.christopher.tree
{
    public class BehaviourPath : Node
    {
        /*[Export]
        protected Godot.Collections.Array<Behaviour> behaviours;*/
        [Export]
        protected Behaviour[] bs;
        public Behaviour[] Behaviours { get => bs; }
        /*protected Godot.Collections.Dictionary<string, Resource> behaviours;
        public Godot.Collections.Dictionary<string, Resource> Behaviours { get => behaviours; }*/
        protected Dictionary<string, int> behaviorIndex;
        public Dictionary<string, int> BehaviorIndex { get => behaviorIndex; }

        /*[Export]
        protected Godot.Collections.Array<string> code;*/

        [Export]
        public bool running = true;

        [Export]
        public int start_index = 0;

        public int behaviour_pointer;

        public override void _EnterTree()
        {
            GD.Print("test-2");
        }

        public override void _Ready()
        {
            GD.Print("test-1");
            behaviour_pointer = start_index;
            behaviorIndex = new Dictionary<string, int>();

            GD.Print("test");

            Refreshbehaviours();
        }

        public void Refreshbehaviours()
        {
            //behaviorIndex.Clear();
            for (int i = 0; i < bs.Length; i++)
            {
                GD.Print("test2");
                Behaviour r = bs[i];
                //Behaviour b = (Behaviour)r;
                r.bp = this;
                r.Next = i + 1 % bs.Length;
                //behaviorIndex.Add(r.tag, i);
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            if (running)
            {
                Behaviour b = bs[behaviour_pointer];
                if (b.Run())
                {
                    behaviour_pointer = b.Next;
                }
            }
        }
    }
}
