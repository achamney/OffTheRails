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
    public class Object3D
    {
        public Matrix world;
        public Vector3 rotation;
        public Vector3 position;
        protected Vector3 velocity;
        protected Vector3 scale;
        public Object3D(Vector3 position)
        {
            this.position = position;
            world = Matrix.Identity;
            scale = new Vector3(1, 1, 1);
        }
        public virtual void Draw(GraphicsDevice gd)
        {
        }
        public Vector3 getScale() { return scale; }
        public Vector3 getRotation() { return rotation; }
    }
}
