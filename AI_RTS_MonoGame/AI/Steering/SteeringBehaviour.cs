using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.AI.Steering
{

    abstract class SteeringBehaviour
    {

        protected GameplayManager gm;
        protected Unit owner;

        public SteeringBehaviour(GameplayManager gm, Unit owner){
            this.gm = gm;
            this.owner = owner;
        }

        public abstract Vector2 GetLinearAcceleration();

        public virtual void Steer(float dt) { 
            Vector2 vel = owner.GetVelocity();
            vel += GetLinearAcceleration() * dt;
            if (vel.Length() > owner.MovementSpeed) {
                vel.Normalize();
                vel *= owner.MovementSpeed;
            }
            owner.SetVelocity(vel);
        }


    }
}
