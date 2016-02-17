using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.AI.Steering
{
    class StandStill : SteeringBehaviour
    {

        public StandStill(GameplayManager gm, Unit owner) : base(gm, owner) { 
        }

        public override Vector2 GetLinearAcceleration()
        {
            return Vector2.Zero;
        }

        public override void Steer(float dt)
        {
            owner.SetVelocity(Vector2.Zero);
        }
    }
}
