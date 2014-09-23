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
    class FlatPlane : Object3D
    {
        VertexPositionNormalTexture[] plane;
        public FlatPlane(Vector3 position, Vector3 scale)
            : base(position)
        {
            plane = new VertexPositionNormalTexture[5];
            plane[0] = new VertexPositionNormalTexture(new Vector3(0.5f, 0, 0.5f), new Vector3(0, 1, 0), new Vector2(0.5f, 0.5f));
            plane[1] = new VertexPositionNormalTexture(new Vector3(1, 0, 1), new Vector3(0,1,0),new Vector2(0,0));
            plane[2] = new VertexPositionNormalTexture(new Vector3(1, 0, -1), new Vector3(0, 1, 0), new Vector2(1, 0));
            plane[3] = new VertexPositionNormalTexture(new Vector3(-1, 0, -1), new Vector3(0, 1, 0), new Vector2(1, 1));
            plane[4] = new VertexPositionNormalTexture(new Vector3(-1, 0, 1), new Vector3(0, 1, 0), new Vector2(0, 1));
            this.scale = scale;
        }
        public void Draw(GraphicsDevice graphics)
        {
            // draw it
            //rotation.Y += 0.1f;
            //Game1.be.VertexColorEnabled = true;
            Game1.lworld.SetValue(  Matrix.CreateScale(scale)*
                Matrix.CreateTranslation(position)) ;
            Game1.myeffect.CommitChanges();
            graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleFan, plane, 0, 3);
        }
    }
}
