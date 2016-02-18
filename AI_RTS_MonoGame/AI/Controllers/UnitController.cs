using AI_RTS_MonoGame.AI.FSM;
using AI_RTS_MonoGame.AI.Steering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame
{
    class UnitController
    {
        Unit controlledUnit;
        GameplayManager gm;
        UnitFSM fsm;
        SteeringBehaviour steering;

        public bool AttackMoving { get; set; }
        public Vector2 AttackMoveDestination { get; set; }

        public Path PathToFollow { get; set; }
        public Attackable AttackTarget { get; set; }



        public Unit ControlledUnit {
            get { return controlledUnit; }
        }

        public UnitController(Unit u, GameplayManager gm) {
            controlledUnit = u;
            this.gm = gm;
            u.Controller = this;
            fsm = new UnitFSM(this,gm);
            SetSteering(new StandStill(gm, u));
        }

        public void Detach() {
            controlledUnit = null;
        }

        public void SetSteering(SteeringBehaviour sb) {
            steering = sb;
        }
        

        public void Update(GameTime gameTime) {
            if (controlledUnit == null)
                return;
            if (AttackTarget != null && AttackTarget.IsDead())
                AttackTarget = null;

            fsm.Update(gameTime);
            if(steering != null)
                steering.Steer((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void AttackMove(Vector2 target) {
            PathToFollow = gm.GetPath(controlledUnit.Position, target);
            fsm.ChangeState(FSMState.FSMStates.AttackMove, true);
            AttackMoving = true;
            AttackMoveDestination = target;
        }

        /// <summary>
        /// Checks if the controlled unit has reached the end of the path it is following
        /// Strategy: increase distance tolerance if there are many units in the area
        /// & check if the path "param" hasn't changed much in the past few seconds
        /// </summary>
        /// <returns></returns>
        public bool HasArrived() {
            return Vector2.Distance(PathToFollow.GetPoint(PathToFollow.PointCount() - 1), ControlledUnit.Position) < 1.0f;
        }

        public void Move() { 
        
        }

        public void Attack(Attackable target) {
            AttackTarget = target;
            fsm.ChangeState(FSMState.FSMStates.Attack, true);
            AttackMoving = false;
        }

        public void HoldPosition() {
            AttackTarget = null;
            fsm.ChangeState(FSMState.FSMStates.HoldPosition, true);
            AttackMoving = false;
        }

        public void Stop() {
            AttackTarget = null;
            PathToFollow = null;
            fsm.ChangeState(FSMState.FSMStates.Idle, true);
        }

        public void FollowPath(Path path) {
            if (path == null)
                return;
            PathToFollow = path;
            fsm.ChangeState(FSMState.FSMStates.Move, true);
            AttackMoving = false;
            //previousPathParam = 0;
        }


    }
}
