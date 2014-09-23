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
    class HalfPipe:Object3D
    {
        int polyCount = 100;
        VertexPositionNormalTexture[] roundFace1;
        VertexPositionNormalTexture[] roundFace2;
        VertexPositionNormalTexture[] side1;
        VertexPositionNormalTexture[] side2;
        VertexPositionNormalTexture[][] pipe;
        public HalfPipe(Vector3 position,Vector3 scale)
            : base(position)
        {
            Color c = Color.Wheat;
            this.scale *= scale;
            pipe = new VertexPositionNormalTexture[polyCount + 2][];
            float angle = 0,nextAngle = 0;
            float pipeSize = 1.8f;
            for (int i = 0; i < polyCount; i++)
            {
                pipe[i] = new VertexPositionNormalTexture[4];
                angle = (i) * (float)Math.PI / polyCount;
                nextAngle = (i + 1) * (float)Math.PI / polyCount;
                Color col2 = Color.White;
                col2.G = (byte)(i * 15);
                col2.R = (byte)(i * 15);
                col2.B = (byte)(i * 15);
                pipe[i][0] = new VertexPositionNormalTexture(new Vector3((float)Math.Cos(angle) / pipeSize, 1 - (float)Math.Sin(angle) / pipeSize, 0),
                    new Vector3((float)Math.Sin(angle),(float)Math.Cos(angle),0),new Vector2(0,0));
                pipe[i][1] = new VertexPositionNormalTexture(new Vector3((float)Math.Cos(angle) / pipeSize, 1 - (float)Math.Sin(angle) / pipeSize, -1),
                    new Vector3((float)Math.Sin(angle), (float)Math.Cos(angle), 0), new Vector2(1, 0));
                pipe[i][2] = new VertexPositionNormalTexture(new Vector3((float)Math.Cos(nextAngle) / pipeSize, 1 - (float)Math.Sin(nextAngle) / pipeSize, -1),
                    new Vector3((float)Math.Sin(nextAngle), (float)Math.Cos(nextAngle), 0), new Vector2(1, 1));
                pipe[i][3] = new VertexPositionNormalTexture(new Vector3((float)Math.Cos(nextAngle) / pipeSize, 1 - (float)Math.Sin(nextAngle) / pipeSize, 0),
                    new Vector3((float)Math.Sin(nextAngle), (float)Math.Cos(nextAngle), 0), new Vector2(0, 1));
            }
            pipe[polyCount] = new VertexPositionNormalTexture[4];
            pipe[polyCount][0] = new VertexPositionNormalTexture(new Vector3((float)Math.Cos(0) / pipeSize, 1 - (float)Math.Sin(0) / pipeSize, 0),
                new Vector3((float)Math.Sin(0), (float)Math.Cos(0), 0), new Vector2(0, 0));
            pipe[polyCount][1] = new VertexPositionNormalTexture(new Vector3((float)Math.Cos(0) / pipeSize, 1 - (float)Math.Sin(0) / pipeSize, -1),
                new Vector3((float)Math.Sin(0), (float)Math.Cos(0), 0), new Vector2(0, 1));
            pipe[polyCount][2] = new VertexPositionNormalTexture(new Vector3(1, 1, -1),
                new Vector3((float)Math.Sin(0), (float)Math.Cos(0), 0), new Vector2(1, 1));
            pipe[polyCount][3] = new VertexPositionNormalTexture(new Vector3(1, 1, 0),
                new Vector3((float)Math.Sin(0), (float)Math.Cos(0), 0), new Vector2(0, 1));
            pipe[polyCount + 1] = new VertexPositionNormalTexture[4];
            pipe[polyCount + 1][0] = new VertexPositionNormalTexture(new Vector3((float)Math.Cos(3.14) / pipeSize, 1 - (float)Math.Sin(3.14) / pipeSize, 0),
                new Vector3((float)Math.Sin(0), (float)Math.Cos(0), 0), new Vector2(0, 0));
            pipe[polyCount + 1][1] = new VertexPositionNormalTexture(new Vector3((float)Math.Cos(3.14) / pipeSize, 1 - (float)Math.Sin(3.14) / pipeSize, -1),
                new Vector3((float)Math.Sin(0), (float)Math.Cos(0), 0), new Vector2(0, 1));
            pipe[polyCount + 1][2] = new VertexPositionNormalTexture(new Vector3(-1, 1, -1),
                new Vector3((float)Math.Sin(0), (float)Math.Cos(0), 0), new Vector2(1, 1));
            pipe[polyCount + 1][3] = new VertexPositionNormalTexture(new Vector3(-1, 1, 0),
                new Vector3((float)Math.Sin(0), (float)Math.Cos(0), 0), new Vector2(1, 0));


            roundFace1 = new VertexPositionNormalTexture[6 + polyCount];
            roundFace2 = new VertexPositionNormalTexture[6 + polyCount];
            side1 = new VertexPositionNormalTexture[4];
            side2 = new VertexPositionNormalTexture[4];

            side1[0] = new VertexPositionNormalTexture(new Vector3(-1, 1, 0),
                new Vector3(-1, 0, 0), new Vector2(0, 0));
            side1[1] = new VertexPositionNormalTexture(new Vector3(-1, 1, -1),
                new Vector3(-1, 0, 0), new Vector2(1, 0));
            side1[2] = new VertexPositionNormalTexture(new Vector3(-1, 0, -1),
                new Vector3(-1, 0, 0), new Vector2(1, 1));
            side1[3] = new VertexPositionNormalTexture(new Vector3(-1, 0, 0),
                new Vector3(-1, 0, 0), new Vector2(0, 1));

            side2[0] = new VertexPositionNormalTexture(new Vector3(1, 1, 0),
                new Vector3(1, 0, 0), new Vector2(0, 0));
            side2[1] = new VertexPositionNormalTexture(new Vector3(1, 1, -1),
                new Vector3(1, 0, 0), new Vector2(1, 0));
            side2[2] = new VertexPositionNormalTexture(new Vector3(1, 0, -1),
                new Vector3(1, 0, 0), new Vector2(1, 1));
            side2[3] = new VertexPositionNormalTexture(new Vector3(1, 0, 0),
                new Vector3(1, 0, 0), new Vector2(0, 1));

            roundFace1[0] = new VertexPositionNormalTexture(new Vector3(0, 0, 0),
                new Vector3(0, 0, 1), new Vector2(0, 0));
            roundFace1[1] = new VertexPositionNormalTexture(new Vector3(1, 0, 0),
                new Vector3(0, 0, 1), new Vector2(1, 0));
            roundFace1[2] = new VertexPositionNormalTexture(new Vector3(1, 1, 0),
                new Vector3(0, 0, 1), new Vector2(0, 1));

            roundFace2[0] = new VertexPositionNormalTexture(new Vector3(0, 0, -1),
                new Vector3(0, 0, -1), new Vector2(0, 0));
            roundFace2[1] = new VertexPositionNormalTexture(new Vector3(1, 0, -1),
                new Vector3(0, 0, -1), new Vector2(1, 0));
            roundFace2[2] = new VertexPositionNormalTexture(new Vector3(1, 1, -1),
                new Vector3(0, 0, -1), new Vector2(0, 1));
            angle = 0;
            for (int i = 3; i < polyCount+4; i++)
            {
                angle = (i - 3) * (float)Math.PI / polyCount;
                roundFace1[i] = new VertexPositionNormalTexture(new Vector3((float)Math.Cos(angle) / pipeSize, 1 - (float)Math.Sin(angle) / pipeSize, 0),
                    new Vector3(0, 0, 1), new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle)));
                roundFace2[i] = new VertexPositionNormalTexture(new Vector3((float)Math.Cos(angle) / pipeSize, 1 - (float)Math.Sin(angle) / pipeSize, -1),
                    new Vector3(0, 0, -1), new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle)));
            }
            roundFace1[polyCount + 4] = new VertexPositionNormalTexture(new Vector3(-1, 1, 0),
                new Vector3(0, 0, 1), new Vector2(1, 1));
            roundFace1[polyCount + 5] = new VertexPositionNormalTexture(new Vector3(-1, 0, 0),
                new Vector3(0, 0, 1), new Vector2(1, 0));
            roundFace2[polyCount + 4] = new VertexPositionNormalTexture(new Vector3(-1, 1, -1),
                new Vector3(0, 0, -1), new Vector2(1, 1));
            roundFace2[polyCount + 5] = new VertexPositionNormalTexture(new Vector3(-1, 0, -1),
                 new Vector3(0, 0, -1), new Vector2(0, 1));
        }
        public override void Draw(GraphicsDevice graphics)
        {
            // draw it
            //rotation.Y += 0.1f;
            Game1.lworld.SetValue(  Matrix.CreateScale(scale) *
                Matrix.CreateFromYawPitchRoll(rotation.Y, 0, rotation.Z) * Matrix.CreateTranslation(position) * world);
            Game1.myeffect.CommitChanges();
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, roundFace1, 0, polyCount + 4);
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, roundFace2, 0, polyCount + 4);
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, side1, 0, 2);
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, side2, 0, 2);
            for (int i = 0; i < polyCount+2; i++)
            {
                graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, pipe[i], 0, 2);
            }

        }
    }
}
