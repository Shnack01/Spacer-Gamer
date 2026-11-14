using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpacerGamer
{
    public class Asteriod : EnemyBase
    {
        private Cube cube;
        private float angle;
        private const float RADIUS = 24f;  

        public override int Health { get; set; } = 1;
        public override int points { get; set; } = 0;
        private int dropRate = 20;
        public Game game;

        public Asteriod(float spawnTimer, Vector2 position) : base(position, spawnTimer, 24)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            if (cube == null) cube = new Cube(game);
        }

        public override void Update(GameTime gameTime, int wave)
        {
            angle += (float)gameTime.ElapsedGameTime.TotalSeconds * 2f;

            if (spawnTimer > 0)
            {
                spawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }

            int speed = 0;

            position += new Vector2(0, 3 + speed);
            bounds.Center = position;
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
        public void DropHealth()
        {
            int drop = Random.Shared.Next(1, 101);
            if(drop <= dropRate)
            {
                if (game is SpaceGame spaceGame)
                {
                    var heart = new Heart(position);
                    heart.LoadContent(spaceGame.Content); 
                    spaceGame.hearts.Add(heart);
                }
            }
        }
        public void Draw3D()
        {
            if (Destroyed || spawnTimer > 0)
            {
                
                return;
            }
            cube.Draw(position, RADIUS, angle);
        }
    }
}