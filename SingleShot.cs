using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using SpacerGamer.Collisions;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using SpacerGamer.ParticleStuff;

namespace SpacerGamer
{
    public class SingleShot : WeaponBase
    {
        protected float cooldown = 1f;
        protected float timer;
        public SingleShot() : base(.25f)
        {
            
        }

        protected override void Fire(Vector2 location, List<Laser> list)
        {
            list.Add(new Laser(location));
        }
    }
}