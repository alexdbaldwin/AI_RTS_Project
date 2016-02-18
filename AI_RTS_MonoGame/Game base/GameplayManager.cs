using AI_RTS_MonoGame.GameObjects.Attackables.Buildings;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AI_RTS_MonoGame
{
    internal class GameplayManager
    {
        public static readonly bool drawDebug = true;


        GameWindow window;
        Grid grid;
        List<ArmyController> armyControllers = new List<ArmyController>();
        List<UnitController> controllers = new List<UnitController>();
        List<Attackable> attackables = new List<Attackable>();
        World world;
        VisualEffectsManager vfx;

        public VisualEffectsManager VFX { get { return vfx; } }


        public GameplayManager(GameWindow window) {
            world = new World(Vector2.Zero);
            ConvertUnits.SetDisplayUnitToSimUnitRatio(20.0f); //Not sure what a good number is here?
            vfx = new VisualEffectsManager();

            this.window = window;
            grid = new Grid(world, this);
            grid.LoadLevel();
            armyControllers.Add(new PlayerController(this, 0));

            SpawnUnit(new Vector2(150, 150), 0, UnitTypes.Ranged);
            SpawnUnit(new Vector2(250, 150), 0, UnitTypes.Ranged);
            SpawnUnit(new Vector2(252, 150), 0, UnitTypes.Melee);
            SpawnUnit(new Vector2(290, 150), 0, UnitTypes.Melee);
            SpawnUnit(new Vector2(150, 170), 0, UnitTypes.Melee);
            SpawnUnit(new Vector2(250, 170), 0, UnitTypes.Melee);
            SpawnUnit(new Vector2(252, 170), 0, UnitTypes.Ranged);
            SpawnUnit(new Vector2(290, 170), 0, UnitTypes.Ranged);
            SpawnUnit(new Vector2(150, 190), 0, UnitTypes.Ranged);
            SpawnUnit(new Vector2(250, 190), 0, UnitTypes.Ranged);
            SpawnUnit(new Vector2(252, 190), 0, UnitTypes.Ranged);
            //SpawnUnit(new Vector2(290, 190), 0, UnitTypes.Ranged);
            //SpawnUnit(new Vector2(151, 650), 1, UnitTypes.Ranged);
            //SpawnUnit(new Vector2(251, 650), 1, UnitTypes.Ranged);
            //SpawnUnit(new Vector2(252, 650), 1, UnitTypes.Melee);
            //SpawnUnit(new Vector2(291, 650), 1, UnitTypes.Melee);
            //SpawnUnit(new Vector2(151, 670), 1, UnitTypes.Melee);
            //SpawnUnit(new Vector2(251, 670), 1, UnitTypes.Melee);
            //SpawnUnit(new Vector2(252, 670), 1, UnitTypes.Ranged);
            //SpawnUnit(new Vector2(291, 670), 1, UnitTypes.Ranged);
            //SpawnUnit(new Vector2(151, 690), 1, UnitTypes.Ranged);
            //SpawnUnit(new Vector2(250, 690), 1, UnitTypes.Ranged);
            //SpawnUnit(new Vector2(251, 690), 1, UnitTypes.Ranged);
            //SpawnUnit(new Vector2(291, 690), 1, UnitTypes.Ranged);
            //SpawnBase(10,3, 0);
            //SpawnBarracks(14, 6, 0);

            //SpawnBase(1, 1, 1);
            
        }

        public void SpawnUnit(Vector2 position, int faction, UnitTypes type)
        {
            Unit u = UnitFactory.SpawnUnit(this, type, position, faction, world);
            u.Enable();
            attackables.Add(u);
            controllers.Add(new UnitController(u, this));
        }

        public void SpawnUnit(Unit u, Vector2 position)
        {
            u.Enable();
            u.Position = position;
            //u.Nudge();
            attackables.Add(u);
            controllers.Add(new UnitController(u, this));
            
        }

        public bool SpawnPowerPlant(int gridX, int gridY, int faction) {
            if (grid.GetTile(gridX, gridY) is ResourceTile)
            {
                PowerPlant pp = new PowerPlant(this, gridX, gridY, faction, world, grid);
                attackables.Add(pp);
                return true;
            }
            return false;
        }

        public void GiveResources(int faction, int amount) {
            foreach (ArmyController ac in armyControllers)
                if (ac.Faction == faction)
                    ac.Resources += amount;
        }

        public void SpawnBase(int gridX, int gridY, int faction) {
            Base b = new Base(this, gridX, gridY, faction, world, grid);
            attackables.Add(b);
        }

        public void SpawnBarracks(int gridX, int gridY, int faction)
        {
            Barracks b = new Barracks(this, gridX, gridY, faction, world, grid);
            //TEST:
            b.QueueRangedUnit();
            b.QueueRangedUnit();
            b.QueueRangedUnit();
            b.QueueRangedUnit();
            b.QueueRangedUnit();
            b.QueueRangedUnit();
            b.QueueRangedUnit();
            b.QueueMeleeUnit();
            b.QueueMeleeUnit();
            b.QueueMeleeUnit();
            b.QueueMeleeUnit();
            b.QueueMeleeUnit();
            b.QueueMeleeUnit();
            attackables.Add(b);
        }


        public void Update(GameTime gameTime) {

            foreach(ArmyController ac in armyControllers)
                ac.Update(gameTime);

            window.Title = "Player 1: " + armyControllers[0].Resources;

            for (int i = 0; i < attackables.Count; i++){
                attackables[i].Update(gameTime);
                if (attackables[i].IsDead())
                {
                    if (attackables[i] is Unit)
                    {
                        controllers.Remove((attackables[i] as Unit).Controller);
                        (attackables[i] as Unit).Controller.Detach();
                        (attackables[i] as Unit).Controller = null;
                    }
                    foreach(ArmyController ac in armyControllers)
                        ac.Deselect(attackables[i]);
                    attackables[i].DestroyBody();
                    attackables.RemoveAt(i--);
                }
            } 
            foreach (UnitController c in controllers)
                c.Update(gameTime);
            vfx.Update(gameTime);

            //Update physics (variable time step but never less then 30 Hz):
            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, 1f / 30f));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            grid.Draw(spriteBatch);
            foreach (Attackable a in attackables)
                a.Draw(spriteBatch);
            if (drawDebug)
            {
                foreach (UnitController uc in controllers)
                {
                    if (uc.PathToFollow != null)
                        for (int i = 1; i < uc.PathToFollow.PointCount(); i++)
                            DebugDraw.DrawLine(spriteBatch, uc.PathToFollow.GetPoint(i - 1), uc.PathToFollow.GetPoint(i));
                }
            }
            vfx.Draw(spriteBatch);
            DebugDraw.DrawRectangle(spriteBatch, (armyControllers[0] as PlayerController).SelectionBox);
        }

        public IAttackable ClickSelect(Vector2 location, int faction) {
            IAttackable result = null;
            foreach (Attackable a in attackables) {
                if (a.Contains(location) && a.Faction == faction)
                {
                    result = a;
                    a.Select();
                }
                else if (a.Faction == faction) {
                    a.Deselect();
                }
            }
            return result;
        }

        public List<IAttackable> BoxSelect(Rectangle box, int faction)
        {
            List<IAttackable> selected = new List<IAttackable>();
            foreach (IAttackable a in attackables)
            {
                if (a.Faction == faction)
                {
                    if (box.Contains(a.GetSelectionPoint()))
                    {
                        a.Select();
                        selected.Add(a);
                    }
                    else
                    {
                        a.Deselect();
                    }
                }
            }
            return selected;
        }

        public Path GetPath(Vector2 a, Vector2 b, float goalTolerance = 0.0f)
        {
            return grid.FindPath(a, b, goalTolerance);
        }

        public bool LineOfSight(Attackable a, Attackable b) {
            return grid.LineOfSight(a.Position, b.Position);
        }

        public Attackable GetAttackableAtLocation(Vector2 location, int faction) {
            Attackable result = null;
            foreach (Attackable a in attackables)
            {
                if (a.Contains(location) && a.Faction != faction)
                {
                    result = a;
                }
            }
            return result;
        }

        public Attackable GetNearestTarget(Attackable u, float range = float.MaxValue) {
            float distance = float.MaxValue;
            Attackable closest = null;
            foreach (Attackable a in attackables) {
                if (a == u || a.Faction == u.Faction)
                    continue;
                float d = AttackableHelper.Distance(u,a);
                if (d < range && d < distance) {
                    closest = a;
                    distance = d;
                }
            }
            return closest;
        }

        public List<Attackable> GetAllFriendlyUnitsInRange(Unit u, float range) {
            List<Attackable> friendly = new List<Attackable>();
            foreach (Attackable a in attackables) {
                if (a != u && a.Faction == u.Faction && AttackableHelper.Distance(u, a) <= range)
                    friendly.Add(a);
            }
            return friendly;
        }

        public bool IsInSightRange(Attackable a, int faction)
        {

            foreach (Attackable att in attackables)
            {
                if (att.Faction == faction && AttackableHelper.Distance(att, a) <= att.VisionRange)
                {
                    return true;   
                }
            }
            return false;
        }

    }
}
