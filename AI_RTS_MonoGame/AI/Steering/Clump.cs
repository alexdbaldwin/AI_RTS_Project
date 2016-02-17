using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.AI.Steering
{
    class Clump : SteeringBehaviour
    {
        float threshold;

        public Clump(GameplayManager gm, Unit owner) : base(gm, owner) { 
        
        }

        public override Vector2 GetLinearAcceleration()
        {
            Vector2 linearAcceleration = Vector2.Zero;
            Vector2 center = owner.Position;

            List<Attackable> friendly = gm.GetAllFriendlyUnitsInRange(owner, 100.0f);
            foreach (Attackable a in friendly)
            {
                center += a.Position;
            }
            center /= friendly.Count + 1;

            Vector2 direction = center - owner.Position;
            float distance = direction.Length();
            if (distance < threshold)
            {
                float strength = owner.MaxAcceleration * (threshold - distance) / threshold;
                direction.Normalize();
                linearAcceleration = strength * direction;
            }

            if (linearAcceleration.Length() > owner.MaxAcceleration)
            {
                linearAcceleration.Normalize();
                linearAcceleration *= owner.MaxAcceleration;
            }

            return linearAcceleration;
        }
    }
}
