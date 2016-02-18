using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame.GameObjects.Attackables.Buildings
{
    class PowerPlant : Building
    {
        float collectionRate = 1.0f;
        float collectionTimer = 1.0f;
        ResourceTile powerSource;

        public PowerPlant(GameplayManager gm, int gridX, int gridY, int faction, World world, Grid grid)
            : base(gm, gridX, gridY, faction, world, 1, 1, 300,100.0f, 100, grid)
        {
            powerSource = (ResourceTile)grid.GetTile(gridX, gridY);
        }

        public override void Update(GameTime gameTime)
        {
            if (powerSource.Resources <= 0)
                return;
            collectionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (collectionTimer <= 0) {
                collectionTimer += collectionRate;
                gm.GiveResources(faction, 1);
                powerSource.Resources -= 1;
            }
        }

    }
}
