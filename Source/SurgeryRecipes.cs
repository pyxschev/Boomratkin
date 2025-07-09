//using System;
//using System.Collections.Generic;
//using System.Linq;
//using RimWorld;
//using UnityEngine;
//using Verse; 

//namespace Boomratkin
//{

//    public class Recipe_RemoveFuelSack : Recipe_Surgery
//    {
//        public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
//        {
//            Pawn pawn = thing as Pawn;
//            if (pawn == null || !pawn.Spawned || pawn.Dead) return false;
//            if (!pawn.IsMoeLotl()) return false;
//            if (GetTail(pawn) == null || !GetBodyPartIsMaxHP(pawn, this.GetTail(pawn))) return false;
//            return base.AvailableOnNow(thing, part);
//        }

//        public override AcceptanceReport AvailableReport(Thing thing, BodyPartRecord part = null)
//        {
//            Pawn pawn = thing as Pawn;
//            if (!pawn.IsMoeLotl())
//            {
//                return "Recipe_CutOffTail_Report_PawnNotAxolotl".Translate(pawn);
//            }
//            if (pawn == null || !pawn.Spawned || pawn.Dead)
//            {
//                return "Recipe_CutOffTail_Report_PawnCanDoThis".Translate(pawn);
//            }
//            if (pawn.DevelopmentalStage.Baby() || pawn.DevelopmentalStage.Child())
//            {
//                return "Recipe_CutOffTail_Report_TooSmall".Translate();
//            }
//            return base.AvailableReport(thing, part);
//        }

//        public override bool CompletableEver(Pawn surgeryTarget)
//        {
//            return base.CompletableEver(surgeryTarget) && this.GetTail(surgeryTarget) != null ? this.GetBodyPartIsMaxHP(surgeryTarget, this.GetTail(surgeryTarget)) : false;
//        }

//        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
//        {
//            if (!pawn.IsMoeLotl())
//            {
//                return;
//            }
//            if (this.GetTail(pawn) == null)
//            {
//                Messages.Message("Recipe_CutOffTail_Message_NotHaveTail".Translate(pawn), pawn, MessageTypeDefOf.NeutralEvent, true);
//                return;
//            }
//            if (!this.GetBodyPartIsMaxHP(pawn, this.GetTail(pawn)))
//            {
//                Messages.Message("Recipe_CutOffTail_Message_NotHaveMaxHPTail".Translate(pawn), pawn, MessageTypeDefOf.NeutralEvent, true);
//                return;
//            }

//            //成功手术效果
//            this.OnSurgerySuccess(pawn, part, billDoer, ingredients, bill);

//            //掉派系关系
//            if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayer))
//            {
//                base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -5, AxolotlHistoryEventDefOf.Axolotl_Surgery_CutMoeLotlTail);
//            }

//            //心情
//            this.GiveThoughtsForPawnTailCut(pawn, billDoer);
//        }

//        protected override void OnSurgerySuccess(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
//        {
//            //尾巴切除
//            pawn.health.AddHediff(HediffDefOf.MissingBodyPart, this.GetTail(pawn));

//            //飙血
//            pawn.DropBloodFilth(3);

//            //掉落尾巴物品
//            if (!GenPlace.TryPlaceThing(ThingMaker.MakeThing(AxolotlThingDefOf.Axolotl_DropTail, null), pawn.PositionHeld, pawn.MapHeld, ThingPlaceMode.Near, null, null, default(Rot4)))
//            {
//                Debug.Warning("无法生成尾巴掉落物在 " + pawn.ToString() + " 边上");
//            }
//        }

//        private void GiveThoughtsForPawnTailCut(Pawn pawn, Pawn billDoer)
//        {
//            //被切除者的心情
//            if (pawn.needs.mood != null)
//            {
//                pawn.needs.mood.thoughts.memories.TryGainMemory(AxolotlThoughtDefOf.MyMoeLotlTailBeCutOff, null, null);
//            }

//            //切除者的心情
//            //非人化无效
//            if (billDoer.needs.mood != null && !billDoer.health.hediffSet.HasHediff(HediffDefOf.Inhumanized))
//            {
//                Pawn_StoryTracker story = billDoer.story;
//                if (((story != null) ? story.traits : null) != null)
//                {
//                    //心理变态 和 蝾螈食人者 不受这个效果
//                    if (!(billDoer.story.traits.HasTrait(TraitDefOf.Psychopath) || (billDoer.IsMoeLotl() && billDoer.story.traits.HasTrait(AxolotlTraitDefOf.Cannibal))))
//                    {
//                        if (billDoer.story.traits.HasTrait(TraitDefOf.Bloodlust))
//                        {
//                            billDoer.needs.mood.thoughts.memories.TryGainMemory(AxolotlThoughtDefOf.DoctorCutOffMoeLotlTail_Bloodlust, null, null);
//                        }
//                        else
//                        {
//                            billDoer.needs.mood.thoughts.memories.TryGainMemory(AxolotlThoughtDefOf.DoctorCutOffMoeLotlTail, null, null);
//                        }
//                    }
//                }
//            }

//            //其他蝾螈殖民者心情
//            foreach (Pawn otherPawn in billDoer.Map.mapPawns.AllHumanlikeSpawned)
//            {
//                if (otherPawn.Faction == Faction.OfPlayer)
//                {
//                    if (otherPawn.IsMoeLotl())
//                    {
//                        //自己不添加 食人萌螈不添加
//                        if (otherPawn == pawn || otherPawn.story.traits.HasTrait(AxolotlTraitDefOf.Cannibal)) continue;
//                        if (pawn.IsColonist || pawn.HostFaction == Faction.OfPlayer)
//                        {
//                            otherPawn.needs.mood.thoughts.memories.TryGainMemory(AxolotlThoughtDefOf.KnowColonistMoeLotlTailBeCutOff);
//                        }
//                        else
//                        {
//                            otherPawn.needs.mood.thoughts.memories.TryGainMemory(AxolotlThoughtDefOf.KnowGuestMoeLotlTailBeCutOff);
//                        }

//                    }
//                }
//            }

//            return;
//            /*
//            if (pawn.IsColonist)
//            {
//                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.HarvestedOrganFromColonist, billDoer.Named(HistoryEventArgsNames.Doer)), true);
//                return;
//            }
//            if (pawn.HostFaction == Faction.OfPlayer)
//            {
//                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.HarvestedOrganFromGuest, billDoer.Named(HistoryEventArgsNames.Doer)), true);
//            }
//            */
//        }

//        private BodyPartRecord GetTail(Pawn pawn)
//        {
//            foreach (BodyPartRecord bodyPartRecord in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
//            {
//                if (bodyPartRecord.def.tags.Contains(AxolotlBodyPartTagDefOf.Axolotl_Tail))
//                {
//                    return bodyPartRecord;
//                }
//            }
//            return null;
//        }

//        private bool GetBodyPartIsMaxHP(Pawn pawn, BodyPartRecord bodyPart)
//        {
//            float partHP = pawn.health.hediffSet.GetPartHealth(bodyPart);
//            float partMaxHP = AxolotlBodyPartDefOf.Axolotl_Tail.GetMaxHealth(pawn);
//            bool flag_isTailHasMaxHP = partHP >= partMaxHP;
//            return flag_isTailHasMaxHP;
//        }
//    }


//    //public override void ApplyThoughts(Pawn pawn, Pawn billDoer)
//    //{
//    //    if (pawn.needs.mood != null)
//    //    {
//    //        pawn.needs.mood.thoughts.memories.TryGainMemory(BRK_ThoughtsDefOf.FuelSackRemoved);
//    //    }
//    //}
//}

//}
