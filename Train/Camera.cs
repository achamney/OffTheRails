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
    public class Camera
    {
        Vector3 position;
        Vector3 lookAt;
        Vector3 velocity;
        Vector3 rotation;
        float lastRotation;
        bool rotate;
        Vector3 targetPosition;
        int circleTime;
        float viewingAngle,maxViewingAngle;
        States state;
        public enum States { FOLLOWTARGET, CIRCLETARGET, ZOOMTOTARGET };
        public Camera(Vector3 camPos, Vector3 camLookAt,Vector3 targetPos)
        {
            position = camPos;
            lookAt = camLookAt;
            viewingAngle = maxViewingAngle = (float)Math.Atan2(targetPos.Z - camPos.Z, targetPos.X - camPos.X);
            state = States.CIRCLETARGET;
            circleTime = 400;
            if (state == States.CIRCLETARGET)
            {
                position.X = 10;
                position.Z =-20;
            }
        }
        public void handleInput(KeyboardState kbs)
        {
            if(kbs.IsKeyDown(Keys.P))
            {
                viewingAngle += 0.01f;
            }
            if (kbs.IsKeyDown(Keys.O))
            {
                viewingAngle -= 0.01f;
            }
            if (kbs.IsKeyDown(Keys.Space))
            {
                rotate = true;
            }
            else
                rotate = false;

            if (circleTime == 0 && state == States.CIRCLETARGET)
                changeStates(States.ZOOMTOTARGET);
            else
                circleTime--;
            
            MathHelper.Clamp(viewingAngle, maxViewingAngle, -maxViewingAngle);
        }
        public void update(Vector3 trainPos, float yRot, Vector3 trainScale, Vector3 tVelocity,int direc)
        {
            /// find the magnetude of distance between the cam and train.
            /// The further the distance, the faster the cam follows
            targetPosition = trainPos;
            float dz = nextToTarget(trainPos, trainScale);
            
            float moveAngle = yRot;
            if (state == States.FOLLOWTARGET)/////////////////Camera follows the target
            {
                if (lastRotation != yRot)
                {
                    if (lastRotation - yRot > 6)
                        lastRotation = yRot;
                    else if (lastRotation - yRot < -6)
                        lastRotation = yRot;
                    lastRotation += -(lastRotation - yRot) / 20;
                }
               moveAngle = lastRotation;
               Vector3 optimalPosition;
               if (direc > 0)
               {
                   optimalPosition = trainPos + new Vector3((float)Math.Sin(moveAngle - 0.2f) * 10, 0, (float)Math.Cos(moveAngle - 0.2f) * 10);
               }
               else
               {
                   optimalPosition = trainPos + 
                       new Vector3((float)Math.Sin(moveAngle - 0.2f) * 10 , 0, (float)Math.Cos(moveAngle - 0.2f) * 10 ) +
                       new Vector3((float)Math.Sin(moveAngle - 0.2f + Math.PI / 2) * 7, 0, (float)Math.Cos(moveAngle - 0.2f + Math.PI / 2) * 7);
               }
               dz = nextToTarget(optimalPosition, new Vector3());
               float direction = (float)Math.Atan2((position.Z - optimalPosition.Z), position.X - optimalPosition.X);
               velocity.Z = -(float)Math.Sin(direction) * dz * dz / 400;
               velocity.X = -(float)Math.Cos(direction) * dz * dz / 400;
               velocity.Z *= 0.99f; // slows the camera down
               velocity.X *= 0.99f; // slows the camera down
               rotation.Y = (float)(Math.Atan2(lookAt.Z - position.Z, lookAt.X - position.X));
               position.Z += velocity.Z;
               position.X += velocity.X;
               position.Y = (4 - 4 / (Math.Abs(dz / 30) + 1));
               lookAt = trainPos + new Vector3(-(float)Math.Sin(moveAngle) * (10 + dz), 0, -(float)Math.Cos(moveAngle) * (10 + dz));
            }
            else if (state == States.CIRCLETARGET) /// Camera circles the target
            {
                
                if (dz > 20)
                {
                    float direction = (float)Math.Atan2((position.Z - trainPos.Z), position.X - trainPos.X);
                    position.X += -(float)Math.Cos(direction) * dz / 60 + tVelocity.Z;
                    position.Z -= (float)Math.Sin(direction) * dz / 60 + tVelocity.X;
                }
                position.X += (float)Math.Cos(rotation.Y) * 1.5f + tVelocity.X;
                position.Z += (float)Math.Sin(rotation.Y) * 1.6f + tVelocity.Z;
                if(!rotate)
                rotation.Y += 0.02f;
                position.Y = 3;
                lookAt = trainPos;
            }
            else if (state == States.ZOOMTOTARGET) /// Camera zooms to the target
            {
                //float dz = nextToTarget(trainPos, trainScale);
                float Angle = (float)Math.Atan2(trainPos.Z - position.Z, trainPos.X - position.X);
                velocity.Z = -(float)Math.Cos(Angle+Math.PI/2) * dz / 70 + tVelocity.Z;
                velocity.X = (float)Math.Sin(Angle + Math.PI / 2) * dz / 70 + tVelocity.X;
                if (Angle + 0.01 < viewingAngle || Angle - 0.1 > viewingAngle)
                {
                    if (Angle < viewingAngle)
                    {
                        velocity.Z += -(float)Math.Cos(Angle) * 0.2f;
                        velocity.X += (float)Math.Sin(Angle) * 0.2f;
                    }
                    else
                    {
                        velocity.Z -= -(float)Math.Cos(Angle) * 0.2f;
                        velocity.X -= (float)Math.Sin(Angle) * 0.2f;
                    }
                }

                if (dz <= 4f)
                {
                    changeStates(States.FOLLOWTARGET);
                }
                
                position.Z += velocity.Z;
                position.X += velocity.X;
                if (position.Y > 1)
                    position.Y -= dz / 100;
               
                lookAt = trainPos;
                rotation.Y = (float)(Math.Atan2(lookAt.Z - position.Z, lookAt.X - position.X));
            }
           
        }

        ///Get and Set Methods
        public void setPosition(Vector3 pos) { position.X = pos.X; position.Y = pos.Y; position.Z = pos.Z; }
        public void setLookAt(Vector3 look) { lookAt.X = look.X; lookAt.Y = look.Y; lookAt.Z = look.Z; }

        public Vector3 getPosition() { return position; }
        public Vector3 getLookAt() { return lookAt; }
        public void setState(States s) { state = s; }
        public States getState() { return state; }
        ///Get and Set Methods
        ///

        public float nextToTarget(Vector3 trainPos, Vector3 trainScale)
        {
            float dist = getDistance(trainPos);
            if (dist + trainScale.Z > 0 && dist - trainScale.Z < 0)
                return 0;
            if( dist + trainScale.Z < 0)
                return dist + trainScale.Z;
            else
                return dist - trainScale.Z;
        }
        public float getDistance(Vector3 trainPos)
        {
            return (float)Math.Sqrt((trainPos.X - position.X) * (trainPos.X - position.X) +
                (trainPos.Z - position.Z) * (trainPos.Z - position.Z));
        }
        public void changeStates(States state)
        {
            this.state = state;
            /*if (state == States.ZOOMTOTARGET)
            {
                position.X = 0;
                position.Z = 0;
            }*/
        }
        public Vector3 getRotation()
        {
            return rotation;
        }
    }
}
