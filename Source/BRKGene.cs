using RimWorld;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boomratkin;
using UnityEngine;

namespace Boomratkin
{

    public abstract class BRKGene_GlandBase : Gene_Resource, IGeneResourceDrain
    {
        public abstract float generateSpeed { get; }
        public abstract Thing fuelThing { get; }
        public abstract HediffDef hediff { get; }

        public Gene_Resource Resource => this;
        public bool CanOffset => true;
        public Pawn Pawn => pawn;
        public float ResourceLossPerDay => -1 * generateSpeed;
        public abstract string DisplayLabel { get; }

        // Auto-collect toggle
        public bool autoCollectAllowed = true;
        public float progress = 0f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref autoCollectAllowed, "autoCollectAllowed", true);
            Scribe_Values.Look(ref progress, "fuelProgress", 0f);
        }

        public override void Tick()
        {
            if (pawn.IsHashIntervalTick(600))
            {
                BodyPartRecord fuelSack = BRKframe.GetFuelSack(Pawn.health.hediffSet);
                if (fuelSack == null)
                {
                    progress = 0;
                    return;
                }
                var malnut = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition);
                if (malnut == null)
                {
                    progress = Mathf.Clamp01(progress + generateSpeed);
                    BRKframe.AdjustOrAddHediff(pawn, hediff, progress);
                }

                if (autoCollectAllowed && progress >= 1f)
                {
                    int spawnCount = Mathf.CeilToInt(Max - targetValue);
                    if (spawnCount > 0)
                    {
                        var thing = fuelThing;
                        thing.stackCount = spawnCount;
                        if (GenPlace.TryPlaceThing(thing, pawn.Position, pawn.Map, ThingPlaceMode.Direct))
                            progress = targetValue / Max; 
                    }
                }
            }
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            if (!Active) yield break;
            foreach (var g in base.GetGizmos())
                yield return g;
            foreach (var g in GeneResourceDrainUtility.GetResourceDrainGizmos(this))
                yield return g;
        }

        public override void PostAdd()
        {
            base.PostAdd();
            Reset();
        }
    }

    public class BRKGene_ChemfuelGland : BRKGene_GlandBase
    {
        public override float generateSpeed => 0.03f;
        public override Thing fuelThing => ThingMaker.MakeThing(ThingDefOf.Chemfuel);
        public override HediffDef hediff => BRK_HediffDefOf.ChemfuelGeneration;
        protected override Color BarColor => new ColorInt(220, 120, 50).ToColor;
        protected override Color BarHighlightColor => new ColorInt(255, 153, 51).ToColor;
        public override float InitialResourceMax => 140f;
        public override float MinLevelForAlert => 0f;
        public override string DisplayLabel =>  "ChemFuel".Translate();
    }



    public class BRKGene_HEfuelGland : BRKGene_GlandBase
    {
        public override float generateSpeed => 0.02f;
        public override Thing fuelThing => ThingMaker.MakeThing(ThingDefOf.Chemfuel);
        public override HediffDef hediff => BRK_HediffDefOf.ChemfuelGeneration;
        protected override Color BarColor => new ColorInt(230, 50, 0).ToColor;
        protected override Color BarHighlightColor => new ColorInt(255, 60, 0).ToColor;
        public override float InitialResourceMax => 140f;
        public override float MinLevelForAlert => 0f;
        public override string DisplayLabel => "High Energy Fuel".Translate();
    }

    public class BRKGene_FertilizerGland : BRKGene_GlandBase
    {
        public override float generateSpeed => 0.02f;
        public override Thing fuelThing => ThingMaker.MakeThing(ThingDefOf.Chemfuel);
        public override HediffDef hediff => BRK_HediffDefOf.ChemfuelGeneration;
        protected override Color BarColor => new ColorInt(133, 152, 184).ToColor;
        protected override Color BarHighlightColor => new ColorInt(184, 193, 210).ToColor;
        public override float InitialResourceMax => 140f;
        public override float MinLevelForAlert => 0f;
        public override string DisplayLabel => "Fertilizer".Translate();
    }
}
