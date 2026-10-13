using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpacerGamer.Collisions;

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
        

    }
}