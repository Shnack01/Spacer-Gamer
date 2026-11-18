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


        // Toggle this off when you're done tuning the bounds
        public static bool DebugDrawBounds = true;
        private static Texture2D pixel;
        
        private Vector2 boundsOffset = new Vector2(-4f, -4f);

        public Missile(Vector2 shipLocation)
        {
            this.position = shipLocation;
            this.bounds = new BoundingRectangle(position + boundsOffset, 7, 10);
        }

        public void Update(GameTime gameTime)
        {
            this.position -= direction;
            this.bounds.X = position.X + boundsOffset.X;
            this.bounds.Y = position.Y + boundsOffset.Y;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Missile");
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

             if (DebugDrawBounds) DrawBounds(spriteBatch);
        }

        private void DrawBounds(SpriteBatch spriteBatch)
        {
            if (pixel == null)
            {
                pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                pixel.SetData(new[] { Color.White });
            }

            int x = (int)bounds.X;
            int y = (int)bounds.Y;
            int w = (int)bounds.Width;
            int h = (int)bounds.Height;
            const int t = 1; // line thickness

            spriteBatch.Draw(pixel, new Rectangle(x, y, w, t), Color.Red);         // top
            spriteBatch.Draw(pixel, new Rectangle(x, y + h - t, w, t), Color.Red); // bottom
            spriteBatch.Draw(pixel, new Rectangle(x, y, t, h), Color.Red);         // left
            spriteBatch.Draw(pixel, new Rectangle(x + w - t, y, t, h), Color.Red); // right
        }
    }
}