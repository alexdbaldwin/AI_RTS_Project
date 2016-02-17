using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.AI.FSM
{
    class UnitFSM : FSM
    {
        public UnitFSM(UnitController controller, GameplayManager gm) {
            StateIdle idleState = new StateIdle(controller,gm);
            AddState(idleState);
            AddState(new StateMove(controller,gm));
            AddState(new StateAttack(controller,gm));
            AddState(new StateAttackMove(controller,gm));
            AddState(new StateChase(controller,gm));
            AddState(new StateHoldPosition(controller,gm));
            defaultState = idleState;
            Reset();
        }
    }
}
