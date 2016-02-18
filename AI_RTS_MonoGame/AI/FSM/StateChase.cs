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
        bool followingPath = false;
        public StateChase(UnitController controller, GameplayManager gm) : base(FSMStates.Chase, controller, gm) { }

        public override void Enter()
        {
            controller.SetSteering(new Chase(gm, controller.ControlledUnit, controller.AttackTarget));
            followingPath = false;
        }
        public override void Exit()
        {

        }
        public override void Update(GameTime gameTime)
        {
            if (gm.LineOfSight(controller.ControlledUnit, controller.AttackTarget))
            {
                if (followingPath)
                {
                    controller.SetSteering(new Chase(gm, controller.ControlledUnit, controller.AttackTarget));
                }
                followingPath = false;
            }
            else {
                if (followingPath)
                {
                    if (Vector2.Distance(controller.PathToFollow.GetPoint(controller.PathToFollow.PointCount() - 1), controller.AttackTarget.Position) > 50.0f) {
                        if(controller.AttackTarget is Building)
                            controller.PathToFollow = gm.GetPath(controller.ControlledUnit.Position, controller.AttackTarget.Position, 200.0f/*(controller.AttackTarget as Building).Radius + Grid.TileSize*/);
                        else
                            controller.PathToFollow = gm.GetPath(controller.ControlledUnit.Position, controller.AttackTarget.Position);
                        controller.SetSteering(new BlendedFollowPath(gm, controller.ControlledUnit, controller.PathToFollow));
                    }
                }
                else {
                    if (controller.AttackTarget is Building)
                        controller.PathToFollow = gm.GetPath(controller.ControlledUnit.Position, controller.AttackTarget.Position, ((controller.AttackTarget as Building).Radius + Grid.TileSize) / Grid.TileSize);
                    else
                        controller.PathToFollow = gm.GetPath(controller.ControlledUnit.Position, controller.AttackTarget.Position);
                    controller.SetSteering(new BlendedFollowPath(gm, controller.ControlledUnit, controller.PathToFollow));
                    followingPath = true;
                }
            }

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
            {
                if (controller.AttackMoving)
                {
                    controller.AttackMove(controller.AttackMoveDestination);
                    return FSMStates.AttackMove;
                }
                return FSMStates.Idle;
            }

            float distance = AttackableHelper.Distance(controller.ControlledUnit, controller.AttackTarget);
            if (distance <= controller.ControlledUnit.AttackRange)
            {
                return FSMStates.Attack;
            }
            else if (distance <= controller.ControlledUnit.VisionRange)
            {
                return FSMStates.Chase;
            }
            else if(!gm.IsInSightRange(controller.AttackTarget,controller.ControlledUnit.Faction))//Shouldn't happen unless vision range is lower than attack range
            {
                if (controller.AttackMoving)
                {
                    controller.AttackMove(controller.AttackMoveDestination);
                    return FSMStates.AttackMove;
                }

                return FSMStates.Idle;
            }
            return FSMStates.Chase;
        }
    }
}
