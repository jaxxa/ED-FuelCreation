using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using RimWorld;
using Verse;

namespace ED_FuelCreation
{
    public class Building_PowerCellStation : Building
    {
        bool m_Charging = true;
  
        CompPowerTrader m_Power;

        const int POWER_RATE = 1000;

        public override void SpawnSetup()
        {
            base.SpawnSetup();

            this.m_Power = base.GetComp<CompPowerTrader>();
        }

        public override void TickRare()
        {
            base.TickRare();

            if (this.m_Power.PowerOn)
            {
            //   this.TrySpawnReinforcedStuff();
            }

            this.UpdatePowerLevel();
        }

        private void TrySpawnReinforcedStuff()
        {

            //Thing _NewResource = ThingMaker.MakeThing(ThingDef.Named("PlasteelReinforced"));
            Thing _OldResourceThing = this.GetValidThingStack();
            if (_OldResourceThing == null) { return; }

            ThingDef _NewResourceDef = this.GetReinforcedVersion(_OldResourceThing);
            if (_NewResourceDef == null) { return; }

            _OldResourceThing.SplitOff(10);

            Thing _NewResource = ThingMaker.MakeThing(_NewResourceDef);
            GenPlace.TryPlaceThing(_NewResource, this.InteractionCell, ThingPlaceMode.Near);

        }

        private Thing GetValidThingStack()
        {
            List<IntVec3> _Cells = Enumerable.ToList<IntVec3>(Enumerable.Where<IntVec3>(GenAdj.CellsAdjacentCardinal((Thing)this), (Func<IntVec3, bool>)(c => GenGrid.InBounds(c))));

            List<Thing> _closeThings = new List<Thing>();

            foreach (IntVec3 _Cell in _Cells)
            {
                _closeThings.AddRange(GridsUtility.GetThingList(_Cell));
            }

            foreach (Thing _TempThing in _closeThings)
            {
                if (_TempThing.stackCount >= Building_MolecularReinforcementCompressor.STUFF_AMMOUNT_REQUIRED)
                {
                    ThingDef _ReinforcedVersion = this.GetReinforcedVersion(_TempThing);

                    if (_ReinforcedVersion != null)
                    {
                        return _TempThing;
                    }
                }
            }

            return null;
        }

        private void UpdatePowerLevel()
        {
            if (this.m_Charging)
            {
                this.m_Power.PowerOutput = Building_PowerCellStation.POWER_RATE;
            }
            else
            {
                this.m_Power.PowerOutput = -Building_PowerCellStation.POWER_RATE;
            }
        }

    }
}



//private List<Thing> FindValidStuffNearBuilding(Thing centerBuilding, int radius)
//{

//    //IEnumerable<Thing> _closeThings = GenRadial.RadialDistinctThingsAround(centerBuilding.Position, radius, true);

//    List<Thing> _closeThings = new List<Thing>();

//    List<IntVec3> _Cells = Enumerable.ToList<IntVec3>(Enumerable.Where<IntVec3>(GenAdj.CellsAdjacentCardinal((Thing)this), (Func<IntVec3, bool>)(c => GenGrid.InBounds(c))));

//    foreach (IntVec3 _Cell in _Cells)
//    {
//        _closeThings.AddRange(GridsUtility.GetThingList(_Cell));
//    }

//    List<Thing> _ValidCloseThings = new List<Thing>();

//    foreach (Thing _TempThing in _closeThings)
//    {
//        if (_TempThing.stackCount > Building_MolecularReinforcmentCompressor.STUFF_AMMOUNT_REQUIRED)
//        {
//            ThingDef _ReinforcedVersion = this.GetReinforcedVersion(_TempThing);

//            if (_ReinforcedVersion != null)
//            {
//                _ValidCloseThings.Add(_TempThing);
//                _TempThing.stackCount -= Building_MolecularReinforcmentCompressor.STUFF_AMMOUNT_REQUIRED;
//            }
//        }
//        //if (tempThing.def.category == ThingCategory.Item)
//        //{
//        //    validCloseThings.Add(tempThing);
//        //}
//    }
//    return _ValidCloseThings;
//}