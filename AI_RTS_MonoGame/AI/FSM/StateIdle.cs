using AI_RTS_MonoGame.AI.Steering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.AI.FSM
{
    class StateIdle : UnitFSMState
    {
        public StateIdle(UnitController controller, GameplayManager gm) : base(FSMStates.Idle, controller, gm) { }

        public override void Enter()
        {
            controller.SetSteering(new StandStill(gm, controller.ControlledUnit));
        }
        public override void Exit()
        {

        }
        public override void Update(GameTime gameTime)
        {
            controller.ControlledUnit.SetVelocity(Vector2.Zero);
        }
        public override void Init()
        {

        }
        public override FSMStates CheckTransitions()
        {
            Attackable nearestTarget = controller.ControlledUnit.GetNearestTargetInAggroRange();
            if (nearestTarget != null)
            {
                controller.AttackTarget = nearestTarget;
                return FSMStates.Attack;
            }
            return FSMStates.Idle;
        }
    }
}
