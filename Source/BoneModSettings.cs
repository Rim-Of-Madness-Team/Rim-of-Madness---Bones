using System;
using UnityEngine;
using Verse;
using SettingsHelper;

namespace BoneMod
{

    public class BoneModSettings : ModSettings
    {
        public static float boneFactor = 1;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref BoneModSettings.boneFactor, "boneFactor", 0);
        }

    }

    public class BoneModMod : Mod
    {
        public static BoneModSettings settings;

        public BoneModMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<BoneModSettings>();
        }

        public override string SettingsCategory() => "Bone Mod Settings";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var label = "";

            if (BoneModSettings.boneFactor < 0.1f)
            {
                label = "ROM_SettingsBoneMultiplier_None".Translate();
            }
            else
            {
                label = "ROM_SettingsBoneMultiplier_Num".Translate(BoneModSettings.boneFactor);
            }

            BoneModSettings.boneFactor = Widgets.HorizontalSlider(inRect.TopHalf().TopHalf().TopHalf(), BoneModSettings.boneFactor, 0.0f, 3f, false, label, "0", "3", 0.1f);

            this.WriteSettings();
        }
    }

}