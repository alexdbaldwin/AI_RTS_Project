using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.AI.FSM
{
    abstract class UnitFSMState : FSMState
    {
        protected UnitController controller;
        protected GameplayManager gm;

        public UnitFSMState(FSMState.FSMStates type, UnitController controller, GameplayManager gm) :base (type){
            this.controller = controller;
            this.gm = gm;
        }

    }
}
