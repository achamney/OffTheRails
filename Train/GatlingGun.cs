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
    class GatlingGun : Weapon
    {
        public float rotSpeed;
        HalfPipe bottom;
        public GatlingGun(MainTrain parent,Vector3 position)
            : base(parent,position)
        {
            model = Weapon.gatGun;
            scale = 0.01f;
            bottom = new HalfPipe(new Vector3(0.0f,-0.3f,-0.5f), new Vector3(0.5f,0.5f,0.5f));
  
            fireRateMax = 3;
        }
        public override void Draw(GraphicsDevice gd)
        {
            
            base.Draw(gd);
            bottom.world = world;
            bottom.Draw(gd);
        }
        public override void update()
        {
            base.update();
            rotSpeed *= 0.97f;
            rotation.Z += rotSpeed;
        }
        public override void fire(bool mp)
        {
            if (parent.getHealth() > 0)
            {
                rotSpeed += 0.015f;
                //Game1.sfx[1].Play(0.25f, 0, 0);

                if (!mp)
                    Game1.sfxLoop[1].Play();

                if (rotSpeed > 0.4f)
                {
                    if (fireRate <= 0)
                    {
                        fireRate = fireRateMax;

                        new Projectile(this, parent.position, new Vector3(rotation.X, rotation.Y + parent.getRotation().Y, 0), 2f, Projectile.types.Regular,200);


                        base.fire(mp);
                    }
                    else
                    {
                        fireRate--;
                    }
                }
            }
        }
        public override void secondFire(bool mp)
        {
            if (parent.getHealth() > 0)
            {
                rotSpeed += 0.015f;

                if (!mp)
                    Game1.sfxLoop[1].Play();

                if (rotSpeed > 0.4f)
                {
                    if (fireRate <= 0)
                    {
                        fireRate = fireRateMax*10;

                        new Projectile(this, parent.position, new Vector3(rotation.X, rotation.Y + parent.getRotation().Y, 0), 2f, Projectile.types.Special,200);

                        parent.decEnergy(parent.getMaxPower()/2);
                        base.fire(mp);
                    }
                    else
                    {
                        fireRate--;
                    }
                }
            }
        }
    }
}
