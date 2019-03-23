using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace BulletHell4_0
{
    public class VisualEffect : IUpdatable
    {
        public float CurrentTime;
        public float TotalTime;
        public bool OnEnding { get { return CurrentTime >= TotalTime; } }
        protected GameObject owner;


        public VisualEffect(GameObject aff, float totTime)
        {
            owner = aff;
            TotalTime = totTime;
        }

        public void TimeUpdate(float deltaTime)
        {
            CurrentTime += deltaTime;
        }

        public virtual void BeginEffect()
        {
            CurrentTime = 0;
            UpdateMgr.Add(this);
        }

        public virtual void EndEffect()
        {            
            UpdateMgr.Remove(this);
        }

        public virtual void Update()
        {
            if (OnEnding)
                EndEffect();
        }
    }


    public class FadingEffect : VisualEffect
    {
        public float TotalFrameTime;
        int direction;


        public FadingEffect(GameObject aff, float totTime) : base(aff, totTime)
        {
            TotalFrameTime = (TotalTime * 0.5f);
            direction = 1;
        }

        public override void Update()
        {
            if (CurrentTime >= TotalFrameTime)
            {
                direction = -1;
            }

            TimeUpdate(Game.window.deltaTime * direction);
            float perc = CurrentTime / (TotalFrameTime);

            owner.Sprite.SetMultiplyTint(perc, perc, perc, perc);

            base.Update();
        }
    }


    public class BlinkingEffect : VisualEffect
    {
        float currblinkTime;
        float maxBlinks;
        float maxBlinkTime;
        bool isVisible;

        public BlinkingEffect(GameObject aff, float totTime, float blinks) : base(aff, totTime)
        {
            maxBlinks = blinks;
            maxBlinkTime = 1 / maxBlinks;
        }


        public override void EndEffect()
        {
            owner.Sprite.SetMultiplyTint(1, 1, 1, 1);
            base.EndEffect();
        }


        public override void Update()
        {
            TimeUpdate(Game.window.deltaTime);

            //float blink = (float)Math.Abs(Math.Sin(CurrentTime * rate));
            //float clamp = MathHelper.Clamp(blink, 0, 1);
            //
            //afflicted.Sprite.SetMultiplyTint(clamp, clamp, clamp, clamp);

            currblinkTime += Game.window.deltaTime;
            if (currblinkTime >= maxBlinkTime)
            {
                currblinkTime = 0;
                if (isVisible)
                {
                    owner.Sprite.SetMultiplyTint(1, 1, 1, 1);
                    isVisible = false;
                }
                else
                {
                    owner.Sprite.SetMultiplyTint(0, 0, 0, 0);
                    isVisible = true;
                }
            }

            base.Update();
        }
    }
}
