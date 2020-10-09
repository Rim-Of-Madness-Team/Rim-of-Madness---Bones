using System;
using UnityEngine;
using Verse;

namespace BoneMod
{

    public class BoneModSettings : ModSettings
    {
        public static float boneFactor = 1;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref boneFactor, "boneFactor", 0);
        }

    }

    public class BoneModMod : Mod
    {
        BoneModSettings settings;

        public BoneModMod(ModContentPack con) : base(con)
        {
            settings = GetSettings<BoneModSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);
            listing.Label("Multiplier");
            BoneModSettings.boneFactor = listing.Slider(BoneModSettings.boneFactor, 0f, 10f);
            listing.Label("ROM_SettingsBoneMultiplier_Num".Translate(BoneModSettings.boneFactor));
            listing.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Rim of Madness - Bones";
        }
    }
}