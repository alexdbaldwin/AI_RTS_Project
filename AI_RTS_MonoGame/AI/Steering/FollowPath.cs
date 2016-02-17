using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.AI.Steering
{
    class FollowPath : Arrive
    {
        protected Path path;
        protected float previousPathParam;
        protected float lookAheadAmount;
        public FollowPath(GameplayManager gm, Unit owner, Path path, float lookAheadAmount = 20.0f) : base(gm, owner, path.GetPoint(0)) {
            this.lookAheadAmount = lookAheadAmount;
            this.path = path;
            previousPathParam = 0.0f;
        }

        public override Vector2 GetLinearAcceleration()
        {
            float newParam = path.GetParam(owner.Position, previousPathParam);
            position = path.GetPosition(newParam + lookAheadAmount);
            previousPathParam = newParam;
            return base.GetLinearAcceleration();
        }

    }
}
