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
    class Suicide : Enemy
    {
        Triangle body;
        public Suicide(Vector3 pos)
            : base(pos)
        {
            body = new Triangle(pos);
            body.scale *= 3;
            score = 7;
            damage = 20;
        }
        public override void Draw(GraphicsDevice gd)
        {
            Game1.myeffect.Parameters["face"].SetValue(Game1.texlibrary[7]);
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
            body.rotation.Y = (float)Math.Atan2(train.position.X - position.X,train.position.Z - position.Z)+(float)Math.PI;
            if (Vector3.Distance(train.position, position) < 30 && train.getActive())
            {
                position.X -= (float)Math.Sin(body.rotation.Y)*0.5f;
                position.Z -= (float)Math.Cos(body.rotation.Y)*0.5f;
                if (position.Y > train.position.Y)
                    position.Y -= 0.25f;
            }
        }
    }
}
