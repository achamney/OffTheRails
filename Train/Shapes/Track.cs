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
    class Track
    {
        Vector3 endPosition;
        Vector3 startPosition;
        TrackRungs leftSide;
        TrackRungs rightSide;
        
        List<TrackTies> rungs;
        Track nextTrack;
        public float specVal;
        public bool onEnd;
        float slope;
        float size;
        float rotation;
        public Track(Vector3 startPosition, Vector3 endPosition)
        {
            rungs = new List<TrackTies>();
            this.specVal = 1.8f;
            this.startPosition = new Vector3(startPosition.X,startPosition.Y,endPosition.Z);
            this.endPosition = new Vector3(endPosition.X, endPosition.Y, startPosition.Z);
            float changeInX = endPosition.X - startPosition.X;
            float changeInZ = endPosition.Z - startPosition.Z;
            float changeInY = endPosition.Y - startPosition.Y;
            size = Vector3.Distance(startPosition, endPosition);
            slope = (float)Math.Atan2(endPosition.Y -startPosition.Y, endPosition.Z - startPosition.Z);
            rotation = -(float)Math.Atan2(endPosition.Z-startPosition.Z,endPosition.X - startPosition.X);
            leftSide = new TrackRungs(startPosition + new Vector3(changeInX / 2, changeInY / 2, changeInZ/2 ),
                new Vector3(0.2f,0.1f,size/2));
            rightSide = new TrackRungs(startPosition + new Vector3(changeInX / 2, changeInY / 2, changeInZ /2),
                new Vector3(0.2f, 0.1f, size/2));

            rightSide.orbitMatrix = Matrix.CreateTranslation(new Vector3(1.1f * (float)Math.Cos(rotation + Math.PI / 2),
                0, -1.1f * (float)Math.Sin(rotation + Math.PI / 2)));
            leftSide.orbitMatrix = Matrix.CreateTranslation(new Vector3(-1.1f * (float)Math.Cos(rotation + Math.PI / 2),
                0, 1.1f * (float)Math.Sin(rotation + Math.PI / 2)));
            float rungsPerUnit = 8;
            rightSide.rotation.Y = rotation + (float)Math.PI / 2;
            leftSide.rotation.Y = rotation + (float)Math.PI / 2;
            for (int i = 0; i < size/rungsPerUnit; i++)
            {
                TrackTies rung = new TrackTies(leftSide.position + new Vector3(0,-0.1f,0),
                    new Vector3(0.2f,0.1f,2.3f));
                rung.rotation.Y = rotation + (float)Math.PI;
                float orbitSize = (i - size / (rungsPerUnit * 2)) * rungsPerUnit;
                rung.orbitMatrix = Matrix.CreateTranslation(orbitSize * (float)Math.Sin(rotation + (float)Math.PI / 2),
                    0, orbitSize * (float)Math.Cos(rotation + (float)Math.PI / 2));
                rungs.Add(rung);
            }
            

        }
        public void Draw(GraphicsDevice gd,Vector3 campos)
        {
            
            if (Vector3.DistanceSquared(leftSide.position, campos) < 40000)
            {
                Game1.myeffect.Parameters["face"].SetValue(Game1.texlibrary[0]);
                rightSide.Draw(gd);
                leftSide.Draw(gd);
                Game1.myeffect.Parameters["face"].SetValue(Game1.texlibrary[4]);
                foreach (TrackTies r in rungs)
                {
                    r.rotation.Y = rightSide.rotation.Y + (float)Math.PI / (2);
                    r.Draw(gd);
                }
            }
            else
            {
                
            }
        }
        public Vector3 getStart() { return new Vector3(startPosition.X, startPosition.Y, endPosition.Z); }
        public Vector3 getEnd() { return new Vector3(endPosition.X,endPosition.Y,startPosition.Z); }
        public void setStart(Vector3 newpos) { startPosition = newpos; }
        public void setEnd(Vector3 newpos) { endPosition = newpos; }
        public float getSize() { return size; }
        public float getRotation() {
            if (rotation < 0)
                return rotation + (float)Math.PI * 2;
            else
                return rotation; 
        }
        public float getSlope() { return slope; }
        public void setNextTrack(Track t) { nextTrack = t; }
        public Track getNextTrack() { return nextTrack; }
        public void changeTrack(Vector3 startPosition, Vector3 endPosition)
        {
            rungs = new List<TrackTies>();
            this.specVal = 1.8f;
            this.startPosition = new Vector3(startPosition.X, startPosition.Y, endPosition.Z);
            this.endPosition = new Vector3(endPosition.X, endPosition.Y, startPosition.Z);
            float changeInX = endPosition.X - startPosition.X;
            float changeInZ = endPosition.Z - startPosition.Z;
            float changeInY = endPosition.Y - startPosition.Y;
            size = Vector3.Distance(startPosition, endPosition);
            slope = (float)Math.Atan2(endPosition.Y - startPosition.Y, endPosition.Z - startPosition.Z);
            rotation = -(float)Math.Atan2(endPosition.Z - startPosition.Z, endPosition.X - startPosition.X);
            leftSide = new TrackRungs(startPosition + new Vector3(changeInX / 2, changeInY / 2, changeInZ / 2),
                new Vector3(0.2f, 0.1f, size / 2));
            rightSide = new TrackRungs(startPosition + new Vector3(changeInX / 2, changeInY / 2, changeInZ / 2),
                new Vector3(0.2f, 0.1f, size / 2));

            rightSide.orbitMatrix = Matrix.CreateTranslation(new Vector3(1.5f * (float)Math.Cos(rotation + Math.PI / 2),
                0, -1.5f * (float)Math.Sin(rotation + Math.PI / 2)));
            leftSide.orbitMatrix = Matrix.CreateTranslation(new Vector3(-1.5f * (float)Math.Cos(rotation + Math.PI / 2),
                0, 1.5f * (float)Math.Sin(rotation + Math.PI / 2)));
            float rungsPerUnit = 4;
            rightSide.rotation.Y = rotation + (float)Math.PI / 2;
            leftSide.rotation.Y = rotation + (float)Math.PI / 2;
            for (int i = 0; i < size / rungsPerUnit; i++)
            {
                TrackTies rung = new TrackTies(leftSide.position,
                    new Vector3(0.2f, 0.1f, 2.3f));
                rung.rotation.Y = rotation + (float)Math.PI;
                float orbitSize = (i - size / (rungsPerUnit * 2)) * rungsPerUnit;
                rung.orbitMatrix = Matrix.CreateTranslation(orbitSize * (float)Math.Sin(rotation + (float)Math.PI / 2),
                    0, orbitSize * (float)Math.Cos(rotation + (float)Math.PI / 2));
                rungs.Add(rung);
            }
        }
    }
}
