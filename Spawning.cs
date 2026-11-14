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
        public Game game;

        private int asteriodChance = 80;
        
        public Spawning(int screenWidth, Game game)
        {
            this.screenWidth = screenWidth;
            this.game = game;
        }

        public void Update()
        {
            
        }
        public List<IEnemy> MakeEnemies(List<IEnemy> enemies)
        {
            
            enemies.Clear();/*
            MegaZornMothership m = new MegaZornMothership(0, new Vector2(screenWidth / 2, -50));
                enemies.Add(m);*/
            
            if(wave < 4 && wave != 5)
            {
                
                
                for(int i = 0;  i < enemyCount; i++)
                {
                    int chance = Random.Shared.Next(1, 101);
                    float spawnDelay = (float)_rad.NextDouble() * wave;
                    float x = _rad.Next(0, screenWidth - 48);
                    int y = -48;
                    if(chance >= asteriodChance)
                    {
                        Asteriod a = new Asteriod(spawnDelay, new Vector2(x, y));
                        a.game = game;
                        enemies.Add(a);
                    }
                    else
                    {
                        EnemySprite e = new EnemySprite(spawnDelay, new Vector2(x, y));
                        enemies.Add(e);
                    }
                    
                }
            }
            else if (wave == 5)
            {
                MegaZornMothership m = new MegaZornMothership(0, new Vector2(screenWidth / 2, -50));
                enemies.Add(m);
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
                    int chance = Random.Shared.Next(1, 101);
                    float spawnDelay = (float)_rad.NextDouble() * wave;
                    float x = _rad.Next(screenWidth - 48);
                    int y = -48;
                    
                     if(chance >= asteriodChance)
                    {
                        Asteriod a = new Asteriod(spawnDelay, new Vector2(x, y));
                        a.game = game;
                        enemies.Add(a);
                    }
                    else
                    {
                        EnemySprite e = new EnemySprite(spawnDelay, new Vector2(x, y));
                        enemies.Add(e);
                    }
                    
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