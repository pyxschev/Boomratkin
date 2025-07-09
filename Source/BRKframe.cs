using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Boomratkin
{
    public class BRKframe
    {
        public static void AdjustOrAddHediff(Pawn pawn, HediffDef hediffDef, float severity = -1f, int overrideDisappearTicks = -1, BodyPartRecord part = null, DamageInfo? dinfo = null, DamageWorker.DamageResult result = null)
        {
            Hediff hediff = pawn?.health.GetOrAddHediff(hediffDef, part, dinfo, result);
            if (hediff == null)
            {
                return;
            }
            if (severity > 0f)
            {
                hediff.Severity = severity;
            }
            if (overrideDisappearTicks > 0)
            {
                HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
                if (hediffComp_Disappears != null)
                {
                    hediffComp_Disappears.ticksToDisappear = overrideDisappearTicks;
                }
            }
        }

        public static BodyPartRecord GetFuelSack(HediffSet hediffSet)
        {
            foreach (BodyPartRecord notMissingPart in hediffSet.GetNotMissingParts())
            {
                if (notMissingPart.def.tags.Contains(BRK_BodyPartTagDefOf.FuelGenerationSource))
                {
                    return notMissingPart;
                }
            }

            return null;
        }
    }
}
