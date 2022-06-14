using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Incline.addons.christopher.tree.Behaviours
{
    public class WarpBehaviour : Behaviour
    {
        [Export]
        protected string destination;

        public override int Next { 
            get {
                if (CheckCondition()) return bp.BehaviorIndex[destination];
                return next;
            } 
            set => next = value;
        }

        public override bool Run()
        {
            return true;
        }
    }
}
