using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.AI.Steering
{
    class BlendedFollowPath : BlendedSteeringBehaviour
    {
        public BlendedFollowPath(GameplayManager gm, Unit owner, Path path)
            : base(gm, owner, new List<Tuple<SteeringBehaviour, float>> { 
                new Tuple<SteeringBehaviour,float>(new FollowPath(gm,owner,path),1.0f),
                new Tuple<SteeringBehaviour,float>(new Separate(gm,owner),0.8f),
                new Tuple<SteeringBehaviour,float>(new Clump(gm,owner),0.5f)
            }) { }
    }
}
