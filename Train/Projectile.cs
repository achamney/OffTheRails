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
    public class Projectile : Cube
    {
        public static List<Projectile> projList;
        public enum types { Special, Regular,None };
        types type;
        int lifetime;
        public Projectile(Weapon parent, Vector3 position, Vector3 rotation, float magSpeed, types type, int lifetime)
            : base(position)
        {
            this.type = type;
            velocity.X = -(float)Math.Sin(rotation.Y+parent.rotation.Y) * magSpeed * (float)Math.Cos(rotation.X);
            velocity.Z = -(float)Math.Cos(rotation.Y+parent.rotation.Y) * magSpeed * (float)Math.Cos(rotation.X);
            Game1.sfx[0].Play(0.25f, 0, 0);
            velocity.Y = (float)Math.Sin(rotation.X) * magSpeed;
            velocity += new Vector3((float)Game1.rand.NextDouble() * 0.1f, (float)Game1.rand.NextDouble() * 0.1f, 0);
            this.position = position;
            this.position.Y += 1.3f;
            //this.position.X += (float)Math.Cos(rotation.Y) * 2;
            //this.position.Z += (float)Math.Cos(rotation.Y) * 2;
            this.rotation.Y = rotation.Y;
            this.rotation.X = rotation.X;
            scale.X = 0.1f;
            scale.Y = 0.1f;
            scale.Z = 1; 
            //world = parent.world;
            this.lifetime = lifetime;

            //Particle.MakeParticle(position + parent.position, velocity, new Vector3(), Particle.Texts.SPARK, 200, 0.3f);
            projList.Add(this);
        }
        public static void initProj()
        {
            projList = new List<Projectile>();
        }
        public static void updateProj()
        {
            foreach (Projectile p in projList)
            {
                p.lifetime--;
                p.position.X += p.velocity.X;
                p.position.Z += p.velocity.Z;
                p.position.Y += p.velocity.Y;
            }
            for (int i = 0; i < projList.Count; i++)
            {
                if (projList[i].lifetime <= 0)
                {
                    projList.RemoveAt(i);
                }
            }
        }
        public static void drawProj(GraphicsDevice g)
        {
            foreach (Projectile p in projList)
            {
                   p.Draw(g);
            }
        }
        public static types collideProj(ScenePiece sp)
        {
            for (int i = 0; i < projList.Count; i++)
            {
                Projectile p = projList[i];
                if (sp.isInside(p.position))
                {
                    types t = p.type;
                    projList.RemoveAt(i);
                    return t;
                }
            }
            return types.None;
        }
        public static types collideProj(Enemy en)
        {
            for (int i = 0; i < projList.Count; i++)
            {
                Projectile p = projList[i];
                if (en.isInside(p.position))
                {
                    types t = p.type;
                    projList.RemoveAt(i);
                    return t;
                }
            }
            return Projectile.types.None;
        }
        public override void Draw(GraphicsDevice graphics)
        {
            // draw it
            //rotation.Y += 0.1f;
            if(type == types.Special)
            Game1.myeffect.Parameters["face"].SetValue(Game1.getTexture("energybar"));
            if (type == types.Regular)
                Game1.myeffect.Parameters["face"].SetValue(Game1.getTexture("darkmetal"));
            Game1.lworld.SetValue(  Matrix.CreateScale(scale) *
                Matrix.CreateTranslation(new Vector3(-0,0, -2 * (float)Math.Cos(rotation.X)-2)) * Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, 0) *
                Matrix.CreateTranslation(position) * world);
            Game1.myeffect.CommitChanges();
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, face1, 0, 2);
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, face2, 0, 2);
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, face3, 0, 2);
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, face4, 0, 2);

            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, top, 0, 2);
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, bottom, 0, 2);//list

        }
        public types getType()
        { return type; }
    }
}
