﻿using AI_RTS_MonoGame.AI.Steering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.AI.FSM
{
    class StateHoldPosition : UnitFSMState
    {
        public StateHoldPosition(UnitController controller, GameplayManager gm) : base(FSMStates.HoldPosition, controller, gm) { }

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
            if (controller.AttackTarget != null)
            {
                if (controller.AttackTarget.IsDead() || AttackableHelper.Distance(controller.AttackTarget, controller.ControlledUnit) > controller.ControlledUnit.AttackRange)
                {
                    controller.AttackTarget = null;
                    return;
                }
                controller.ControlledUnit.Attack(controller.AttackTarget);
            }
            else
            {
                Attackable nearestTarget = controller.ControlledUnit.GetNearestTargetInAttackRange();
                if (nearestTarget != null)
                {
                    controller.AttackTarget = nearestTarget;
                    controller.ControlledUnit.Attack(controller.AttackTarget);
                }
            }
        }
        public override void Init()
        {

        }
        public override FSMStates CheckTransitions()
        {
            return FSMStates.HoldPosition;
        }
    }
}
