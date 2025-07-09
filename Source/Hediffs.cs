using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Boomratkin
{
    public class HediffCompProperties_ExplodeOnDeath : HediffCompProperties
    {
        public float explodeRadius;
        public int explodeDamage;
        public int explodePenetration;
        public float fireChance;

        public HediffCompProperties_ExplodeOnDeath()
        {
            compClass = typeof(HediffComp_ExplodeOnDeath);
        }
    }

    public class HediffComp_ExplodeOnDeath : HediffComp
    {
        public HediffCompProperties_ExplodeOnDeath Props => (HediffCompProperties_ExplodeOnDeath)props;

        public override void Notify_PawnKilled()
        {
            
            BodyPartRecord fuelSack = BRKframe.GetFuelSack(Pawn.health.hediffSet);
            if (fuelSack == null)
            {
                return;
            }
            Pawn.TakeDamage(new DamageInfo(DamageDefOf.Bomb, 99999f, 999f, -1f, null, fuelSack));
            GenExplosion.DoExplosion(base.Pawn.Position, base.Pawn.Map, Props.explodeRadius, DamageDefOf.Bomb, null, Props.explodeDamage, Props.explodePenetration, chanceToStartFire: Props.fireChance);
            base.Notify_PawnKilled();
        }

        
    }
}
