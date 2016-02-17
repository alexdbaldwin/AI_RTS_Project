using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.AI.Steering
{
    class Separate : SteeringBehaviour
    {
        protected float threshold;
        protected float decayCoefficient;

        public Separate(GameplayManager gm, Unit owner) : base(gm, owner) {
            threshold = 100.0f;
            decayCoefficient = 1000.0f;
        }

        public override Vector2 GetLinearAcceleration()
        {

	        Vector2 linearAcceleration = Vector2.Zero;

            List<Attackable> friendly = gm.GetAllFriendlyUnitsInRange(owner, 100.0f);
            foreach(Attackable a in friendly){
                Vector2 direction = owner.Position - a.Position;
			    float distance = direction.Length();
			    if(distance < threshold){
				    float strength = owner.MaxAcceleration * (threshold - distance) / threshold;
				    direction.Normalize();
				    linearAcceleration += strength * direction;
			    }
            }
            if(linearAcceleration.Length() > owner.MaxAcceleration){
                linearAcceleration.Normalize();
                linearAcceleration *= owner.MaxAcceleration;
            }

            return linearAcceleration;
        }
    }
}
