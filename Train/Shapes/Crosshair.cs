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
    class Crosshair: Object3D
    {
        VertexPositionNormalTexture[] hLine;
        VertexPositionNormalTexture[] vLine;
        public Crosshair(Vector3 position)
            : base(position)
        {
            hLine = new VertexPositionNormalTexture[2];
            hLine[0] = new VertexPositionNormalTexture(position + new Vector3(-2.5f, 0, 0), new Vector3(0, 0, 1), new Vector2(0, 0));
            hLine[1] = new VertexPositionNormalTexture(position + new Vector3(2.5f, 0, 0), new Vector3(0, 0, 1), new Vector2(1, 0));
            vLine = new VertexPositionNormalTexture[2];
            vLine[0] = new VertexPositionNormalTexture(position + new Vector3(-0f, 2.5f, 0),new Vector3(0,0,1),new Vector2(0,0));
            vLine[1] = new VertexPositionNormalTexture(position + new Vector3(0f, -2.5f, 0), new Vector3(0,0,1),new Vector2(1,0));
        }
        public void Draw(GraphicsDevice gd, Matrix world)
        {
            Game1.lworld.SetValue( Matrix.CreateTranslation(position)*world);
            Game1.myeffect.CommitChanges();
            gd.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.LineList, hLine, 0, 1);
            gd.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.LineList, vLine, 0, 1);
        }
    }
}
