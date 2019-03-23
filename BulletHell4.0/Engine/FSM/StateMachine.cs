using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BulletHell4_0
{
    public class StateMachine
    {
        Dictionary<int, State> states;
        State currentState;
        public GameObject Owner { get; protected set; }
        

        public StateMachine(GameObject owner)
        {
            states = new Dictionary<int, State>();
            Owner = owner;
        }


        public void RegisterState(int id, State state)
        {
            states.Add(id, state);
            state.AssignStateMachine(this);
        }

        public void Switch(int id)
        {
            if (currentState != null)
                currentState.Exit();
            currentState = states[id];
            currentState.Enter();
        }

        public void Run()
        {
            if (currentState != null)
                currentState.Update();
        }
    }
}
