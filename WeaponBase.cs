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
    public abstract class WeaponBase
    {
        protected float cooldown;
        protected float timer;
        protected WeaponBase(float cooldown)
        {
            this.cooldown = cooldown;
        }
        public void Update(GameTime gt)
    {
        if (timer > 0) timer -= (float)gt.ElapsedGameTime.TotalSeconds;
    }

    public bool TryFire(Vector2 muzzle, List<Laser> projectiles)
    {
        if (timer > 0) return false;
        timer = cooldown;
        Fire(muzzle, projectiles);
        return true;
    }

    protected abstract void Fire(Vector2 muzzle, List<Laser> projectiles);

    }
}