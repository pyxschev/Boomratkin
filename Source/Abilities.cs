//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using RimWorld;
//using Verse;

//namespace Boomratkin
//{
//    public class CompAbilityEffect_FuelCost : CompAbilityEffect
//    {
//        public new CompProperties_AbilityFuelCost Props => (CompProperties_AbilityFuelCost)props;

//        private bool HasEnoughFuel
//        {
//            get
//            {
//                BRKGene_GlandBase gene_Fuel = parent.pawn.genes?.GetFirstGeneOfType<BRKGene_GlandBase>();
//                if (gene_Fuel == null || gene_Fuel.Value < Props.hemogenCost)
//                {
//                    return false;
//                }

//                return true;
//            }
//        }

//        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
//        {
//            base.Apply(target, dest);
//            GeneUtility.OffsetFuel(parent.pawn, 0f - Props.hemogenCost);
//        }

//        public override bool GizmoDisabled(out string reason)
//        {
//            Gene_Hemogen gene_Hemogen = parent.pawn.genes?.GetFirstGeneOfType<Gene_Hemogen>();
//            if (gene_Hemogen == null)
//            {
//                reason = "AbilityDisabledNoFuelGene".Translate(parent.pawn);
//                return true;
//            }

//            if (gene_Hemogen.Value < Props.fuelCost)
//            {
//                reason = "AbilityDisabledNoFuel".Translate(parent.pawn);
//                return true;
//            }

//            float num = TotalFuelCostOfQueuedAbilities();
//            float num2 = Props.fuelCost + num;
//            if (Props.fuelCost > float.Epsilon && num2 > gene_Hemogen.Value)
//            {
//                reason = "AbilityDisabledNoHemogen".Translate(parent.pawn);
//                return true;
//            }

//            reason = null;
//            return false;
//        }

//        public override bool AICanTargetNow(LocalTargetInfo target)
//        {
//            return HasEnoughFuel;
//        }

//        private float TotalHemogenCostOfQueuedAbilities()
//        {
//            float num = ((!(parent.pawn.jobs?.curJob?.verbToUse is Verb_CastAbility verb_CastAbility)) ? 0f : (verb_CastAbility.ability?.HemogenCost() ?? 0f));
//            if (parent.pawn.jobs != null)
//            {
//                for (int i = 0; i < parent.pawn.jobs.jobQueue.Count; i++)
//                {
//                    if (parent.pawn.jobs.jobQueue[i].job.verbToUse is Verb_CastAbility verb_CastAbility2)
//                    {
//                        num += verb_CastAbility2.ability?.FuelCost() ?? 0f;
//                    }
//                }
//            }

//            return num;
//        }
//    }
//}
