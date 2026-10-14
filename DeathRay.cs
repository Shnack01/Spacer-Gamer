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
    public class DeathRay : WeaponBase{
        public DeathRay() : base(0f)
        {
            
        }

        protected override void Fire(Vector2 location, List<Laser> list)
        {
            Laser ray = new Laser(location);
            ray.direction = new Vector2(0, 4f);
            list.Add(ray);
        }
    }
}