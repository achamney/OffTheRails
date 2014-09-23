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
    public class Enemy
    {
        
        protected Vector3 position;
        protected Vector3 rotation;
        protected int hurtTime;
        int health;
        protected int score;
        protected int damage;

        protected bool volatileExplosion; 
        public Enemy(Vector3 position)
        {
            this.position = position;
            health = 10;
            volatileExplosion = false;
        }
        public virtual void Draw(GraphicsDevice gd)
        {
            
            
        }
        public void setRotation(float rot)
        {
            rotation.Y = rot;
        }
        public Vector3 getPosition()
        { return position; }
        public int decHealth(int dec,Projectile.types type)
        {
            health -= dec;
            hurtTime = 10;
            if (type == Projectile.types.Special)
                volatileExplosion = true;
            return health;
        }
        public void kill(MainTrain train)
        {
            for (int j = 0; j < 10; j++)
            {
                float anglx = (float)(Game1.rand.NextDouble() * Math.PI * 2);
                float angly = (float)(Game1.rand.NextDouble() * Math.PI * 2);
                float magSpeed = (float)(Game1.rand.NextDouble() * 0.5f);
                //Explosion
                Particle.MakeParticle(position,
                    new Vector3((float)Math.Sin(anglx) * (float)Math.Sin(angly) * magSpeed, (float)Math.Cos(angly) * magSpeed, (float)Math.Sin(anglx) * (float)Math.Cos(angly) * magSpeed),
                    new Vector3(),
                    Particle.Texts.SPARK,
                    50, 10, 1.01f, 0);
            }
            if (volatileExplosion)
            {
                int maxBuls = 5;
                for (int j = 0; j < maxBuls; j++)
                {
                    for (int i = 0; i < maxBuls; i++)
                    {
                        new Projectile(train.getCurWeapon(), position,
                            new Vector3((float)i * (float)Math.PI / (float)maxBuls, (float)j * (float)Math.PI / (float)maxBuls, 0), 1,
                            Projectile.types.Special,20);
                    }
                }
            }
        }
        public bool isInside(Vector3 pos)
        {
            BoundingSphere sph = new BoundingSphere(position, 7);
            BoundingSphere point = new BoundingSphere(pos, 1);
            
            if (point.Intersects(sph))
                return true;
            return false;
        }
        public virtual void update(MainTrain train)
        {

        }
        public int getDamage()
        {
            return damage;
        }
        public int getScore()
        {
            return score;
        }
    }
}
