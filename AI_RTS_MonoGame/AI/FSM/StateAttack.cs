﻿using AI_RTS_MonoGame.AI.Steering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.AI.FSM
{
    class StateAttack : UnitFSMState
    {

        public StateAttack(UnitController controller, GameplayManager gm) : base(FSMStates.Attack, controller,gm) { }

        public override void Enter() {
            controller.SetSteering(new StandStill(gm, controller.ControlledUnit));
        }
        public override void Exit()
        {

        }
        public override void Update(GameTime gameTime)
        {
            controller.ControlledUnit.SetVelocity(Vector2.Zero);
            if (controller.AttackTarget == null || controller.AttackTarget.IsDead())
            {
                controller.AttackTarget = null;
                return;
            }

            controller.ControlledUnit.Attack(controller.AttackTarget);

        }
        public override void Init()
        {

        }
        public override FSMStates CheckTransitions()
        {
            if (controller.AttackTarget == null)
            {
                if (controller.AttackMoving)
                {
                    controller.AttackMove(controller.AttackMoveDestination);
                    return FSMStates.AttackMove;
                }
                return FSMStates.Idle;
            }
                

            float distance = AttackableHelper.Distance(controller.ControlledUnit, controller.AttackTarget);
            if(distance <= controller.ControlledUnit.AttackRange){
                return FSMStates.Attack;
            }
            else if (gm.IsInSightRange(controller.AttackTarget, controller.ControlledUnit.Faction))
            {
                return FSMStates.Chase;
            }
            else //Shouldn't happen unless vision range is lower than attack range
            {
                if (controller.AttackMoving)
                {
                    controller.AttackMove(controller.AttackMoveDestination);
                    return FSMStates.AttackMove;
                }
                    
                return FSMStates.Idle;
            }

            
        }

    }
}
