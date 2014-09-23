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
    class ParticleSystem
    {
        ParticleSystem ps;
        int numPS = 0;
        Effect pe;
        EffectParameter pe_pic;
        EffectParameter pe_world;
        EffectParameter pe_view;
        EffectParameter pe_project;

        Texture fire;
        Texture smoke;
        Texture bird;

        Vector3 emitterpos;
        Vector3 emittervel;
        int numparticles;
        Vector3 gravity;
        Vector3 egravity;
        Random rand;
        Effect particleEffect;
        VertexDeclaration myVertexDeclaration;
        prVertex[] myPtSprite;
        struct oneparticle
        {
            public Vector3 pos;
            public Vector3 vel;

            public float lifeRemaining;
        }
        private struct prVertex
        {
            public Vector3 pos;
            public Vector4 color;
            public float size;

            public static readonly VertexElement[] vertexElements = new VertexElement[] {
                new VertexElement(0,0,VertexElementFormat.Vector3,
                    VertexElementMethod.Default, VertexElementUsage.Position,0),
                new VertexElement(0,sizeof(float)*3,VertexElementFormat.Vector4,
                    VertexElementMethod.Default, VertexElementUsage.Color,0),
                new VertexElement(0,sizeof(float)*7,VertexElementFormat.Single,
                    VertexElementMethod.Default, VertexElementUsage.PointSize,0),
            };

            public prVertex(Vector3 ppos, Vector4 pcol, float psize)
            {
                this.pos = ppos;
                this.color = pcol;
                this.size = psize;
            }

        }
        oneparticle[] p;
        public ParticleSystem()
        {
            
        }
        private oneparticle newparticle()
        {
            float theta;

            oneparticle retval;
            retval = new oneparticle();
            float magSpeed = 0.8f;
            // explosion:
            retval.pos = emitterpos + 5 * Vector3.Forward;
            retval.vel = magSpeed * Vector3.Up * (float)rand.NextDouble();
            theta = (float)(rand.NextDouble() * MathHelper.TwoPi);
            retval.vel = Vector3.Transform(retval.vel, Matrix.CreateRotationX(theta));
            theta = (float)(rand.NextDouble() * MathHelper.TwoPi);
            retval.vel = Vector3.Transform(retval.vel, Matrix.CreateRotationY(theta));
            theta = (float)(rand.NextDouble() * MathHelper.TwoPi);
            retval.vel = Vector3.Transform(retval.vel, Matrix.CreateRotationZ(theta));
            retval.lifeRemaining = (float)(rand.NextDouble() * 2 + rand.NextDouble() * 3);
            //fire:
            /*
            retval.pos = emitterpos + 5 * Vector3.Forward;
            retval.vel = 0.3f*(float)(rand.NextDouble()) * Vector3.Forward;
            retval.vel = Vector3.Transform(retval.vel,
                Matrix.CreateRotationY(theta));
            retval.lifeRemaining = (float)(rand.NextDouble()*3 + rand.NextDouble()*3);
             */


            return retval;
        }

        public ParticleSystem(Vector3 begin, int size, int budget,
            Effect e, Texture fir, Texture smok, Texture bird)
        {
            pe = e;
            fire = fir;
            smoke = smok;
            this.bird = bird;
            pe_pic = pe.Parameters["pic"];
            pe_pic.SetValue(this.fire);
            //            pe_pic.SetValue(smoke);
            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.Up);
            Matrix project = Matrix.CreatePerspectiveFieldOfView(0.5f, 4.0f / 3.0f, 0.01f, 1000.0f);
            Matrix world = Matrix.Identity;
            pe_world = pe.Parameters["World"];
            pe_view = pe.Parameters["View"];
            pe_project = pe.Parameters["Projection"];
            pe_world.SetValue(world);
            pe_view.SetValue(view);
            //pe_project.SetValue(project);
            pe.CommitChanges();

            particleEffect = pe;
            rand = new Random();
            //explosion
            gravity = -0.98f * Vector3.Up;
            //fire
            //            gravity = -5.98f * Vector3.Up;
            numparticles = budget;
            emitterpos = begin;
            p = new oneparticle[budget];
            float maxlife = 5;
            for (int i = 0; i < numparticles; i++)
            {
                p[i] = newparticle();
                p[i].lifeRemaining = maxlife * (i / (float)numparticles);
            }
            emittervel = p[0].vel;
            emittervel.X = 1.2f;
            egravity = -10.2f * Vector3.Up;
            myPtSprite = new prVertex[numparticles];

            for (int i = 0; i < numparticles; i++)
            {
                Vector3 pos = p[i].pos;
                Vector4 col = new Vector4(0.95f, 0.7f, 0.4f, 1.0f);
                float psize = 50; // p[i].lifeRemaining;
                myPtSprite[i] = new prVertex(pos, col, psize);
            }
        }

        public void Draw(GraphicsDevice g)
        {
            // set vertex declaration to custom vertex format
            myVertexDeclaration = new VertexDeclaration(g, prVertex.vertexElements);

            g.VertexDeclaration = myVertexDeclaration;
            g.RenderState.PointSpriteEnable = true;

            g.RenderState.DepthBufferWriteEnable = false;

            g.RenderState.AlphaBlendEnable = true;
            g.RenderState.SourceBlend = Blend.SourceAlpha;
           g.RenderState.DestinationBlend = Blend.One;
            g.RenderState.AlphaBlendOperation = BlendFunction.Max;

            // update point sprite data
            for (int i = 0; i < numparticles; i++)
            {

                myPtSprite[i].pos = p[i].pos;
                myPtSprite[i].size = 10 * p[i].lifeRemaining;

            }

            // now the actual draw

            particleEffect.Begin();
            particleEffect.Techniques[0].Passes[0].Begin();

            g.DrawUserPrimitives<prVertex>(PrimitiveType.PointList,
                myPtSprite, 0, numparticles);

            particleEffect.Techniques[0].Passes[0].End();
            particleEffect.End();
            g.RenderState.DepthBufferWriteEnable = true;
            g.RenderState.PointSpriteEnable = false;
            g.RenderState.AlphaBlendEnable = false;
            g.RenderState.SourceBlend = Blend.DestinationColor;
            g.RenderState.DestinationBlend = Blend.InverseSourceColor;
            g.RenderState.AlphaBlendOperation = BlendFunction.Add;
        }

        public void Update(GameTime gt)
        {

            for (int i = 0; i < numparticles; i++)
            {
                p[i].vel += 0.1f * gravity * (float)gt.ElapsedGameTime.TotalSeconds;
                p[i].pos += 0.1f * p[i].vel * (float)gt.ElapsedGameTime.TotalSeconds;
                p[i].lifeRemaining -= (float)gt.ElapsedGameTime.TotalSeconds;
                if (p[i].lifeRemaining < 0)
                    p[i] = newparticle();
            }
            //emitterpos = Game1.helper.position;
            
                        //emitterpos = new Vector3( 
                         //       (float)Math.Cos(gt.TotalGameTime.TotalSeconds),
                          //      (float)Math.Sin(gt.TotalGameTime.TotalSeconds), 0);
             


            emittervel += 0.1f * egravity * (float)gt.ElapsedGameTime.TotalSeconds;
            emitterpos += 0.1f * emittervel * (float)gt.ElapsedGameTime.TotalSeconds;
            // hack for emitter bounce
            if (emitterpos.Y < -1)
            {
                emitterpos.Y = -0.99f;
                emittervel.Y = -emittervel.Y;
            }
            if (emitterpos.X > 1)
            {
                emitterpos.X = 0.99f;
                emittervel.X = -emittervel.X;
            }
            if (emitterpos.X < -1)
            {
                emitterpos.X = -0.99f;
                emittervel.X = -emittervel.X;
            }

        }
        public void setWorld(Matrix m)
        {
            pe.Parameters["World"].SetValue(m);
           // pe.Parameters["World"].SetValue(Matrix.CreateTranslation());
        }
        public void setView(Matrix v)
        {
            pe.Parameters["View"].SetValue(v);
        }
    }
}
