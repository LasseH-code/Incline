using Godot;
using System;
using System.Collections.Generic;

//using System.Windows;
//using System.Numerics;

namespace Incline.addons.PEWD.Bullet
{
    /*/// <summary>
    /// For General information about Smart Bullets
    /// </summary>
    public struct SmartBulletTypeData
    {
        public string _name;
        public Mesh _m;
        public float _max_distance;
        public float _damage;
        public float _speed;
        public Script[] _custom_actions;
    }*/         // Irrelevant
    /// <summary>
    /// Centralized Hub for damage and projectiles
    /// </summary>
    public class BulletServer : Spatial
    {
        // structs for server handled bullets
        /// <summary>
        /// For Modifications to a bullet
        /// </summary>
        internal struct BulletModData
        {
            public BulletModData(BulletModData m) { Set(m); }
            public BulletModData(float df, float sf, float mdf, Script[] cam) { Set(df, sf, mdf, cam); }
            public float _damage_factor = 0.0f;
            public float _speed_factor = 0.0f;
            public float _max_distance_factor = 0.0f;
            internal float Df() => _damage_factor;
            internal float Sf() => _speed_factor;
            internal float Mdf() => _max_distance_factor;
            public Script[] _custom_actions_mods = null;
            internal Script[] Cam() => _custom_actions_mods;

            public void Set(float df, float sf, float mdf, Script[] cam)
            {
                _damage_factor = df;
                _speed_factor = sf;
                _max_distance_factor = mdf;
                _custom_actions_mods = cam;
            }
            public void Set(BulletModData m) => Set(m.Df(), m.Sf(), m.Mdf(), m.Cam());

#if COMPLEX_BULLET_MODS
            internal Script[] AddScriptArrays(Script[] a, Script[] b)
            {
                Script[] ret = new Script[a.Length + b.Length];
                for (int i = 0; i < a.Length; i++) ret[i] = a[i];
                for (int i = 0; i < b.Length; i++) ret[a.Length + i] = b[i];
                return ret;
            }
            public void Add(BulletModData m) =>
                Set(m.Df() + Df(),
                    m.Sf() + Sf(),
                    m.Mdf() + Mdf(),
                    AddScriptArrays(m.Cam(), Cam()));
            public void Sub(BulletModData m) =>
                 Set(m.Df() - Df(),
                     m.Sf() - Sf(),
                     m.Mdf() - Mdf(),
                     Cam());
            public void mul(BulletModData m) =>
                Set(m.Df() * Df(),
                    m.Sf() * Sf(),
                    m.Mdf() * Mdf(),
                    Cam());
            public void div(BulletModData m) =>
                Set(Df() / m.Df(),
                    Sf() / m.Sf(),
                    Mdf() / m.Mdf(),
                    Cam());
#endif
        }
        /// <summary>
        /// For instances of Smart Bullets
        /// </summary>
        internal struct SmartBulletInstanceData
        {
            public SmartBulletInstanceData(int t_id, Vector3 s_tr, BulletModData m)
            {
                _type_id = t_id;
                _start_translation = s_tr;
                _translation = _start_translation;
                _mod = m;
            }
            public int _type_id;
            public Vector3 _translation;
            public Vector3 _start_translation;
            public BulletModData _mod;
        }

        // flags
        public bool _is_paused;

        // important nodes
        internal Spatial _meshes_node;
        internal Spatial _bullet_domain_node;

        // server handled projectiles
        internal List<BulletResource> _bulletTypes = new List<BulletResource>();
        internal BulletResource[] _bulletTypesArr;
        internal MultiMesh[] _smartBulletMeshes;
        //internal List<SmartBulletInstanceData> _smart_bullet_instances = new List<SmartBulletInstanceData>();
        //internal SmartBulletInstanceData[] _smart_bullet_instances_arr;     // May not use

        internal int AddBulletType(BulletResource r)
        {
            r.SetTypeId(_bulletTypesArr.Length, true);
            _bulletTypes.Add(r);
            _bulletTypesArr = _bulletTypes.ToArray();
            return _bulletTypesArr.Length-1;
        }

        public int RegisterBulletType(BulletResource r)
        {
            return AddBulletType(r);
        }

        public void SpawnSmartBullet(int id)
        {
            if (id >= _bulletTypesArr.Length) return;       // Maybe trigger Exeption???
            GD.Print(_bulletTypesArr[id].ToString());
        }

        public override void _Ready()
        {
            _meshes_node = new Spatial();
            _bullet_domain_node = new Spatial();
            _meshes_node.Name = "Meshes";
            _bullet_domain_node.Name = "Bullet Domain";
            AddChild(_meshes_node);
            AddChild(_bullet_domain_node);
        }
    }

}