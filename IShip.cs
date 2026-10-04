using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpacerGamer.Collisions;

namespace SpacerGamer
{
    public interface IShip
    {
        int Health { get; set; }
        Vector2 Position { get; }
        BoundingRectangle Bounds { get; }
        bool Destroyed { get; }
        Color Color { get; set; }

        void LoadContent(ContentManager content);
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void TakeDamage(int amount);
    }
}