using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot; 

namespace Incline.addons.christopher.tree
{
    public class Behaviour : Resource
    {
        public enum BehaviourMode { EXPRESSION, SCRIPT, TRUE }
        [Export]
        protected BehaviourMode mode;
        //public BehaviourMode Mode { get => mode; set => mode = value; }
        [Export]
        public string expression;
        protected Expression e = new Expression();
        [Export]
        public Script script;
        /*[Export]
        public NodePath node;*/
        
        public BehaviourPath bp;
        protected int next;
        public virtual int Next { get => next; set => next = value; }

        protected bool CheckCondition()
        {
            switch (mode)
            {
                case BehaviourMode.TRUE: return true;
                case BehaviourMode.SCRIPT: 
                    if ((bool)script.Call("CheckCondition") == true) return true; 
                    break;
                //case BehaviourMode.NODE: if ((bool)(node).Call("CheckCondition") == true) return true; else break;
                case BehaviourMode.EXPRESSION:
                    e.Parse(expression);
                    if ((bool)e.Execute()) return true;
                    break;
            }
            return false;
        }

        protected virtual void Action()
        {

        }

        public virtual bool Run()
        {
            Action();
            return CheckCondition();
        }
    }
}
