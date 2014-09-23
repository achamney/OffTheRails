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
    class Cylinder : Object3D
    {
        VertexPositionNormalTexture[] top;
        VertexPositionNormalTexture[] bottom;
        VertexPositionNormalTexture[][] roundPart;
        int polyCount = 10;
        public Cylinder(Vector3 position, float length)
            : base(position)
        {
            this.position = position;
            Color col = new Color(200, 200, 200);
            top = new VertexPositionNormalTexture[polyCount];
            bottom = new VertexPositionNormalTexture[polyCount];
            roundPart = new VertexPositionNormalTexture[polyCount][];
            for (int i = 0; i < polyCount; i++)
            {
                roundPart[i] = new VertexPositionNormalTexture[4];
            }
            float angle;
            for (int i = 0; i < polyCount; i++)
            {
                angle = (float)(i * Math.PI * 2) / polyCount;
                top[i] = new VertexPositionNormalTexture(new Vector3((float)Math.Sin(angle), (float)Math.Cos(angle), length),
                    new Vector3(0, 0, 1), new Vector2((float)Math.Sin(angle) / 2 + 0.5f, (float)Math.Cos(angle) / 2 + 0.5f));

                bottom[i] = new VertexPositionNormalTexture(new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), -0),
                    new Vector3(0, 0, -1), new Vector2((float)Math.Sin(angle)/2 +0.5f, (float)Math.Cos(angle)/2+0.5f));
            }
            float angle2;
            for (float i = 0; i < polyCount; i++)
            {
                Color col2 = Color.White;
                col2.G = (byte)(i * 15);
                col2.R = (byte)(i * 15);
                col2.B = (byte)(i * 15);
                angle = -(float)(i * Math.PI * 2) / polyCount;
                angle2 = -(float)((i + 1) * Math.PI * 2) / polyCount;
                roundPart[(int)i][0] = new VertexPositionNormalTexture(new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), length),
                    new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0), new Vector2(0, 0));
                roundPart[(int)i][1] = new VertexPositionNormalTexture(new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), -0),
                    new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0), new Vector2(1, 0));
                roundPart[(int)i][2] = new VertexPositionNormalTexture(new Vector3((float)Math.Cos(angle2), (float)Math.Sin(angle2), -0),
                    new Vector3((float)Math.Cos(angle2), (float)Math.Sin(angle2), 0), new Vector2(1, 1));
                roundPart[(int)i][3] = new VertexPositionNormalTexture(new Vector3((float)Math.Cos(angle2), (float)Math.Sin(angle2), length),
                    new Vector3((float)Math.Cos(angle2), (float)Math.Sin(angle2), 0), new Vector2(0, 1));
            }
        }
        public Cylinder(Vector3 position, Vector3 scale, float length)
            : this(position, length)
        {
            this.scale = scale;
        }
        public override void Draw(GraphicsDevice graphics)
        {
            // draw it
            Game1.lworld.SetValue(Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) *
                Matrix.CreateTranslation(position) * world);
            Game1.myeffect.CommitChanges();
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, top, 0, polyCount - 2);
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, bottom, 0, polyCount - 2);
            for (int i = 0; i < polyCount; i++)
            {
                graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, roundPart[i], 0, 2);
            }


        }
    }
}
