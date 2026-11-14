using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SpacerGamer.Collisions;
using System.Threading;
using System.Net.Http.Headers;


namespace SpacerGamer
{

    public class Heart
    {
        
    
        private const float ANIMATION_SPEED = 0.1f;

        private double animationTimer;

        private int animationFrame; 
        private Vector2 position;
        public Vector2 Position
        {
            get => position;
            set
            {
                position = value;
                bounds.Center = position;
            }
        }

        private Texture2D texture;

        private BoundingCircle bounds;
        /// <summary>
        /// The bounding volume of the Hammer
        /// </summary>
        public BoundingCircle Bounds => bounds;

        public bool Collected {get; set;} = false;

        public Heart(Vector2 position)
        {
            this.bounds = new BoundingCircle(position, 9);
            this.Position = position;
        }
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Hammer");
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(Collected) return;
            var source = new Rectangle(0, 0, 32, 32);
            spriteBatch.Draw(texture, Position, source, Color.White, 0, new Vector2(16, 16), 1f, SpriteEffects.None, 0f);
        }
    }
}