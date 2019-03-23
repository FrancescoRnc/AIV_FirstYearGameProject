using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    enum States { }

    public abstract class State
    {
        protected StateMachine machine;
        protected GameObject owner { get { return machine.Owner; } }


        public void AssignStateMachine(StateMachine sm)
        {
            machine = sm;
        }

        public virtual void Enter()
        {

        }
               
        public virtual void Exit()
        {

        }
               
        public virtual void Update()
        {

        }
    }
}
