using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Boomratkin
{
    [DefOf]
    public static class BRK_GeneDefOf
    {
        public static GeneDef BRKGene_ChemfuelGland;
        public static GeneDef BRKGene_FertilizerGland;
        public static GeneDef BRKGene_HEfuelGland;
        

        static BRK_GeneDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BRK_GeneDefOf));
        }

    }
    [DefOf]
    public static class BRK_HediffDefOf
    {
        public static HediffDef ChemfuelGeneration;


        static BRK_HediffDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BRK_HediffDefOf));
        }

    }

    [DefOf]
    public static class BRK_BodyPartDefOf
    {
        public static BodyPartDef BRK_FuelSack;

        static BRK_BodyPartDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BRK_BodyPartDefOf));
        }
    }

    [DefOf]
    public static class BRK_RaceDefOf
    {
        public static ThingDef Boomratkin;

        static BRK_RaceDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BRK_RaceDefOf));
        }
    }

    [DefOf]
    public static class BRK_BodyPartTagDefOf
    {
        public static BodyPartTagDef FuelGenerationSource;
        static BRK_BodyPartTagDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BRK_BodyPartTagDefOf));
        }
    }

    [DefOf]

    public static class BRK_ThoughtsDefOf
    {
        public static ThoughtDef FuelSackRemoved;

        static BRK_ThoughtsDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BRK_ThoughtsDefOf));
        }
    }
}
