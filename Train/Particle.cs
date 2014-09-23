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
    public class Particle
    {
        public enum Texts { SMOKE, SPARK };
        private static List<Particle> particles = new List<Particle>();
        public static VertexDeclaration vertexDeclaration;
        public static VertexPositionTexture [] vertices;
        public static Effect ef;
        public static Texture2D[] textures=new Texture2D[3];
        public Vector3 position;
        public Matrix World = Matrix.Identity;
        float rotation=0,rotationStep;
        public float lifetime = 0;
        float sizeStep;
        public float maxlife = 0;
        Vector3 acceleration;
        public Vector3 velocity = new Vector3();
        public float scale = 5;
        public Texture2D currentTexture;
        public Particle(Vector3 pos, int life, Texts curTexture, Vector3 acc, Vector3 vel,float size,float sizeStep, float rotStep)
        {
            position = pos;
            scale = size;
            rotationStep = rotStep;
            acceleration = acc;
            maxlife = lifetime = (float)life;
            velocity = vel;
            this.sizeStep = sizeStep;
            this.currentTexture = textures[(int)curTexture];
        }
        public static void setupVerticies(Texture2D smoke,Texture2D spark)
        {
            
            //ef = new BasicEffect(Game.graphics.GraphicsDevice, null);
            //ef.TextureEnabled = true;
            textures[0] = smoke;
            textures[1] = spark;
            vertices=new VertexPositionTexture[4];
            vertices[0].Position = new Vector3(1, 1, 0); // top right
            vertices[1].Position = new Vector3(-1, 1, 0); // top left
            vertices[2].Position = new Vector3(-1, -1, 0); // bottom left
            vertices[3].Position = new Vector3(1, -1, 0); // bottom right
            vertices[0].TextureCoordinate = new Vector2(0, 0); // top right
            vertices[1].TextureCoordinate = new Vector2(1, 0); // top left
            vertices[2].TextureCoordinate = new Vector2(1, 1); // bottom left
            vertices[3].TextureCoordinate = new Vector2(0, 1); // bottom right

        }
        public void Update()
        {
            lifetime--;
            velocity += acceleration;
            scale *= sizeStep;
            position += velocity;
            rotation += rotationStep;
            if (lifetime <= 0)
            {
                particles.Remove(this);
            }
        }
        public static void DrawParticles(Effect ef,Camera cam, GraphicsDevice graphics)
        {
            vertexDeclaration = new VertexDeclaration(graphics,
                VertexPositionTexture.VertexElements);
            foreach (Particle p in particles)
            {
                p.Draw(ef,cam, graphics);
            }
        }
        public static void UpdateParticles()
        {
           for(int i=0;i<particles.Count;i++)
            {
                particles[i].Update();
            }
        }
        public static void MakeParticle(Vector3 pos, Vector3 vel,Vector3 acc,Texts curTexture, int life,float size, float sizeStep, float rotStep)
        {
            particles.Add(new Particle(pos, life, curTexture, acc, vel,size,sizeStep,rotStep));
        }

        public virtual void Draw(Effect ef,Camera cam, GraphicsDevice graphics)
        {
            //setup effect
            //Vector3 campos = cam.getPosition();
            graphics.VertexDeclaration = vertexDeclaration;
            Vector3 camrot = cam.getRotation();
           // float dis = (float)Math.Sqrt(dx * dx + dy * dy);
            //rotation += 0.5f;
            //float angle = (float)Math.Atan2(dy, dx) + camrot.Z;
            //Vector3 newpos = new Vector3(position.X, position.Y, position.Z);
            ef.Parameters["World"].SetValue(Matrix.CreateFromYawPitchRoll(-camrot.Y-(float)Math.PI/2,-camrot.X,rotation) * 
                Matrix.CreateScale(scale) * Matrix.CreateTranslation(position) * World);
            //ef.Texture = textures[currentTexture];
           // ef.Alpha = lifetime / maxlife;//0.5f;
            //render
            SetUpRenderState(graphics);
            Game1.myeffect.Parameters["face"].SetValue(currentTexture);
            ef.Begin();
            ef.CurrentTechnique.Passes[0].Begin();
           // Game.graphics.GraphicsDevice.VertexDeclaration = vertexDeclaration;
            graphics.DrawUserPrimitives(PrimitiveType.TriangleFan, vertices, 0, 2);
            ef.CurrentTechnique.Passes[0].End();
            ef.End();
            RestoreRenderState(graphics);
        }
        public void SetUpRenderState(GraphicsDevice graphics)
        {
            // Ensures we draw the back-side, too!
            graphics.RenderState.CullMode = CullMode.None;
            // Use additive blending.
            graphics.RenderState.SourceBlend = Blend.SourceAlpha;
            graphics.RenderState.DestinationBlend = Blend.One;
            graphics.RenderState.DepthBufferEnable = true;
            // Enable alpha blending.
            graphics.RenderState.AlphaBlendEnable = true;

        }

        public void RestoreRenderState(GraphicsDevice graphics)
        {
            // Put things back.
            graphics.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
            graphics.RenderState.SourceBlend = Blend.One;
            graphics.RenderState.DestinationBlend = Blend.Zero;
            graphics.RenderState.AlphaBlendEnable = false;

        }
        public static Particle getLastParticle()
        {
            return particles[particles.Count - 1];
        }
    }
}
