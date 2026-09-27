using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SpacerGamer.Collisions;

namespace SpacerGamer{


    public class EnemyLaser
    {
        private Texture2D texture;
        private Vector2 position;
        public bool used;
        public Vector2 Posision => position;

        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;
        public EnemyLaser(Vector2 shipLocation)
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
            texture = content.Load<Texture2D>("Laser");
            
        }
        public void Update(GameTime gameTime)
        {
            this.position.Y += 8f;
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
            if(used) return;
            var source = new Rectangle(16, 0, 2, 16);
            Vector2 origin = new Vector2(1f, 8f);
            spriteBatch.Draw(texture, position, source, Color.White, 0, origin, 3f, SpriteEffects.None, 0);
        }
    }
}