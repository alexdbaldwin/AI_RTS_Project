using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.AI.Steering
{
    class Chase : Arrive
    {
        Attackable target;
        public Chase(GameplayManager gm, Unit owner, Attackable target) : base(gm, owner, target.Position) {
            this.target = target;
        }

        public override Vector2 GetLinearAcceleration()
        {
            position = target.Position;
            return base.GetLinearAcceleration();
        }
    }
}
