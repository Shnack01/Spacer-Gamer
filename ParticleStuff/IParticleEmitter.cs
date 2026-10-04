using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacerGamer.ParticleStuff
{
    public interface IParticleEmitter
    {
        public Vector2 Position {get;}

        public Vector2 Velocity {get;}
    }
}