using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.AI.Steering
{
    class BlendedChase : BlendedSteeringBehaviour
    {

        public BlendedChase(GameplayManager gm, Unit owner, Unit target)
            : base(gm, owner, new List<Tuple<SteeringBehaviour, float>> { 
                new Tuple<SteeringBehaviour,float>(new Chase(gm,owner,target),1.0f),
                new Tuple<SteeringBehaviour,float>(new Separate(gm,owner),0.5f),
                new Tuple<SteeringBehaviour,float>(new Clump(gm,owner),0.5f)
            }) { }
    }
}
