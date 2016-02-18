using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_RTS_MonoGame
{

    internal abstract class ArmyController
    {
        protected GameplayManager gm;
        protected List<IAttackable> selection = new List<IAttackable>();
        protected int faction;
        protected int resources = 100;

        internal int Resources {
            get { return resources; }
            set { resources = value; }
        }

        public int Faction {
            get {
                return faction;
            }
        }

        public ArmyController(GameplayManager gm, int faction) {
            this.gm = gm;
            this.faction = faction;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Deselect(IAttackable s) {
            selection.Remove(s);
        }

        protected bool SpawnPowerPlant(int gridX, int gridY) {
            return gm.SpawnPowerPlant(gridX, gridY, faction);
        }


    }
}
