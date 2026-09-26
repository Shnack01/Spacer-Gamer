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

    public interface IEnemy
    {
        Vector2 Position { get; }
        BoundingCircle Bounds { get; }
        bool hit { get; set; }
        Color color { get; set; }
        float spawnTimer { get; set; }
        public void LoadContent(ContentManager content);
        public void Update(GameTime gameTime, int wave);
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}