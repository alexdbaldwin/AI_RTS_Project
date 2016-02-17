using AI_RTS_MonoGame.AI.Steering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.AI.FSM
{
    class StateChase : UnitFSMState
    {
        public StateChase(UnitController controller, GameplayManager gm) : base(FSMStates.Chase, controller, gm) { }

        public override void Enter()
        {
            controller.SetSteering(new Chase(gm, controller.ControlledUnit, controller.AttackTarget));
        }
        public override void Exit()
        {

        }
        public override void Update(GameTime gameTime)
        {
            //controller.ControlledUnit.SetVelocity(Vector2.Zero);
            
            //Vector2 dir = controller.AttackTarget.Position - controller.ControlledUnit.Position;
            //dir.Normalize();
            //controller.ControlledUnit.SetVelocity(dir * controller.ControlledUnit.MovementSpeed);

        }
        public override void Init()
        {

        }
        public override FSMStates CheckTransitions()
        {
            if (controller.AttackTarget == null)
                return FSMStates.Idle;

            float distance = AttackableHelper.Distance(controller.ControlledUnit, controller.AttackTarget);
            if (distance <= controller.ControlledUnit.AttackRange)
            {
                return FSMStates.Attack;
            }
            else if (distance <= controller.ControlledUnit.VisionRange)
            {
                return FSMStates.Chase;
            }
            else //Shouldn't happen unless vision range is lower than attack range
            {
                return FSMStates.Idle;
            }
        }
    }
}
