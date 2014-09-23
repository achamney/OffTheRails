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
    public class MainTrain
    {
        Cube mainBox;
        Cube rear;
        Cylinder top;
        List<Cylinder> wheelList;
        Triangle cowCatcher;
        Cylinder smokeStack;
        int deadTime;
        //
        
        bool mousePressed;
        bool invincible;
        bool boosting;

        public Vector3 position;
        float acceleration;
        float magSpeed ;
        float newRotation;
        int direc;
        int power,maxPower = 200;
        int score;

        public Vector3 offset;
        Vector3 rotation;
        Vector3 velocity;
        Weapon curWeapon;
        TrackCurve curTrack;
        TrackCurve lastTrack;
        public Matrix world;

        int health, maxHealth;
        bool active;
        public MainTrain(Vector3 position)
        {
            wheelList = new List<Cylinder>();
            this.position = position;
            this.position.Y = -1f;
            health = maxHealth = 100;
            power = maxPower;
            mainBox = new Cube(position+new Vector3(0,1,4.5f),new Vector3(1.5f,1.5f,5));
            wheelList.Add(new Cylinder(position + new Vector3(-1.09f, -1.0f, -8), new Vector3(0.5f, 0.5f, 1), 2.18f));
            wheelList.Add( new Cylinder(position + new Vector3(-1.09f, -1.0f, -6.5f), new Vector3(0.5f, 0.5f, 1), 2.18f));
            wheelList.Add(new Cylinder(position + new Vector3(-1.09f, -1.0f, 0), new Vector3(0.5f, 0.5f, 1), 2.18f));
            wheelList.Add(new Cylinder(position + new Vector3(-1.09f, -1.0f, 1.5f), new Vector3(0.5f, 0.5f, 1), 2.18f));
            wheelList.Add(new Cylinder(position + new Vector3(-1.09f, -1.0f, 6), new Vector3(0.5f, 0.5f, 1), 2.18f));
            wheelList.Add(new Cylinder(position + new Vector3(-1.09f, -1.0f, 7.5f), new Vector3(0.5f, 0.5f, 1), 2.18f));
            foreach (Cylinder c in wheelList)
            {
                c.rotation.Y =(float) Math.PI / 2;
            }
            smokeStack = new Cylinder(position + new Vector3(0,1.5f,-3), new Vector3(1, 1, 1) * 0.333f, 5);
            smokeStack.rotation.X = -(float)Math.PI / 2;
            rear = new Cube(new Vector3(1,-1.6f,11f - 5),new Vector3(0.7f,0.35f,10));
            curWeapon = new GatlingGun(this,new Vector3(0.1f, 1.2f,0));
            cowCatcher = new Triangle(new Vector3(1, -2, -5f),new Vector3(1.2f,1.5f,1.5f));
            top = new Cylinder(position+ new Vector3(0,0.5f,-8.5f),new Vector3(1.5f,1.5f,8), 1);
        }
        public void Draw(GraphicsDevice graphics)
        {

            top.world = Matrix.CreateFromYawPitchRoll(newRotation,0,0)*Matrix.CreateTranslation(position);
            world = top.world;
            rear.world = world;
            cowCatcher.world = world;
            mainBox.world = world;
            smokeStack.world = world;
            if (health <= 0)
            {
                if(deadTime ==0)
                {
                    velocity.Y =2;
                    velocity.X = (float)Game1.rand.NextDouble()*10-5;
                    velocity.Z = (float)Game1.rand.NextDouble()*10-5;
                    Game1.sfx[3].Play(0.5f,0,0);
                    Game1.sfx[3].Play(0.5f, 0, 0);
                    Game1.sfx[3].Play(0.5f, 0, 0);
                    Game1.sfx[3].Play(0.5f, 0, 0);
                }
                deadTime +=1;
                velocity.Y -= 0.03f;
                if(position.Y>-1)
                position += velocity;
                TrackSet.createExplosion(position);

                world = Matrix.CreateFromYawPitchRoll(newRotation, 0, 0) * Matrix.CreateTranslation(position);
                top.world = Matrix.CreateFromYawPitchRoll(newRotation, 0, 0) * Matrix.CreateTranslation(position);
                rear.world = Matrix.CreateFromYawPitchRoll(newRotation, 0, 0) * Matrix.CreateTranslation(position);
                cowCatcher.world = Matrix.CreateFromYawPitchRoll(newRotation, 0, 0) * Matrix.CreateTranslation(position);
                mainBox.world = Matrix.CreateFromYawPitchRoll(newRotation, 0, 0) * Matrix.CreateTranslation(position);
                smokeStack.world = Matrix.CreateFromYawPitchRoll(newRotation, 0, 0) * Matrix.CreateTranslation(position);
                curWeapon.world = Matrix.CreateFromYawPitchRoll(newRotation, 0, 0) * Matrix.CreateTranslation(position);
               
            }
            foreach (Cylinder c in wheelList)
            {
                c.world = top.world;
                Game1.myeffect.Parameters["face"].SetValue(Game1.texlibrary[0]);
                c.Draw(graphics);
            }

            
            Game1.myeffect.Parameters["face"].SetValue(Game1.texlibrary[3]);
            Projectile.drawProj(graphics);
            Game1.myeffect.Parameters["face"].SetValue(Game1.texlibrary[2]);
            mainBox.Draw(graphics);
            cowCatcher.Draw(graphics);
            smokeStack.Draw(graphics);
            Game1.myeffect.Parameters["face"].SetValue(Game1.texlibrary[1]);
            top.Draw(graphics);
            rear.Draw(graphics);
            Game1.myeffect.Parameters["face"].SetValue(Game1.texlibrary[2]);
            curWeapon.Draw(graphics);
        }
        public void handleInput(KeyboardState kbs)
        {
            Particle.UpdateParticles();
            foreach (Cylinder c in wheelList)
            {
                c.rotation.Z -= magSpeed;
            }
            if (active)
            {

                if (kbs.IsKeyDown(Keys.Space) && position.Y <= 2.5f 
                    && power >= maxPower/2 && velocity.Y <=0)
                {
                    velocity.Y = 0.7f;
                    power -= maxPower/2;
                    Game1.sfx[4].Play(0.15f,0,0);
                }
            }
            Projectile.updateProj();
            acceleration = 0.004f;
            if (kbs.IsKeyDown(Keys.W) && power > 3)
            {
                acceleration = 0.02f;
                if (!boosting)
                    Game1.sfxLoop[6].Play();
                boosting = true;
                power -= 3;
            }
            else
            {
                boosting = false;
                Game1.sfxLoop[6].Stop();
            }
            if (kbs.IsKeyDown(Keys.S) && power > 3)
            {
                acceleration = -0.001f;
                power -= 3;
            }
            if (kbs.IsKeyDown(Keys.Delete))
            {
                invincible = true;
            }
            magSpeed += acceleration;
            magSpeed *= 0.99f;
            if (power < maxPower)
                power++;
            velocity.X = -(float)Math.Sin(rotation.Y) * magSpeed;
            velocity.Z = -(float)Math.Cos(rotation.Y) * magSpeed;
           
           

            direc = (int)(Math.Abs(curTrack.getCurve()) / curTrack.getCurve());
            if (curTrack.getCurve() == 0)
                direc = 1;
            float dr = 0;
            Vector3 offset = Vector3.Transform(Vector3.Forward,
            Matrix.CreateFromYawPitchRoll(rotation.Y+(float)Math.PI/2, 0, 0));
            this.offset = offset;
            if (health > 0)
            {
                position.Y += velocity.Y;
                if (position.Y > -0.9f)
                {
                    velocity.Y -= 0.02f;
                }
                else if (velocity.Y < 0)
                {
                    velocity.Y = 0;
                    for(int i=0;i<100;i++)
                        createSparks(1);
                    Game1.sfx[5].Play(0.5f, 0, 0);
                    position.Y = -0.9f;
                }
                if (curTrack.getType() == TrackCurve.trackTypes.CURVE)
                {
                    dr = magSpeed / Math.Abs(curTrack.getRadius()) * direc;
                    newRotation += dr;
                    curTrack.addProgress(dr);
                    rotation.Y = this.newRotation;

                    if (direc < 0)
                        offset.X *= 1f;
                    float lastRotation = 0;
                    if (lastTrack != null)
                        lastRotation = curTrack.getLastCurveRotation();
                    position = findPointAlongCurve(curTrack.getRadius(), curTrack.getProgress(), lastRotation, direc)
                        + curTrack.getPosition() + new Vector3(offset.X, position.Y+3.4f, offset.Z);
                    createSparks(0);
                }
                else
                {
                    dr = magSpeed;
                    curTrack.addProgress(dr);
                    position = curTrack.getStart() +
                        Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(curTrack.getFirstRotation() - (float)Math.PI / 2)) * curTrack.getProgress()
                        + new Vector3(offset.X, 3.4f + position.Y, offset.Z);
                }
            }
        }
        public void handleMouse(MouseState ms, Vector3 mPosition)
        {
            if (active)
            {
                
                //rotate the weapon to face the cursor
                curWeapon.setRotationX(mPosition.Y / 25-0.1f);
                curWeapon.setRotationY(-mPosition.X / 25-0.01f);
                curWeapon.update();
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    curWeapon.fire(mousePressed);
                    mousePressed = true;

                }
                else if (ms.RightButton == ButtonState.Pressed && power >= maxPower/2)
                {
                    curWeapon.secondFire(mousePressed);
                    mousePressed = true;
                }
                else
                {
                    mousePressed = false;
                    Game1.sfxLoop[1].Stop();
                }
            }
        }
        public Vector3 getVelocity() {
            return velocity;
            }
        public Vector3 getScale() { return mainBox.getScale(); }
        public Vector3 getRotation() { return rotation; }
        public Matrix getWorld() { return world; }
        public bool checkActive() { return active; }
        public void setActive(bool act) { active = act; }
        public void setCurTrack(TrackCurve t) 
        {
            Vector3 offset = Vector3.Transform(Vector3.Forward, 
                Matrix.CreateFromYawPitchRoll(t.getFirstRotation(), 0, 0));
            float temp = position.Y;
            position = t.getStart() + new Vector3(0, 2.5f, 0) + offset;
            position.Y = temp;
            lastTrack = curTrack;
            curTrack = t; 
            newRotation = t.getFirstRotation()-(float)Math.PI/2;
        }
        public TrackCurve getCurTrack() { return curTrack; }
        public static Vector3 findPointAlongCurve(float radius, float rotation, float lastRotation, int direction)
        {
            Vector3 start = new Vector3();
            start.X = (float)Math.Cos(rotation) * (radius) * direction - radius * direction;
            start.Z = -(float)Math.Sin(rotation) * (radius) * direction;
            if (direction > 0)
            {
                start = new Vector3(start.X + radius, start.Y, start.Z);
               // end = new Vector3(end.X + radius, end.Y, end.Z);
            }
            else
            {
                start = new Vector3(start.X - radius, start.Y, start.Z);
               // end = new Vector3(end.X - radius, end.Y, end.Z);
            }
            /////////
            Vector3 newStart = Vector3.Transform(start, Matrix.CreateRotationY(lastRotation));
           // Vector3 newEnd = Vector3.Transform(end, Matrix.CreateRotationY(lastRotation));
            ///////////
            if (direction > 0)
            {
                start = new Vector3(newStart.X - radius * (float)Math.Cos(lastRotation),
                    start.Y, newStart.Z + radius * (float)Math.Sin(lastRotation)) ;

                //end = new Vector3(newEnd.X - radius * (float)Math.Cos(lastRotation),
               //     end.Y, newEnd.Z + radius * (float)Math.Sin(lastRotation)) + lastPosition;
            }
            else
            {

                start = new Vector3(newStart.X + radius * (float)Math.Cos(lastRotation),
                    start.Y, newStart.Z - radius * (float)Math.Sin(lastRotation)) ;

               // end = new Vector3(newEnd.X + radius * (float)Math.Cos(lastRotation),
               //     end.Y, newEnd.Z - radius * (float)Math.Sin(lastRotation)) + lastPosition;
            }
            
            return start;
        }
        public Vector3 getForwardDirection()
        {
            
            return Vector3.Normalize(Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(0, rotation.Y, 0)));
        }
        public bool isInside(Vector3 pos, int radius)
        {
            //Game1.helper.position = pos;
            BoundingSphere sph = new BoundingSphere(position-offset, radius);
            BoundingSphere point = new BoundingSphere(pos, 1);
            //sph = sph.Transform(world);
            if (point.Intersects(sph))
                return true;
            return false;
        }
        public bool isInside(Vector3 pos)
        {
            return isInside(pos, 7);
        }

        public void takeDmg(int dDmg)
        {
            if(active && !curTrack.isEndTrack()&& !invincible)
            health -= dDmg;
        }
        public int getHealth()
        {
            return health;
        }
        public int getMaxHealth()
        {
            return maxHealth;
        }
        public int getPower()
        {
            return power;
        }
        public int getMaxPower()
        {
            return maxPower;
        }
        public int getScore()
        {
            return score;
        }
        public bool getActive()
        {
            return active;
        }
        public void addScore(int delta)
        {
            if (active && !curTrack.isEndTrack())
            score += delta;
        }
        public void createSparks(int size)
        {
            foreach (Cylinder c in wheelList)
            {
                if (Game1.rand.NextDouble() * 10 < 1 + magSpeed / 3)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        ///Sparks
                        float distance = Vector3.Distance(new Vector3(), c.position);
                        Particle.MakeParticle(new Vector3(c.position.Z * (float)Math.Sin(rotation.Y), -2.5f, c.position.Z * (float)Math.Cos(rotation.Y)) + position,
                            new Vector3((float)Game1.rand.NextDouble() * 0.5f, 0.3f, (float)Game1.rand.NextDouble() * 0.5f),
                            new Vector3(0, -0.02f, 0),
                            Particle.Texts.SPARK,
                            30,
                            0.1f+size,
                            0.99f,
                            0.5f);
                        //Particle.getLastParticle().World = c.world;
                    }
                }
            }
        }
        public void decEnergy(int de)
        {
            power -= de;
        }
        public int getDirection()
        { return direc; }
        public Weapon getCurWeapon()
        {
            return curWeapon;
        }
    }
   
}
