using HarmonyLib;
using System.Collections.Generic;
using Verse;
using RimWorld;
using System.Reflection;

namespace BoneMod
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("rimworld.Sihv.bonemod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

                        harmony.Patch(
                            original: AccessTools.Method(type: typeof(Corpse), name: nameof(Corpse.SpecialDisplayStats)), 
                            prefix: null,
                            postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(SpecialDisplayStats_PostFix)));

                        harmony.Patch(
                            original: AccessTools.Method(type: typeof(Pawn), name: nameof(Pawn.ButcherProducts)), 
                            prefix: null,
                            postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(ButcherProducts_PostFix)));

            //Harmony.DEBUG = true;
        }

        static void SpecialDisplayStats_PostFix(Corpse __instance, ref IEnumerable<StatDrawEntry> __result)
        {
            // Create a modifyable list
            List<StatDrawEntry> NewList = new List<StatDrawEntry>();

            // copy vanilla entries into the new list
            foreach (StatDrawEntry entry in __result)
            {
                // it's possible to discard entries here if needed
                NewList.Add(entry);
            }

            // custom code to modify list contents
            StatDef BoneAmount = DefDatabase<StatDef>.GetNamed("BoneAmount", true);
            float pawnBoneCount = __instance.InnerPawn.GetStatValue(BoneAmount, true) * BoneModSettings.boneFactor;
            NewList.Add(new StatDrawEntry(BoneAmount.category, BoneAmount, pawnBoneCount, StatRequest.For(__instance.InnerPawn), ToStringNumberSense.Undefined));

            // convert list to IEnumerable to match the caller's expectations
            IEnumerable<StatDrawEntry> output = NewList;

            // make caller use the list
            __result = output;
        }

        static void ButcherProducts_PostFix(Pawn __instance, ref IEnumerable<Thing> __result, float efficiency)
        {
            int boneCount = GenMath.RoundRandom(__instance.GetStatValue(DefDatabase<StatDef>.GetNamed("BoneAmount", true), true) * BoneModSettings.boneFactor * efficiency);
            int meatCountCheck = GenMath.RoundRandom(__instance.GetStatValue(DefDatabase<StatDef>.GetNamed("MeatAmount", true), true));
            if (boneCount > 0)
            {

                List<Thing> NewList = new List<Thing>();
                foreach (Thing entry in __result)
                {
                    NewList.Add(entry);
                }
                if (meatCountCheck > 1)
                {
                    Thing bones = ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("BoneItem"), null);
                    bones.stackCount = boneCount;
                    NewList.Add(bones);
                }



                IEnumerable<Thing> output = NewList;
                __result = output;
            }
        }
    }
}
