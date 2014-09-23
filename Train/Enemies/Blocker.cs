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
    class Blocker:Enemy
    {
        Cylinder x1;
        Cylinder x2;
        Cube mainBox;
        Cylinder[] guns;
        public Blocker(Vector3 pos)
            : base(pos)
        {
            guns = new Cylinder[4];
            damage = 10;
            score = 5;
            guns[0] = new Cylinder(new Vector3(-2, 3, 0), new Vector3(1, 1, 1) * 0.5f, 5);
            guns[1] = new Cylinder(new Vector3(2, 3, 0), new Vector3(1, 1, 1) * 0.5f, 5);
            guns[2] = new Cylinder(new Vector3(2, -3, 0), new Vector3(1, 1, 1) * 0.5f, 5);
            guns[3] = new Cylinder(new Vector3(-2, -3, 0), new Vector3(1, 1, 1) * 0.5f, 5);
            mainBox = new Cube(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
            x1 = new Cylinder(new Vector3(-2, -3, 0), 7.5f);
            x1.rotation.Y = (float)Math.PI / 2;
            x1.rotation.X = -(float)Math.PI / 3.3f;
            x2 = new Cylinder(new Vector3(-2, 3, 0), 7.5f);
            x2.rotation.Y = (float)Math.PI / 2;
            x2.rotation.X = (float)Math.PI / 3.3f;
        }
        public override void Draw(GraphicsDevice gd)
        {
            base.Draw(gd);
            Game1.myeffect.Parameters["face"].SetValue(Game1.getTexture("darkmetal"));
            if (hurtTime > 0)
            {
                hurtTime--;
                Game1.myeffect.Parameters["face"].SetValue(Game1.texlibrary[3]);
            }
            x1.world = Matrix.CreateRotationY(rotation.Y) * Matrix.CreateTranslation(position);
            x2.world = x1.world;
            x1.Draw(gd);
            x2.Draw(gd);

            mainBox.world = x1.world;
            mainBox.Draw(gd);
            for (int i = 0; i < 4; i++)
            {
                guns[i].world = x1.world;
                guns[i].Draw(gd);
            }
        }
    }
}
