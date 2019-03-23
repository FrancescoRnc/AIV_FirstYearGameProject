using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BulletHell4_0
{
    public class Animation : IUpdatable, IActivable, ICloneable
    {
        //GameObject gameObj;
        public string Name;
        public float FrameWidth { get; private set; }
        public float FrameHeight { get; private set; }
        public float OffsetX { get; private set; }
        public float OffsetY { get; private set; }
        public int CurrentFrame
        {
            get { return currentIndex; }
            private set
            {
                currentIndex = value;
                OffsetX = startX + (currentIndex % cols) * FrameWidth;
                OffsetY = startY + (currentIndex / cols) * FrameHeight;
            }
        }
        public bool IsActive { get; set; }

        int numFrames;
        int cols;
        int rows;
        int startX;
        int startY;
        Timer timer;
        int currentIndex;
        int fps;
        bool loop;
        

        public Animation(float frameW, float frameH, string name = "", int rows = 1, int cols = 1, int fps = 1, bool loop = true, int startX = 0, int startY = 0)
        {
            Name = name;
            FrameWidth = frameW;
            FrameHeight = frameH;
            this.cols = cols;
            this.rows = rows;
            this.startX = startX;
            this.startY = startY;
            numFrames = cols * rows;  
            this.loop = loop;
            float totaltime;
            this.fps = fps;
            if (this.fps == 0)
                totaltime = 0;
            else
                totaltime = 1 / (float)this.fps;
            timer = new Timer(0, totaltime);
            IsActive = true;
            CurrentFrame = 0;
            
        }


        public void SetStartPosition(int x, int y)
        {
            startX = x;
            startY = y;
        }

        void OnAnimationEnd()
        {
            if (loop)
                CurrentFrame = 0;
            else
                IsActive = false;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }


        public void Update()
        {
            if (IsActive)
            {
                timer.Update(Game.window.deltaTime);
                if (timer.EndTime())
                {
                    timer.Reset();
                    CurrentFrame++;
                    if (CurrentFrame >= numFrames)
                        OnAnimationEnd();
                }
            }
        }        
    }
}
