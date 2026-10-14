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
    public class SplitShot : WeaponBase{
        public SplitShot() : base(.5f)
        {}

        protected override void Fire(Vector2 location, List<Laser> list)
        {
            Laser toLeft = new Laser(location);
            toLeft.direction = new Vector2(2f, 8f);
            Laser toRight = new Laser(location);
            toRight.direction = new Vector2(-2f, 8f);
            list.Add(new Laser(location));
            list.Add(toLeft);
            list.Add(toRight);

        }
    }
}