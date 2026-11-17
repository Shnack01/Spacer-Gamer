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
    public class MissileLauncher : WeaponBase{
        public int missilesReady = 1;

        public int Count = 1;

        public float RELOAD_TIMER = 6f;
        public float reload;
        public MissileLauncher() : base(0f)
        {
            name = "Missile Bay";
            cost = 100;
            unlocked = false;
            reload = RELOAD_TIMER;
        }

        protected override void Fire(Vector2 location, List<Missile> list)
        {
            Missile missile = new Missile(location);
            missile.direction = new Vector2(0, 4f);
            list.Add(missile);
        }
        public new bool TryFire(Vector2 location, List<Missile> projectiles)
        {
            if (timer > 0) return false;
            if(missilesReady >= 1)
            {
                timer = cooldown;
                Fire(location, projectiles);
            }
            
            return true;
        }
        public new void Update(GameTime gameTime)
        {
            if (timer > 0) timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            //add the timer for the cooldown, should always be cooling down if MissilesReady != count
        }
    }
}