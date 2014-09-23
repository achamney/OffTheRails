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
    public class Cube :Object3D
    {
        protected VertexPositionNormalTexture[] face1;
        protected VertexPositionNormalTexture[] face2;
        protected VertexPositionNormalTexture[] face3;
        protected VertexPositionNormalTexture[] face4;
        protected VertexPositionNormalTexture[] top;
        protected VertexPositionNormalTexture[] bottom;
        public Matrix orbitMatrix;
        public Vector3 position;
        public Vector3 scale;
        public Vector3 velocity;
        public Vector3 rotation;
        public Matrix world;
        public int lifetime;
        public Cube(Vector3 position):base(position)
        {
            this.position = position;
            world = Matrix.Identity;
            orbitMatrix = Matrix.Identity;
            scale = new Vector3(1.5f, 1.5f, 1.5f);
            face1 = new VertexPositionNormalTexture[4];
            face2 = new VertexPositionNormalTexture[4];
            face3 = new VertexPositionNormalTexture[4];
            face4 = new VertexPositionNormalTexture[4];
            top = new VertexPositionNormalTexture[4];
            bottom = new VertexPositionNormalTexture[4];
            Color c = new Color(200, 100, 0);
            Color c2 = new Color(100, 200, 0);
            Color c3 = new Color(0, 200, 100);
            Color c4 = new Color(0, 100, 200);
            face1[0] = new VertexPositionNormalTexture(new Vector3(-1, 1, 1), new Vector3(0, 0, 1), new Vector2(0, 0));
            face1[1] = new VertexPositionNormalTexture(new Vector3(1, 1, 1), new Vector3(0, 0, 1), new Vector2(0, 1));
            face1[2] = new VertexPositionNormalTexture(new Vector3(1, -1, 1), new Vector3(0, 0, 1), new Vector2(1, 1));
            face1[3] = new VertexPositionNormalTexture(new Vector3(-1, -1, 1), new Vector3(0, 0, 1), new Vector2(1, 0));

            face2[0] = new VertexPositionNormalTexture(new Vector3(-1, 1, -1), new Vector3(0, 0, -1), new Vector2(0, 0));
            face2[1] = new VertexPositionNormalTexture(new Vector3(1, 1, -1), new Vector3(0, 0, -1), new Vector2(0, 1));
            face2[2] = new VertexPositionNormalTexture(new Vector3(1, -1, -1), new Vector3(0, 0, -1), new Vector2(1, 1));
            face2[3] = new VertexPositionNormalTexture(new Vector3(-1, -1, -1), new Vector3(0, 0, -1), new Vector2(1, 0));

            face3[0] = new VertexPositionNormalTexture(new Vector3(-1, 1, 1), new Vector3(-1, 0, 0), new Vector2(0, 0));
            face3[1] = new VertexPositionNormalTexture(new Vector3(-1, 1, -1), new Vector3(-1, 0, 0), new Vector2(1, 0));
            face3[2] = new VertexPositionNormalTexture(new Vector3(-1, -1, -1), new Vector3(-1, 0, 0), new Vector2(1, 1));
            face3[3] = new VertexPositionNormalTexture(new Vector3(-1, -1, 1), new Vector3(-1, 0, 0), new Vector2(0, 1));

            face4[0] = new VertexPositionNormalTexture(new Vector3(1, 1, 1), new Vector3(1, 0, 0), new Vector2(0, 0));
            face4[1] = new VertexPositionNormalTexture(new Vector3(1, 1, -1), new Vector3(1, 0, 0), new Vector2(1, 0));
            face4[2] = new VertexPositionNormalTexture(new Vector3(1, -1, -1), new Vector3(1, 0, 0), new Vector2(1, 1));
            face4[3] = new VertexPositionNormalTexture(new Vector3(1, -1, 1), new Vector3(1, 0, 0), new Vector2(0, 1));

            top[0] = new VertexPositionNormalTexture(new Vector3(-1, 1, 1), new Vector3(0, 1, 0), new Vector2(0, 0));
            top[1] = new VertexPositionNormalTexture(new Vector3(-1, 1, -1), new Vector3(0, 1, 0), new Vector2(0, 1));
            top[2] = new VertexPositionNormalTexture(new Vector3(1, 1, -1), new Vector3(0, 1, 0), new Vector2(1, 1));
            top[3] = new VertexPositionNormalTexture(new Vector3(1, 1, 1), new Vector3(0, 1, 0), new Vector2(1, 0));

            bottom[0] = new VertexPositionNormalTexture(new Vector3(-1, -1, 1), new Vector3(0, -1, 0), new Vector2(0, 0));
            bottom[1] = new VertexPositionNormalTexture(new Vector3(-1, -1, -1), new Vector3(0, -1, 0), new Vector2(0, 1));
            bottom[2] = new VertexPositionNormalTexture(new Vector3(1, -1, -1), new Vector3(0, -1, 0), new Vector2(1, 1));
            bottom[3] = new VertexPositionNormalTexture(new Vector3(1, -1, 1), new Vector3(0, -1, 0), new Vector2(1, 0));

        }
        public Cube(Vector3 position, Vector3 scale)
            : this(position)
        {
            this.scale = scale;
        }
        public override void Draw(GraphicsDevice graphics)
        {
            // draw it
            //rotation.Y += 0.01f;
            // Game1.be.Alpha = 0.5f;
            //Game1.be.PreferPerPixelLighting = true;
            //Game1.be.VertexColorEnabled = false;

            Game1.lworld.SetValue(Matrix.CreateScale(scale) *
                Matrix.CreateFromYawPitchRoll(rotation.Y, 0, 0) * orbitMatrix * Matrix.CreateTranslation(position) * world);
            Game1.myeffect.CommitChanges();
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, face1, 0, 2);
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, face2, 0, 2);
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, face3, 0, 2);
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, face4, 0, 2);

            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, top, 0, 2);
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, bottom, 0, 2);//list
            //Game1.be.VertexColorEnabled = true;
        }
        public Vector3 getScale()
        {
            return scale;
        }
    }
}
