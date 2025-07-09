using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;


namespace Boomratkin {
    [StaticConstructorOnStartup]
    public class GeneGizmo_ResourceFuel : GeneGizmo_Resource
    {
        public GeneGizmo_ResourceFuel(
            Gene_Resource gene,
            List<IGeneResourceDrain> drainGenes,
            Color barColor,
            Color barHighlightColor)
            : base(gene, drainGenes, barColor, barHighlightColor)
        {
        }
        protected override float ValuePercent
        {
            get
            {
                var gland = gene as BRKGene_GlandBase;
                return Mathf.Clamp01(gland.progress);
            }
        }

        protected override IEnumerable<float> GetBarThresholds()
        {
            yield break;
        }

        protected override string Title => gene is BRKGene_GlandBase gland ? gland.DisplayLabel : base.Title;

        protected override string BarLabel
        {
            get
            {
                int current = Mathf.FloorToInt(ValuePercent * gene.Max);
                return $"{current} / {(int)gene.Max}";
            }
        }

        protected override int Increments => Mathf.Max(1, (int)gene.Max);
        protected override FloatRange DragRange => new FloatRange(0, gene.Max);

        protected override bool DraggingBar { get; set; }
        protected override void DrawHeader(Rect headerRect, ref bool mouseOverElement)
        {
            if (IsDraggable && gene is BRKGene_GlandBase gland)
            {
                headerRect.xMax -= 24f;
                Rect iconRect = new Rect(headerRect.xMax, headerRect.y, 24f, 24f);
                Widgets.DefIcon(iconRect, ThingDefOf.Chemfuel);

                Rect chkRect = iconRect.ContractedBy(4f);
                GUI.DrawTexture(chkRect, gland.autoCollectAllowed ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);

                if (Widgets.ButtonInvisible(iconRect))
                {
                    gland.autoCollectAllowed = !gland.autoCollectAllowed;
                    (gland.autoCollectAllowed ? SoundDefOf.Tick_High : SoundDefOf.Tick_Low)
                        .PlayOneShotOnCamera();
                }

                if (Mouse.IsOver(iconRect))
                {
                    Widgets.DrawHighlight(iconRect);
                    mouseOverElement = true;
                    string onOff = gland.autoCollectAllowed ? "On".Translate() : "Off".Translate();
                    TooltipHandler.TipRegion(
                        iconRect,
                        () => "AutoCollectFuelDesc".Translate(onOff.Named("ONOFF")).Resolve(),
                        123456789);
                }
            }
            base.DrawHeader(headerRect, ref mouseOverElement);
        }

        protected override string GetTooltip()
        {
            var gland = gene as BRKGene_GlandBase;
            int displayed = Mathf.FloorToInt(ValuePercent * gene.Max);
            string tip =
                $"{gland.DisplayLabel}: {displayed} / {(int)gene.Max}\n" +
                $"Maintain above: {(int)Mathf.RoundToInt(gene.targetValue)}\n" +
                $"Auto-collect: {(gland.autoCollectAllowed ? "On".Translate() : "Off".Translate())}\n";
            return tip;
        }
    }
}