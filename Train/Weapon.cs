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
    public class Weapon : Object3D
    {
        protected static Model gatGun;
        protected Model model;
        protected MainTrain parent;
        protected Vector3 rotation;
        protected float scale;
        protected int fireRate;
        protected int fireRateMax;
        public Weapon(MainTrain parent, Vector3 position):base (position)
        {
            this.parent = parent;
            
        }
        public static void loadModels(Model gat)
        {
            gatGun = gat;
           
        }
        public virtual void Draw(GraphicsDevice gdg)
        {

            foreach (ModelMesh mesh in gatGun.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = Game1.myeffect;
                }
            }
          
            Game1.myeffect.Techniques[0].Passes[0].End();
            Game1.myeffect.End();
            foreach (ModelMesh m in model.Meshes)
            {
                foreach (Effect effect in m.Effects)
                {
                    //effect.Parameters["View"].SetValue(Game1.myeffect.Parameters["View"].GetValueMatrix());
                    //effect.Parameters["Projection"].SetValue(Game1.myeffect.Parameters["Projection"].GetValueMatrix());
                    //effect.Parameters["World"].SetValue(Matrix.CreateScale(0.05f) * Matrix.CreateTranslation(0, 0, 0));
               
                    effect.Parameters["World"].SetValue( Matrix.CreateScale(new Vector3(0.01f,0.01f,0.01f))*Matrix.CreateTranslation(0.113f,0.341f,1)*
                        Matrix.CreateFromYawPitchRoll(rotation.Y,rotation.X,rotation.Z)*Matrix.CreateTranslation(position)
                        * parent.world );
                    world = Matrix.CreateTranslation(position)*parent.world ;
                    //basic.EnableDefaultLighting();
               
                }
                m.Draw();
            }
           
            Game1.myeffect.Begin();
            Game1.myeffect.Techniques[0].Passes[0].Begin();
        }
        public virtual void fire(bool mp)
        {

        }
        public virtual void secondFire(bool mp)
        {

        }
        public virtual void update()
        {
            rotation.X = MathHelper.Clamp(rotation.X, 0f / 4.5f, 3.141f / 4.5f);
            rotation.Y = MathHelper.Clamp(rotation.Y, -0.5f, 0.5f);
        }
        public void setRotationX(float X) { rotation.X = X; }
        public void setRotationY(float Y) { rotation.Y = Y; }
    }
}
