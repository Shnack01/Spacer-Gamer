using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpacerGamer.Collisions;

namespace SpacerGamer
{
    public abstract class ShipBase : IShip
    {
        // Shared state
        protected Vector2 position;
        protected Texture2D texture;
        protected BoundingRectangle bounds = new BoundingRectangle(Vector2.Zero, 1, 1);
        protected readonly string textureName;
        protected readonly float scale;
        protected readonly float hitboxScale;
        protected Vector2 boundsOffset = Vector2.Zero;
        protected Vector2 muzzleOffset = new Vector2(0, -24f);

        public event Action<Vector2> LaserFired;

        public int MaxHealth { get; }
        public int Health { get; set; }
        public bool Destroyed { get; protected set; }
        public Color Color { get; set; } = Color.White;

        public Vector2 Position => position;
        public BoundingRectangle Bounds => bounds;

        protected ShipBase(Vector2 startPosition, int maxHealth, string textureName,
                           float scale = 3f, float hitboxScale = 0.6f)
        {
            position = startPosition;
            MaxHealth = maxHealth;
            Health = maxHealth;
            this.textureName = textureName;
            this.scale = scale;
            this.hitboxScale = hitboxScale;
        }

        public virtual void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(textureName);
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Destroyed || texture == null) return;
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            spriteBatch.Draw(texture, position, null, Color, 0f, origin, scale, SpriteEffects.None, 0f);
        }

        public virtual void TakeDamage(int amount)
        {
            if (Destroyed) return;
            Health = Math.Max(0, Health - amount);
            if (Health == 0) Explode();
        }

        public virtual void Explode()
        {
            Destroyed = true;
        }

        protected void Fire()
        {
            LaserFired?.Invoke(position + muzzleOffset);
        }

        protected virtual void UpdateBounds()
        {
            float w = texture.Width * scale * hitboxScale;
            float h = texture.Height * scale * hitboxScale;
            bounds.X = position.X - w / 2f + boundsOffset.X;
            bounds.Y = position.Y - h / 2f + boundsOffset.Y;
            bounds.Width = w;
            bounds.Height = h;
        }
    }
}