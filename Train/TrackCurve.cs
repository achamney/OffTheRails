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
    public class TrackCurve
    {
        List<Track> trackList;
        trackTypes typ;
        TrackCurve nextTrack;
        Vector3 position;
        float rotProgress;
        public bool onEnd;
        float radius, curve;
        float lastCurveRotation;
        bool endTrack;
        public enum trackTypes
        {
            CURVE = 0,
            STRAIGHT = 1
        }
        public TrackCurve(Vector3 position,float radius,float curve,float lastRotation, trackTypes t)
        {
            trackList = new List<Track>();
            typ = t;
            this.radius = radius;
            this.curve = curve;
            this.onEnd = false;
            endTrack = false;
            if (t == trackTypes.CURVE)
            {
                createCurve(position, curve, lastRotation, radius,60);
            }
            if (t == trackTypes.STRAIGHT)
            {
                createStraight(position, radius, lastRotation);
            }
            
        }
        public void createCircle(Vector3 lastPosition, float radius, int tracknum)
        {
            float angle = 0;
            float lastAngle = 0;
            
            for (int i = 1; i < tracknum + 1; i++)
            {
                angle = i * (float)Math.PI * 2 / tracknum;
                lastAngle = (i - 1) * (float)Math.PI * 2 / tracknum;

                Vector3 start = new Vector3((float)Math.Cos(lastAngle) * radius - radius-2, 0, -(float)Math.Sin(lastAngle) * radius);
                Vector3 end = new Vector3((float)Math.Cos(angle) * radius - radius-2, 0, -(float)Math.Sin(angle) * radius);
                trackList.Add(new Track(start +lastPosition,
                    end +lastPosition));
                trackList[trackList.Count - 2].setNextTrack(trackList[trackList.Count - 1]);
            }
            trackList[trackList.Count - 1].setNextTrack(trackList[trackList.Count - tracknum]);
        }
        private int createCurve(Vector3 lastPosition, float maxAngle,float lastRotation, float radius, int tracknum)
        {
            float angle = 0;
            float lastAngle = 0;
            int direction = (int)( Math.Abs(maxAngle) / maxAngle);
            for (int i = 1; i < (tracknum + 1) * Math.Abs(maxAngle) / 3.141f; i++)
            {
                angle = (i) * (float)maxAngle / (tracknum* Math.Abs(maxAngle) / 3.141f);
                lastAngle = (i-1 ) * (float)maxAngle / (tracknum* Math.Abs(maxAngle) / 3.141f);


                Vector3 start = MainTrain.findPointAlongCurve(radius, lastAngle, lastRotation, direction);
                Vector3 end = MainTrain.findPointAlongCurve(radius, angle, lastRotation, direction);



                trackList.Add(new Track(start +lastPosition,
                    end +lastPosition));

            }
            

            Vector3 newForward = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(lastRotation));
    
            position = getStart();
            return trackList.Count - 1;

        }
        public int createStraight(Vector3 lastPosition, float radius, float lastRotation)
        {
            Vector3 end = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(lastRotation)) * radius;
            trackList.Add(new Track(lastPosition, end + lastPosition));
            return trackList.Count - 1;
        }
        public void Draw(GraphicsDevice gd, Vector3 camPos)
        {
            foreach (Track t in trackList)
            {
                t.Draw(gd, camPos);
            }
        }
        private void addTrack(Vector3 start, Vector3 end)
        {
            trackList.Add(new Track(start, end));
            if (trackList.Count > 1)
            {
                trackList[trackList.Count - 2].setNextTrack(trackList[trackList.Count - 1]);
            }
        }
        public Vector3 getLastTrack()
        {
            if (trackList.Count > 0)
                return trackList[trackList.Count - 1].getEnd();
            else
                return new Vector3();
        }
        public float getLastRotation()
        {
            if (trackList.Count > 0)
                return trackList[trackList.Count - 1].getRotation()-(float)Math.PI/2;
            else
                return 0;
        }
        public float getCurve()
        {
            return curve;
        }
        public float getRadius()
        {
            return radius;
        }
        public Vector3 getStart()
        {
            return trackList[0].getStart();
        }
        public float getFirstRotation()
        {
            return trackList[0].getRotation();
        }
        public TrackCurve getNextTrack()
        {
            return nextTrack;
        }
        public void setNextTrack(TrackCurve t)
        {
            nextTrack = t;
        }
        public Vector3 getPosition()
        {
            return position;
        }
        public void addProgress(float dr)
        {
            rotProgress += dr;
        }
        public float getProgress()
        {
            return rotProgress;
        }
        public trackTypes getType()
        {
            return typ;
        }
        public float getLastCurveRotation()
        {
            return lastCurveRotation;
        }
        public void setLastCurveRotation(float last)
        {
            lastCurveRotation = last;
        }
        public bool isEndTrack()
        {
            return endTrack;
        }
        public void makeEndPiece()
        {
            endTrack = true;
        }
    }
}
