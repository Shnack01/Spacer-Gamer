using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SpacerGamer.Collisions;

namespace SpacerGamer{


    public class Missile
    {
        public Vector2 direction = new Vector2(0, 8f);
        private Texture2D texture;
        private Vector2 position;
        public bool used;
        public Vector2 Posision => position;

        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;
        
        public Missile(Vector2 shipLocation)
        {
            this.position = shipLocation;
            this.bounds = new BoundingRectangle(position, 2, 16);
            
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Missile");
        }
        public void Update(GameTime gameTime)
        {
            
            this.position -= direction;
            this.bounds.X = position.X;
            this.bounds.Y = position.Y;
        }


        /// <summary>
        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (used) return;
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 0.75f, SpriteEffects.None, 0f);
        }
    }
}