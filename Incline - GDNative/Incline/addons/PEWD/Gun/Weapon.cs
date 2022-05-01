using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Incline.addons.PEWD.Bullet;
//using Incline.addons.PEWD;

namespace Incline.addons.PEWD.Gun
{
    internal class Weapon : Spatial
    {
        internal const string bs_path = "/root/" + nameof(BulletServer);
        internal BulletServer bs;

        [Export]
        internal BulletResource br;

        /*[Signal]
        public delegate int RegisterBullet(BulletResource r);*/


        public override void _Ready()
        {
            bs = ((BulletServer)GetNode(bs_path));
            //br.Connect("IdChanged", this, "_OnIdChanged");
            if (br._typeId == -1) bs.RegisterBulletType(br);
            //Connect("RegisterBullet", BulletServer, "_OnRegsiterBullet");
            bs.SpawnSmartBullet(br._typeId);
        }
    }
}
