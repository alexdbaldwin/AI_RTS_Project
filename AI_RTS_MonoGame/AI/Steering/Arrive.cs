using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.AI.Steering
{
    class Arrive : SteeringBehaviour
    {
        protected Vector2 position;
        protected float tolerance;

        public Arrive(GameplayManager gm, Unit owner, Vector2 position, float tolerance = 5.0f) : base(gm, owner) {
            this.position = position;
            this.tolerance = tolerance;
        }
        public override Vector2 GetLinearAcceleration()
        {
            Vector2 direction = position - owner.Position;
            float distance = direction.Length();
            if (distance < tolerance)
            {
                return Vector2.Zero;
            }
            else
            {
                direction.Normalize();
                return direction * owner.MaxAcceleration;
            }
        }

    }
}
