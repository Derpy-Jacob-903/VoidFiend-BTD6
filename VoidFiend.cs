using MelonLoader;
using BTD_Mod_Helper;
using VoidFiend;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Data.Gameplay.Mods;
using Il2CppAssets.Scripts.Unity.Display;
using UnityEngine;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppAssets.Scripts.Unity.Achievements.List;
using System;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;

[assembly: MelonInfo(typeof(VoidFiend.VoidFiendMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace VoidFiend;

public class VoidFiendMod : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<VoidFiendMod>("「V??oid Fiend』 loaded!");

        float maxCorruption = 100.0f;
        //float minimumCorruptionPerVoidItem = 2.0f; // No Items
        float corruptionPerSecondInCombat = 3.0f; //gain 3 corruption per second
        //float corruptionPerSecondOutOfCombat = 3.0f;
        float corruptionForFullDamage = 50.0f; //50% of damage in added to corruption
        float corruptionForFullHeal = -100.0f; //100% of healing in subtracted from corruption
        float corruptionPerCrit = 0.02f; //used for every hit
        float corruptionDeltaThresholdToAnimate = 1.0f; //idk
        //ModBuffIcon corruptedBuffDef = new();
        float corruptionFractionPerSecondWhileCorrupted = -0.06666667f; // lose 1/15 corruption per second.

        float corruption = 0;
    }
}

public class VoidFiend : ModHero
{
    public override string BaseTower => TowerType.SniperMonkey + "-110";

    public override int Cost => 600;

    public override string DisplayName => "[V??oid Fiend]";
    public override string Title => "[Co??rupted Am?nesiac]";
    public override string Level1Description => "At full Corruption, transform your abilities into more aggressive forms.\n[D??row?n]: Fire a slowing long-range beam for 300% damage per shot. Can target anywhere on the screen.\n[Cor??rupted D??row?n]: Fire a constant short-range beam for 2000% damage per second.";
    public override bool Use2DModel => true;
    public override string Description =>
        "The Void Fiend is a corrupted hero that fluctuates between a controlled and corrupted form, each with different strengths and weaknesses. Managing this curse has become its fate.";


    public override string NameStyle => TowerType.Ezili; // Yellow colored
    public override string BackgroundStyle => TowerType.Ezili; // Yellow colored
    public override string GlowStyle => TowerType.Ezili; // Yellow colored


    public override int MaxLevel => 20;
    public override float XpRatio => 1.425f;

    [System.Obsolete]
    public override int Abilities => 2;

    public override string Portrait => "VoidSurvivorBody";
    public override string Icon => "VoidSurvivorBody";
    public override string Button => "VoidSurvivorBody";
    public override string Square => "VoidSurvivorBody";

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        towerModel.range = 30;
        var p = towerModel.GetWeapon().projectile;
        p.GetDamageModel().damage = 3; //Fire a slowing long-range beam for 300% damage.
        var r = towerModel.GetWeapon().Rate = 1.66666667f; //Fires at a rate of 1.6(repeating) or 5/3 times per second by default.
        towerModel.GetWeapon().rate = r;
        towerModel.GetWeapon().rateFrames = (int)(60 * r);
        towerModel.GetWeapon().projectile.RemoveBehavior<DamageModifierForTagModel>();
        towerModel.GetWeapon().projectile.AddBehavior(new SlowModel("SlowModel_Slow50", 0.5f, 3, "Slow50", 999999, "", true, false, null, false, false, false));

        //towerModel.ApplyDisplay<EeveeDisplay>();
    }

    public static void UpdateDamage(TowerModel towerModel, int Level)
    {
        //var d =  = 3;
        towerModel.GetWeapon().projectile.GetDamageModel().damage = (float)Math.Round(2.4f + 0.6f * Level);
        //if (!(towerModel.GetAbilities()[1] == null)) { }
        if (Level >= 3) 
        {
            Game.instance.model.GetTowerFromId(TowerType.BombShooter).GetWeapon().projectile.GetDescendant<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = (float)Math.Round(11f + 2.2f * Level);
            Game.instance.model.GetTowerFromId(TowerType.BombShooter).GetWeapon().projectile.GetDescendant<CreateProjectileOnContactModel>().projectile.pierce = (float)Math.Round(11f + 2.2f * (Level - 3));
        } //has 

        return;
    }

    public class Level2 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.";
        public override int Level => 2;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level3 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "[Floo?d]: Fire an explosive plasma ball, dealing 1100% damage. Disables [D??row?n] for 5 seconds.\n [Cor??rupted Floo?d]: Fire an explosive plasma ball, dealing 1100% damage.";
        public override int Level => 3;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
            //var p = Game.instance.model.GetTowerFromId(TowerType.SentryParagon).GetDescendant<CreateProjectileOnExpireModel>().projectile.Duplicate();
            //p.GetDamageModel().damage = 11 * (float)Math.Round(1f + 0.2f * Level);
            //var pp = Game.instance.model.GetTowerFromId(TowerType.BombShooter+"-300").GetWeapon().projectile.Duplicate();
            
            //pp.GetDescendant<CreateProjectileOnContactModel>().projectile = p;
            var ability = new AbilityModel("AbilityModel_Flood", "[Floo?d]", "Charge an explosive plasma ball, dealing 1100% damage.", 0, 0, GetSpriteReference("texVoidSurvivorSkillIcons_1"), 15, new Il2CppReferenceArray<Model>(0), false, false, "Level3", 0.0f, 0, -1, false, false);
            ability.addedViaUpgrade = Id;
            //var bi = new DelayedShutoffModel("DelayedShutoffModel_Flood", 0, 1, null);
            //ability.AddBehavior(bi);
            ability.AddBehavior(new ActivateAttackModel("ActivateAttackModel_Flood", 5f, true, new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<AttackModel>(1), false, false, false, false, false));
            var fuck = ability.GetDescendant<ActivateAttackModel>().attacks[0] = Game.instance.model.GetTowerFromId("BombShooter-300").Duplicate().GetAttackModel();
            fuck.weapons[0].projectile.display = new Il2CppNinjaKiwi.Common.ResourceUtils.PrefabReference("2c9b8167585010744932c3f1a46715ff");
            fuck.weapons[0].projectile.GetDescendant<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = (float)Math.Round(11f + 2.2f * Level);
            fuck.weapons[0].projectile.GetDescendant<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.Purple;
            //fuck.weapons[0].projectile = pp;
            fuck.range = 30;
            fuck.weapons[0].rate = fuck.weapons[0].Rate = 10; fuck.weapons[0].rateFrames = 300;
            towerModel.AddBehavior(ability);
        }
    }
    public class Level4 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "[Floo?d]'s pierce is also improved each level.";
        public override int Level => 4;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level5 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "Darkness shrouding darkness - echoes from futures past and past futures and places and times eeking out from between cracks in reality.";
        public override int Level => 5;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level6 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "A suffocating prison of the mind and soul, occupied by beasts poking and prodding at your spirit forever.";
        public override int Level => 6;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level7 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "[Sup??ress]: Crush 25% Corruption to gain 1 life.\n[Cor??rupted Sup??ress]: Crush 1 life to gain 25% Corruption. Costs more lives the more times it is used without exiting corrupted form.";
        public override int Level => 7;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
            var a = new AbilityModel("AbilityModel_Suppress", "[Sup??ress]", "Crush 25% Corruption to gain 1 life.", 0, 0, GetSpriteReference("texVoidSurvivorSkillIcons_3"), 15, new Il2CppReferenceArray<Model>(0), false, false, "Level3", 0.0f, 0, -1, false, false);
            towerModel.AddBehavior(a);
        }
    }
    public class Level8 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "Forever testing you. Forever ?T?E?MPTING? you, iteration after iteration.";
        public override int Level => 8;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level9 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "They've run laps through the brain, ?CRAWLE?D? every last synapse for memories and data: weapons to use against theoretical armies with more resilient hearts and evasive bodies.";
        public override int Level => 9;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level10 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "[?Tr?espass]: Disappear into the Void, cleansing all debuffs. When Trespassing...";
        public override int Level => 10;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
            var a = new AbilityModel("AbilityModel_Trespassing", "[Sup??ress]", "Disappear into the Void, cleansing all debuffs.", 0, 0, GetSpriteReference("texVoidSurvivorSkillIcons_2"), 15, new Il2CppReferenceArray<Model>(0), false, false, "Level3", 0.0f, 0, -1, false, false);
            towerModel.AddBehavior(a);
        }
    }
    public class Level11 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "?CH?TTERIN?G? and clacking they can be heard watching from beyond the cell, no longer interested but still patrolling the grounds.";
        public override int Level => 11;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level12 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "Here in this purgatory there is no hunger and no thirst - all that remains are the ?ECH?OES??.";
        public override int Level => 12;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level13 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "...";
        public override int Level => 13;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level14 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "?H?OW L??ONG?? has it been?";
        public override int Level => 14;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level15 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "Time seems to warp in and out around breaths circulating ???W?THIN???.";
        public override int Level => 15;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level16 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "Darkness all-consuming starts to [?D??ROW?N?] alveoli and [?FLOO?D?] inky black veins.";
        public override int Level => 16;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level17 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "Memories [??TR?ESPASSING?] into reality as reality fades into the distant past.";
        public override int Level => 17;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level18 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "???SILENC?E?? broken only by the sound of watching eyes.";
        public override int Level => 18;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level19 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "????SH?AKE THE ?BARS????.";
        public override int Level => 19;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level20 : ModHeroLevel<VoidFiend>
    {
        public override string Description => "Twisting and contorting, engulfed in ??F?EAR??? and becoming the embodiment.  Reborn like some sick prophecy, arms and legs like new corpses on the seabed.  No longer [?SUP??RESSE?D?]";
        public override int Level => 20;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
}

public class EeveeDisplay : ModDisplay
{
    public override string BaseDisplay => Generic2dDisplay;
    public override float Scale => 1f;
    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        {
            NodeLoader.NodeLoader.LoadNode(node, "VoidSurvivorModel", mod);
        }
    }
}

public class VoidFiendCorrupted : ModHero
{
    public override string BaseTower => TowerType.SuperMonkey;

    public override int Cost => 2000;

    public override string DisplayName => "[Co??rupted V??oid Fiend]";
    public override string Title => "[Co??rupted Am?nesiac]";
    public override string Level1Description => "At full Corruption, transform your abilities into more aggressive forms.\nWhen in Controlled form: Fire a slowing long-range beam for 300% damage.\nWhen in Corrupted form: Fire a constant short-range beam for 2000% damage per second.";
    //public override bool Use2DModel => false;
    public override string Description =>
        "The Void Fiend is a corrupted hero that fluctuates between a controlled and corrupted form, each with different strengths and weaknesses. Managing this curse has become its fate.";


    public override string NameStyle => TowerType.Ezili; // Yellow colored
    public override string BackgroundStyle => TowerType.Ezili; // Yellow colored
    public override string GlowStyle => TowerType.Ezili; // Yellow colored


    public override string Portrait => "VoidSurvivorBody";
    public override string Icon => "VoidSurvivorBody";
    public override string Button => "VoidSurvivorBody";
    public override string Square => "VoidSurvivorBody";


    public override int MaxLevel => 20;
    public override float XpRatio => 1.71f;

    [System.Obsolete]
    public override int Abilities => 0;

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        towerModel.range = 30;
        var r = towerModel.GetWeapon().Rate = 1 / 8;
        towerModel.GetWeapon().rate = r;
        towerModel.GetWeapon().rateFrames = (int)(60 * r);
        var p = towerModel.GetWeapon().projectile;
        p.GetDamageModel().damage = 2.5f;
        towerModel.GetWeapon().projectile.pierce = 999;
        towerModel.GetWeapon().projectile.maxPierce = 999;
        towerModel.GetWeapon().projectile.radius = 3;
        var t = towerModel.GetWeapon().projectile.GetBehavior<TravelStraitModel>();
        t = new TravelStraitModel("TravelStraitModel", 30, 1);
        //var cock = towerModel.GetDescendant<LineProjectileEmissionModel>();
        //cock.projectileInitialHitModel = cock.projectileAtEndModel;
        //cock.projectileInitialHitModel..GetBehavior<DamageModel>().damage = 2.5f;

        towerModel.ApplyDisplay<EeveeDisplay>();
    }
    public static void UpdateDamage(TowerModel towerModel, int Level)
    {
        //var d =  = 3;
        towerModel.GetWeapon().projectile.GetDamageModel().damage = (float)Math.Round(2.0 + 0.5f * Level);
        //if (!(towerModel.GetAbilities()[1] == null)) { }

        return;
    }
    public class Level2 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 2;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level3 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 3;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
            //towerModel.AddBehavior();
        }
    }
    public class Level4 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 4;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level5 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 5;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level6 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 6;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level7 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 7;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level8 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 8;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level9 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 9;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level10 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 10;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level11 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 11;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level12 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 12;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level13 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 13;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level14 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 14;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level15 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 15;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level16 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 16;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level17 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 17;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level18 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 18;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level19 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 19;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
    public class Level20 : ModHeroLevel<VoidFiendCorrupted>
    {
        public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n[Floo?d]'s pierce is also improved each level.";
        public override int Level => 20;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            UpdateDamage(towerModel, this.Level);
        }
    }
}
