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
    class Textures
    {
        private static List<Textures> texts = new List<Textures>();
        String name;
        Texture2D text;
        public Textures()
        {
        }
        public static int addTexture(Texture2D tex)
        {
            foreach (Textures t in texts)
            {
                if (t.name.Equals(tex.Name))
                { return 0; }
            }
            Textures te = new Textures();
            te.text = tex;
            te.name = tex.Name;
            texts.Add(te);
            return 1;
        }
        public static Texture2D getTexture(String name)
        {
            foreach (Textures t in texts)
            {
                if (t.name.Equals(name))
                    return t.text;
            }
            return null;
        }
    }
}
