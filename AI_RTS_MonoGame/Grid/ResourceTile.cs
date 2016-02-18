using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame
{
    class ResourceTile : Tile
    {

        float resources;

        public float Resources {
            get { return resources; }
            set { resources = value; }
        }

        public ResourceTile (int x, int y, Rectangle bounds, int resources) : base(x,y,bounds){
            resourceTile = true;
            this.resources = resources;

        }

    }
}
