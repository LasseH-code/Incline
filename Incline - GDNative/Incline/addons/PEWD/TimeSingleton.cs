using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Incline.addons.PEWD
{
    public class TimeSingleton : Node
    {
        private static float standard_time_scale = 1.0f;
        public static float StandardTimeScale { get => standard_time_scale; set => standard_time_scale = value; }
        public static void ChangeSpeed(float s)
        {
            Engine.TimeScale = s;
        }
        public static void ResetSpeed()
        {
            Engine.TimeScale = standard_time_scale;
        }
        public static float Adjust(float a)
        {
            return a*Engine.TimeScale;
        }
        public static Vector2 Adjust(Vector2 a)
        {
            return a * Engine.TimeScale;
        }
        public static Vector3 Adjust(Vector3 a)
        {
            return a * Engine.TimeScale;
        }
    }
}
