using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpacerGamer.Collisions;

namespace SpacerGamer
{
    public abstract class ShipBase : IShip
    {
        public float heatLevel {get; set;} = 0;
        public WeaponBase weapon{get; set;}
        protected Vector2 position;
        protected Texture2D texture;
        protected BoundingRectangle bounds = new BoundingRectangle(Vector2.Zero, 1, 1);
        protected readonly string textureName;
        protected readonly float scale;
        protected readonly float hitboxScale;
        protected Vector2 boundsOffset = Vector2.Zero;
        protected Vector2 muzzleOffset = new Vector2(3, -30f);
        public Vector2 TailPosition => new Vector2(
            position.X + (opaqueRect.X + opaqueRect.Width / 2f - texture.Width / 2f) * scale,
            position.Y + (opaqueRect.Bottom - texture.Height / 2f) * scale);

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

        protected Rectangle opaqueRect; 

        public virtual void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(textureName);
            opaqueRect = GetOpaqueBounds(texture);
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
        private static Rectangle GetOpaqueBounds(Texture2D tex)
        {
            var data = new Color[tex.Width * tex.Height];
            tex.GetData(data);

            int minX = tex.Width, minY = tex.Height, maxX = -1, maxY = -1;
            for (int y = 0; y < tex.Height; y++)
            {
                for (int x = 0; x < tex.Width; x++)
                {
                    if (data[y * tex.Width + x].A > 16)
                    {
                        if (x < minX) minX = x;
                        if (x > maxX) maxX = x;
                        if (y < minY) minY = y;
                        if (y > maxY) maxY = y;
                    }
                }
            }
            if (maxX < 0) return tex.Bounds; // fully transparent, fall back
            return new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1);
        }
        public virtual void Explode()
        {
            Destroyed = true;
        }

        protected void Fire()
        {
            LaserFired?.Invoke(position + muzzleOffset);
        }
        public bool TryFireWeapon(Vector2 location, List<Laser> list)
        {
            return weapon != null && weapon.TryFire(position + muzzleOffset, list);
        }

        protected virtual void UpdateBounds()
        {
            float left = position.X + (opaqueRect.X - texture.Width / 2f) * scale;
            float top  = position.Y + (opaqueRect.Y - texture.Height / 2f) * scale;
            float w = opaqueRect.Width * scale;
            float h = opaqueRect.Height * scale;

            float sw = w * hitboxScale;            
            bounds.X = left + (w - sw) / 2f + boundsOffset.X;
            bounds.Y = top + boundsOffset.Y; 
            bounds.Width = sw;
            bounds.Height = h;
        }
    }
}