using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
namespace Train
{
    class EnemySet
    {
        static List<Enemy> enemyList = new List<Enemy>();
        public static void initEnemies()
        {
            enemyList = new List<Enemy>();
        }
        public static void createStar(Vector3 pos,float rotation)
        {
            Vector3 left = Vector3.Transform(Vector3.Left, Matrix.CreateRotationY(rotation));
            enemyList.Add(new Enemies.Blocker(pos + new Vector3(0,5,0)));
            enemyList.Add(new Enemies.Blocker(new Vector3(0, 2, 0) + pos + left * 5));
            enemyList.Add(new Enemies.Blocker(new Vector3(0, 10, 0) + pos + left * 5));
            enemyList.Add(new Enemies.Blocker(new Vector3(0, 2, 0) + pos - left * 5));
            enemyList.Add(new Enemies.Blocker(new Vector3(0, 10, 0) + pos - left * 5));
            for (int i = enemyList.Count - 5; i < enemyList.Count; i++)
            {
                enemyList[i].setRotation(rotation);
            }
        }
        public static void createSuicide(Vector3 pos)
        {
            enemyList.Add(new Enemies.Suicide(pos));
        }
        public static void createFly(Vector3 pos)
        {
            enemyList.Add(new Enemies.Fly(pos));
        }
        public static void DrawEnemies(GraphicsDevice gd)
        {
            foreach (Enemy en in enemyList)
            {
                en.Draw(gd);
            }
        }
        public static void updateEnemies(MainTrain train)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                Enemy en = enemyList[i];
                en.update(train);
  
                Projectile.types typ =  Projectile.collideProj(en);
                if ( typ != Projectile.types.None)
                {
                    int dmg = 1;
                    if (typ == Projectile.types.Special)
                        dmg = 11;
                    if (en.decHealth(dmg, typ) < 0)
                    {
                        en.kill(train);
                        Game1.sfx[3].Play(0.25f, 0, 0);
                        train.addScore(en.getScore());
                        enemyList.RemoveAt(i);
                    }
                }
                if (train.isInside(en.getPosition()))
                {
                    en.kill(train);
                    Game1.sfx[3].Play(0.5f, 0, 0);
                    train.takeDmg(en.getDamage());
                    enemyList.RemoveAt(i);
                }
            }
        }
    }
}
