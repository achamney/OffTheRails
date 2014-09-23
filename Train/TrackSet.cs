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
    class TrackSet
    {
        List<TrackCurve> curveList;
        List<Object3D> scenery;
        List<ScenePiece> sceneModels;
        Model cactus;
        public TrackSet(MainTrain train, Model ca)
        {
            curveList = new List<TrackCurve>();
            scenery = new List<Object3D>();
            sceneModels = new List<ScenePiece>();

            cactus = ca;
            createSceneModels();
            Vector3 lastTrack = getLastPos();
            lastTrack = addStraight(new Vector3(0, -3.4f, 300), new Vector3(0, -3.4f, 200));
            lastTrack = addStraight(lastTrack, lastTrack + new Vector3(0, 0, -100));
            lastTrack = addStraight(lastTrack, lastTrack + new Vector3(0, 0, -100));
            lastTrack = addStraight(lastTrack, lastTrack + new Vector3(0, 0, -100));
            //
            
            
            //
            lastTrack = this.addCurve(lastTrack, (float)Math.PI*0.2f, 100);
            
            HalfPipe c = new HalfPipe(lastTrack + new Vector3(0, 30, 0), new Vector3(30, 30, 300));
            c.rotation.Y = getLastRotation() ;
            c.rotation.Z = (float)Math.PI;
            scenery.Add(c);
            

            lastTrack = addStraight(lastTrack, lastTrack +new Vector3(0, 0, -150));
            lastTrack = addStraight(lastTrack, lastTrack + new Vector3(0, 0, -150));
            EnemySet.createStar(lastTrack, getLastRotation());
            for (int i = 0; i < 5; i++)
                EnemySet.createFly(lastTrack + new Vector3(-200 - (float)Game1.rand.NextDouble() * 100, 25, (float)-Game1.rand.NextDouble() * 100 - 100));
            lastTrack = this.addCurve(lastTrack, (float)Math.PI, 100);

            ///////
            ///Suicide enemy row
            ///////
            lastTrack = addStraight(lastTrack, lastTrack + new Vector3(0, 0, -20));
           // EnemySet.createSuicide(lastTrack + new Vector3(20, 9, -3));
            //EnemySet.createSuicide(lastTrack + new Vector3(-20, 9, -3));
            lastTrack = addStraight(lastTrack, lastTrack + new Vector3(0, 0, -20));
            //EnemySet.createSuicide(lastTrack + new Vector3(20, 9, -3));
            //EnemySet.createSuicide(lastTrack + new Vector3(-20, 9, -3));
            lastTrack = addStraight(lastTrack, lastTrack + new Vector3(0, 0, -20));
            //EnemySet.createSuicide(lastTrack + new Vector3(20, 9, -3));
            //EnemySet.createSuicide(lastTrack + new Vector3(-20, 9, -3));
            ///////
            ///Suicide enemy row
            ///////

            lastTrack = this.addCurve(lastTrack, -(float)Math.PI * 0.6f, 100);
            Vector3 left = Vector3.Transform(Vector3.Left, Matrix.CreateRotationY(getLastRotation())) * 3;
            Vector3 right = Vector3.Transform(Vector3.Right, Matrix.CreateRotationY(getLastRotation())) * 3;
            Vector3 forward = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(getLastRotation())) * 3;
            EnemySet.createSuicide(lastTrack + left + forward * 6 + Vector3.Up*3);
            EnemySet.createSuicide(lastTrack + right + forward * 6 + Vector3.Up * 3);
            for (int i = 0; i < 5; i++)
                EnemySet.createFly(lastTrack + new Vector3(- 200-(float)Game1.rand.NextDouble() * 400-25, 25, (float)Game1.rand.NextDouble() * 300-25));
            lastTrack = addStraight(lastTrack, lastTrack + new Vector3(0, 0, -200));
            EnemySet.createStar(lastTrack+ new Vector3(0,-5,0), getLastRotation());
            lastTrack = this.addCurve(lastTrack, (float)Math.PI /1f, 100);
            lastTrack = addStraight(lastTrack, lastTrack + new Vector3(0, 0, -60));
            EnemySet.createStar(lastTrack + new Vector3(0, -5, 0), getLastRotation());
            c = new HalfPipe(lastTrack + new Vector3(0, 30, 0), new Vector3(30, 30, 220));
            c.rotation.Y = getLastRotation();
            c.rotation.Z = (float)Math.PI;
            scenery.Add(c);
            left = Vector3.Transform(Vector3.Left,Matrix.CreateRotationY(getLastRotation()))*3;
            right = Vector3.Transform(Vector3.Right,Matrix.CreateRotationY(getLastRotation()))*3;
            forward = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(getLastRotation())) * 3;
            for (int i = 0; i < 3; i++)
            {
                sceneModels.Add(new ScenePiece(cactus, lastTrack + left + forward*i + Vector3.Up*3.5f));
                sceneModels.Add(new ScenePiece(cactus, lastTrack + right + forward * i + Vector3.Up * 3.5f));
            }
            lastTrack = addStraight(lastTrack, lastTrack + new Vector3(0, 0, -150));
            for (int i = 0; i < 2; i++)
            {
                sceneModels.Add(new ScenePiece(cactus, lastTrack + left + forward * i + Vector3.Up * 3.5f));
                sceneModels.Add(new ScenePiece(cactus, lastTrack + right + forward * i + Vector3.Up * 3.5f));
            }
            lastTrack = addStraight(lastTrack, lastTrack + new Vector3(0, 0, -150));
            lastTrack = this.addCurve(lastTrack, -(float)Math.PI / 2f, 100);

            left = Vector3.Transform(Vector3.Left, Matrix.CreateRotationY(getLastRotation())) * 3;
            right = Vector3.Transform(Vector3.Right, Matrix.CreateRotationY(getLastRotation())) * 3;
            EnemySet.createSuicide(lastTrack + left + Vector3.Up * 3);
            EnemySet.createSuicide(lastTrack + right + Vector3.Up * 3);
            this.createEndTrack(lastTrack);
            train.setCurTrack(curveList[1]);
        }
        public void Draw(GraphicsDevice gd,Vector3 camPos)
        {
            foreach (TrackCurve t in curveList)
            {
                t.Draw(gd,camPos);
            }
            foreach (Object3D o in scenery)
            {
                o.Draw(gd);
            }
            
        }
        public void drawModels()
        {
            foreach (ScenePiece s in sceneModels)
            {
                s.Draw();
            }
        }
        public Vector3 addCurve(Vector3 position, float curve, float radius)
        {
            curveList.Add(new TrackCurve(position,radius,curve,getLastRotation(), TrackCurve.trackTypes.CURVE));
            if (curveList.Count > 1)
            {
                curveList[curveList.Count - 2].setNextTrack(curveList[curveList.Count - 1]);
                curveList[curveList.Count - 1].setLastCurveRotation(curveList[curveList.Count - 2].getLastRotation());
            }
            return getLastPos();
        }
        public void createEndTrack(Vector3 position)
        {
            addCurve(position, -(float)Math.PI * 2.001f, 100);
            curveList[curveList.Count - 1].makeEndPiece();
            curveList[curveList.Count - 1].setNextTrack(curveList[curveList.Count - 1]);
            curveList[curveList.Count - 1].setLastCurveRotation(curveList[curveList.Count - 2].getLastRotation());
        }
        public Vector3 addStraight(Vector3 position, Vector3 position2)
        {
            curveList.Add(new TrackCurve(position, Vector3.Distance(position,position2), 0, getLastRotation(), TrackCurve.trackTypes.STRAIGHT));
            if (curveList.Count > 1)
                curveList[curveList.Count - 2].setNextTrack(curveList[curveList.Count - 1]);
            return getLastPos();
        }
        public Vector3 getLastPos()

        {
            if (curveList.Count > 0)
                return curveList[curveList.Count - 1].getLastTrack();
            else
                return new Vector3();
        }
        public float getLastRotation()
        {
            if (curveList.Count > 0)
                return curveList[curveList.Count - 1].getLastRotation();
            else
                return 0;
        }
        public void update(MainTrain train)
        {

            if (!isOnTrack(train.getCurTrack(), train))
            {
                for (int i = 0; i < curveList.Count; i++)
                {
                    if (curveList[i].Equals(train.getCurTrack()))
                    {
                        train.setCurTrack(train.getCurTrack().getNextTrack());
                        break;
                    }
                }
            }
            for (int i = 0; i < sceneModels.Count; i++)
            {
                ScenePiece sp = sceneModels[i];
                if (Projectile.collideProj(sp) != Projectile.types.None)
                {
                    createExplosion(sp.getPosition());
                    Game1.sfx[2].Play(0.25f, 0, 0);
                    train.addScore(1);
                    sceneModels.RemoveAt(i);
                }
                if(train.isInside(sp.getPosition()))
                {
                    train.takeDmg(5);
                    Game1.sfx[2].Play(0.5f, 0, 0);
                    createExplosion(sp.getPosition());
                    sceneModels.RemoveAt(i);
                }
            }
        }
        public bool isOnTrack(TrackCurve t, MainTrain tr)
        {
            Vector3 realEnd = t.getLastTrack();
            Vector3 trpos = new Vector3(tr.position.X, -0.9f, tr.position.Z);
            float dist = Vector3.Distance(trpos, realEnd);
            if (dist < 3.5f && !t.onEnd)
            {
                t.onEnd = true;
            }
            else if(dist > 3.5f)
            {
                if (t.onEnd)
                {
                    t.onEnd = false;
                    return false;
                }
                return true;
            }
            return false;
        }
       public void createSceneModels()
       {
           for(int i=0;i<100;i++)
               sceneModels.Add(new ScenePiece(cactus, new Vector3(10 + (float)Game1.rand.NextDouble() * 1000 - 500, 0, -10 + (float)Game1.rand.NextDouble() * 1000 - 700)));
       }
       public static void createExplosion(Vector3 pos)
       {
           for (int j = 0; j < 10; j++)
           {
               float anglx = (float)(Game1.rand.NextDouble() * Math.PI * 2);
               float angly = (float)(Game1.rand.NextDouble() * Math.PI * 2);
               float magSpeed = (float)(Game1.rand.NextDouble() * 0.5f);
               //Explosion
               Particle.MakeParticle(pos,
                   new Vector3((float)Math.Sin(anglx) * (float)Math.Sin(angly) * magSpeed, (float)Math.Cos(angly) * magSpeed, (float)Math.Sin(anglx) * (float)Math.Cos(angly) * magSpeed),
                   new Vector3(),
                   Particle.Texts.SPARK,
                   50, 5, 1.01f, 0);
           }
       }
    }
}
