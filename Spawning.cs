using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SpacerGamer.Collisions;
using Microsoft.Xna.Framework.Audio;
using System.Threading;
namespace SpacerGamer
{
    public class Spawning
    {
        public int screenWidth;
        public int wave = 1;
        private Random _rad = new Random();
        public int enemyCount => 3 + wave * 2;
        public Spawning(){
            
        }
        public Spawning(int screenWidth)
        {
            this.screenWidth = screenWidth;

        }

        public void Update()
        {
            
        }
        public List<IEnemy> MakeEnemies(List<IEnemy> enemies)
        {
            
            enemies.Clear();
            if(wave < 4)
            {
                for(int i = 0;  i < enemyCount; i++)
                {
                    float spawnDelay = (float)_rad.NextDouble() * wave;
                    float x = _rad.Next(0, screenWidth - 48);
                    int y = -48;
                    EnemySprite e = new EnemySprite(spawnDelay, new Vector2(x, y));
                    enemies.Add(e);
                }
            }
            else
            {
                
                for(int j = 0; j < 2; j++)
                {
                    float spawnDelay = (float)_rad.NextDouble() * wave;
                    float x = _rad.Next(0, screenWidth - 48);
                    int y = -48;
                    Sentry sE = new Sentry(spawnDelay, new Vector2(x,y));
                    enemies.Add(sE);
                }
                for(int i = 0;  i < enemyCount - 2; i++)
                {
                    float spawnDelay = (float)_rad.NextDouble() * wave;
                    float x = _rad.Next(0, screenWidth - 48);
                    int y = -48;
                    
                    EnemySprite e = new EnemySprite(spawnDelay, new Vector2(x, y));
                    enemies.Add(e);
                }
            }
            /*
            EnemySprite e = new EnemySprite(0, new Vector2(200, 200));
            enemies.Add(e);
            */
            return enemies;
        }
    }
}