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
    public class ScenePiece
    {
        Model model;
        Vector3 position;
        Vector3 scale;
        Vector3 rotation;
        Matrix world;
        public ScenePiece(Model m, Vector3 pos)
        {
            model = m;
            position = pos;
            rotation.X = -(float)Math.PI / 2;
            rotation.Y = (float)Game1.rand.NextDouble()*6;                                                            
            scale = new Vector3(1, 1, 1) * 1.5f;
            world = Matrix.CreateScale(scale) *
                        Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) *
                        Matrix.CreateTranslation(position);
        }
        public void Draw()
        {
            //Game1.lworld.SetValue( Matrix.CreateTranslation(position));
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = Game1.myeffect;
                }
            }
            Game1.myeffect.Techniques[0].Passes[0].End();
            Game1.myeffect.End();
            Game1.myeffect.Parameters["face"].SetValue(Game1.texlibrary[6]);
            foreach (ModelMesh m in model.Meshes)
            {
                foreach (Effect effect in m.Effects)
                {
                    effect.Parameters["World"].SetValue(world);

                }
                m.Draw();
            }

            Game1.myeffect.Begin();
            Game1.myeffect.Techniques[0].Passes[0].Begin();
        }
        public bool isInside(Vector3 pos)
        {
            BoundingSphere sph = model.Meshes[0].BoundingSphere;
            BoundingSphere point = new BoundingSphere(pos, 1);
            sph = sph.Transform(world);
            if (point.Intersects(sph))
                return true;
            return false;
        }
        public Vector3 getPosition()
        {
            return position;
        }
    }
}
