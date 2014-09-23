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

namespace Train.Enemies
{
    public class Fly : Enemy
    {
        Triangle body;
        Cube booster1;
        Cube booster2;
        float incRotation;
        List<Cube> lasers = new List<Cube>();
        int fireRate, fireRateMax;
        public Fly(Vector3 pos)
            : base(pos)
        {
            body = new Triangle(pos);
            booster1 = new Cube( new Vector3(-3, 1, 0), new Vector3(0.75f, 0.75f, 2));
            booster2 = new Cube(new Vector3(3, 1, 0), new Vector3(0.75f, 0.75f, 2));
            body.scale *= 3;
            score = 9;
            damage = 20;
            fireRateMax = 100;
        }
        public override void Draw(GraphicsDevice gd)
        {
            
            Game1.myeffect.Parameters["face"].SetValue(Game1.getTexture("hpbar"));
            foreach (Cube c in lasers)
            {
                c.Draw(gd);
            }
            booster1.world = Matrix.CreateFromYawPitchRoll(body.rotation.Y, body.rotation.X, body.rotation.Z) * Matrix.CreateTranslation(body.position);
            booster2.world = booster1.world;
            booster1.Draw(gd);
            booster2.Draw(gd);
            Game1.myeffect.Parameters["face"].SetValue(Game1.getTexture("darkmetal"));
            if (hurtTime > 0)
            {
                hurtTime--;
                Game1.myeffect.Parameters["face"].SetValue(Game1.texlibrary[0]);
            }
            base.Draw(gd);
            body.Draw(gd);
            
        }
        public override void update(MainTrain train)
        {
            base.update(train);
            body.position = position;
            if(incRotation%50 > 25)
                body.rotation.Y += 0.02f;
            else
                body.rotation.Y -= 0.02f;
            incRotation += (float)Game1.rand.NextDouble()*0.20f;
            position.X -= (float)Math.Sin(body.rotation.Y) * 0.25f;
            position.Z -= (float)Math.Cos(body.rotation.Y) * 0.25f;
            if (Vector3.Distance(train.position, position) < 100)
            {
                if (fireRate <= 0)
                {
                    fireRate = fireRateMax;
                    Cube c = new Cube(position);
                    c.scale.Z = 3;
                    c.scale.X = 0.25f;
                    c.lifetime = 100;
                    c.scale.Y = 0.25f;
                    c.rotation.X = 1.7f;
                    float angle = (float)Math.Atan2(train.position.X - position.X,train.position.Z - position.Z)+(float)Math.PI;
                    c.rotation.Y = angle;
                    c.velocity.X = -(float)Math.Sin(angle);
                    c.velocity.Z = -(float)Math.Cos(angle);
                    c.velocity += train.getVelocity();
                    lasers.Add(c);
                }
                fireRate--;
            }
            for (int i = 0; i < lasers.Count; i++)
            {
                Cube c = lasers[i];
                c.position.X += c.velocity.X;
                c.position.Z += c.velocity.Z;
                c.position.Y -= 0.3f;
                c.lifetime--;
                if (train.isInside(c.position, 4)&& c.lifetime >0)
                {
                    Game1.sfx[2].Play(0.25f, 0, 0);
                    train.takeDmg(4);
                    TrackSet.createExplosion(c.position);
                    lasers.RemoveAt(i);
                }
                if (c.lifetime <= 0)
                {
                    lasers.RemoveAt(i);
                }

            }
          
        }
    }
}
