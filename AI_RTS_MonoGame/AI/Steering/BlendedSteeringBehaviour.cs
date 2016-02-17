using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.AI.Steering
{
    class BlendedSteeringBehaviour : SteeringBehaviour
    {
        protected List<Tuple<SteeringBehaviour, float>> weightedBehaviours;
        protected Vector2 linearAcceleration;

        public BlendedSteeringBehaviour(GameplayManager gm, Unit owner, List<Tuple<SteeringBehaviour, float>> weightedBehaviours) : base(gm, owner) {
            this.weightedBehaviours = weightedBehaviours;
        }

        public override Vector2 GetLinearAcceleration()
        {
            return linearAcceleration;
        }

        public override void Steer(float dt)
        {
            linearAcceleration = Vector2.Zero;
            foreach (Tuple<SteeringBehaviour, float> pair in weightedBehaviours) {
                linearAcceleration += pair.Item1.GetLinearAcceleration() * pair.Item2;
            }
            if (linearAcceleration.Length() > owner.MaxAcceleration)
            {
                linearAcceleration.Normalize();
                linearAcceleration *= owner.MaxAcceleration;
            }
            
            base.Steer(dt);
        }
    }
}
