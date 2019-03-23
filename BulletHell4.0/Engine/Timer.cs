using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHell4_0
{
    public class Timer
    {
        public float CurrTime;
        public float MaxTime;


        public Timer(float currtime, float maxtime)
        {
            CurrTime = currtime;
            MaxTime = maxtime;
        }

        public void Update(float passedtime)
        {
            if (CurrTime < MaxTime)
            {
                CurrTime += passedtime;
                if (CurrTime >= MaxTime)
                    CurrTime = MaxTime;
            }
        }

        public bool EndTime()
        {
            return CurrTime >= MaxTime;
        }

        public void Reset()
        {
            CurrTime = 0;
        }
    }

    static public class TimeMgr
    {
        static List<Timer2> timers;

        static public void Init()
        {
            timers = new List<Timer2>();
        }

        static public void AddTimer(Timer2 timer)
        {
            if (!timers.Contains(timer))
                timers.Add(timer);
            Console.WriteLine(timers.Count);
        }

        static public void RemoveTimer(Timer2 timer)
        {
            if (timers.Contains(timer))
                timers.Remove(timer);
        }

        static public void RemoveAll()
        {
            timers.Clear();
        }


        static public void Update()
        {
            for (int i = 0; i < timers.Count; i++)
            {
                timers[i].Update();
            }
        }
    }

    public class Timer2 : IUpdatable
    {
        public float CurrentTime;
        public float MaxTime;
        public bool OnEndTime
        {
            get
            {
                if (hasMaxTime)
                    return CurrentTime >= MaxTime;
                return false;
            }
        }
        bool hasMaxTime;


        public Timer2(float maxtime = 0)
        {
            CurrentTime = 0;
            if (maxtime == 0)
                hasMaxTime = false;
            else
                hasMaxTime = true;
            MaxTime = maxtime;
            TimeMgr.AddTimer(this);
        }


        public void Start()
        {
            Reset();
            UpdateMgr.Add(this);
        }

        public void Pause()
        {
            UpdateMgr.Remove(this);
        }

        public void Resume()
        {
            UpdateMgr.Add(this);
        }

        public void End()
        {
            UpdateMgr.Remove(this);
            TimeMgr.RemoveTimer(this);
        }

        public void Reset()
        {
            CurrentTime = 0;
        }


        public void Update()
        {
            CurrentTime += Game.window.deltaTime;
            if (hasMaxTime)
            {
                if (CurrentTime > MaxTime)
                {
                    CurrentTime = MaxTime;
                }
            }

            Console.Clear();
            Console.WriteLine(CurrentTime);
        }
    }
}
