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
        [Export]
        protected Godot.Collections.Dictionary<string, Behaviour> behaviours;
        public Godot.Collections.Dictionary<string, Behaviour> Behaviours { get => behaviours; }
        protected Godot.Collections.Dictionary<string, int> behaviorIndex;
        public Godot.Collections.Dictionary<string, int> BehaviorIndex { get => behaviorIndex; }
        [Export]
        public bool running = true;

        [Export]
        public int start_index = 0;

        public int behaviour_pointer;

        public override void _Ready()
        {
            behaviour_pointer = start_index;

            Refreshbehaviours();
        }

        public void Refreshbehaviours()
        {
            BehaviorIndex.Clear();
            for (int i = 0; i < behaviours.Count; i++)
            {
                Behaviour b = behaviours.ElementAt(i).Value;
                b.bp = this;
                b.Next = i + 1 % behaviours.Count;
                BehaviorIndex.Add(behaviours.ElementAt(i).Key, i);
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            if (running)
            {
                Behaviour b = behaviours.ElementAt(behaviour_pointer).Value;
                if (b.Run())
                {
                    behaviour_pointer = b.Next;
                }
            }
        }
    }
}
