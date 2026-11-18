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
    public class MissileLauncher : WeaponBase
{
    public int missilesReady;
    public int Count = 1;
    public float RELOAD_TIMER = 6f;
    public float reload;

    public MissileLauncher() : base(0f)
    {
        name = "Missile Bay";
        cost = 100;
        unlocked = false;
        missilesReady = Count;
        reload = RELOAD_TIMER;
    }

    public override bool TryFireMissile(Vector2 location, List<Missile> projectiles)
    {
        if (missilesReady <= 0) return false;
        Fire(location, projectiles);
        return true;
    }

    protected override void Fire(Vector2 location, List<Missile> projectiles)
    {
        Missile missile = new Missile(location);
        missile.direction = new Vector2(0, 4f);   
        missilesReady--;
        projectiles.Add(missile);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (missilesReady >= Count) { reload = RELOAD_TIMER; return; }

        reload -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (reload <= 0)
        {
            missilesReady++;
            reload = RELOAD_TIMER;
        }
    }
}
}