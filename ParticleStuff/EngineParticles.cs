using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpacerGamer.ParticleStuff;

namespace SpacerGamer.ParticleStuff
{
    
    public class EngineParticle : ParticleSystem
    {
        IParticleEmitter _emitter;
        public EngineParticle(Game game, IParticleEmitter emitter) : base(game, 2000)
        {
            _emitter = emitter;
        }


        protected override void InitializeConstants()
        {
            textureFilename = "particle";

            minNumParticles = 2;
            maxNumParticles = 5;

            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            where += new Vector2(RandomHelper.NextFloat(-4f, 4f));
            var velocity = new Vector2(RandomHelper.NextFloat(-25f, 25f), RandomHelper.NextFloat(120f, 220f)) + _emitter.Velocity * 0.25f;
            var Acceleration = new Vector2(0, RandomHelper.NextFloat(-60f, 20f));
            var scale = RandomHelper.NextFloat(0.1f, 0.5f);

            var lifetime = RandomHelper.NextFloat(0.15f, .5f);

            p.Initialize(where, velocity, Acceleration, Color.OrangeRed, scale: scale, lifetime: lifetime);

            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            AddParticles(_emitter.Position);
        }
    }
}