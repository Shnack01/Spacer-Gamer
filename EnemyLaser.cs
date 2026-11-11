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
        private Color color = Color.White;
        private Rectangle laserBounds;
        private MouseState mouseState = new MouseState();

        public float velocity = 8f;
        private Texture2D texture;
        private Vector2 position;
        public bool used;
        public Vector2 Posision => position;
        public float sizeMult = 1f;

        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;
        public EnemyLaser(Vector2 shipLocation)
        {
            this.position = shipLocation;
            this.bounds = new BoundingRectangle(position, 2 * sizeMult, 16 * sizeMult);
            laserBounds = new Rectangle((int)position.X, (int)position.Y, (int)(2 * sizeMult), (int)(16 * sizeMult));
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
            color = Color.White;
            this.position.Y += velocity;
            this.bounds.X = position.X;
            this.bounds.Y = position.Y;
             laserBounds.X = (int)bounds.X;
            laserBounds.Y = (int)bounds.Y;

            mouseState = Mouse.GetState();
            if (laserBounds.Contains(mouseState.Position))
            {
                color = Color.Blue;
            }
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
            spriteBatch.Draw(texture, position, source, color, 0, origin, 3f * sizeMult, SpriteEffects.None, 0);
        }
        
    }
}