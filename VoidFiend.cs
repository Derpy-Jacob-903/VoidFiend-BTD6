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

[assembly: MelonInfo(typeof(VoidFiend.VoidFiendMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace VoidFiend;

public class VoidFiendMod : BloonsTD6Mod
{
    float maxCorruption = 100.0f;
    float minimumCorruptionPerVoidItem = 2.0f;
    float corruptionPerSecondInCombat = 3.0f;
    float corruptionPerSecondOutOfCombat = 3.0f;
    float corruptionForFullDamage = 50.0f;
    float corruptionForFullHeal = -100.0f;
    float corruptionPerCrit = 2.0f;
    float corruptionDeltaThresholdToAnimate = 1.0f;
    //ModBuffIcon corruptedBuffDef = new();
    float corruptionFractionPerSecondWhileCorrupted = -0.06666667f;
    public override void OnApplicationStart()
    {
        ModHelper.Msg<VoidFiendMod>("「V??oid Fiend』 loaded!");

    }
}

public class VoidFiendTower : ModHero
{
    public override string BaseTower => TowerType.SniperMonkey + "-110";

    public override int Cost => 1250;

    public override string DisplayName => "Void Fiend";
    public override string Title => "『Co??rupted Am?nesiac】";
    public override string Level1Description => "At full Corruption, transform your abilities into more aggressive forms.\n【D??row?n」: Fire a slowing long-range beam for 3 layers per shot. Can target anywhere on the screen.\n【D??row?n」(Corrupted): Fire a constant short-range beam for 20 layers per second.";
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


    public override int MaxLevel => 1;
    public override float XpRatio => 1.425f;

    [System.Obsolete]
    public override int Abilities => 0;

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        towerModel.range = 30;
        var p = towerModel.GetWeapon().projectile;
        p.GetDamageModel().damage = 3; //Fire a slowing long-range beam for 300% damage.
        var r = towerModel.GetWeapon().Rate = 1.66666667f; //Fires at a rate of 1.6(repeating) or 5/3 times per second by default.
        towerModel.GetWeapon().rate = r;
        towerModel.GetWeapon().rateFrames = (int)(60 * r);
        towerModel.GetWeapon().projectile.RemoveBehavior<DamageModifierForTagModel>();
        towerModel.GetWeapon().projectile.AddBehavior(new SlowModel("SlowModel_Slow50", 0.66666667f, 3, "Slow50", 999999, "", true, false, null, false, false, false)); //Inflicts 50% Slow

    }
}

public class EeveeDisplay : ModDisplay
{
    public override string BaseDisplay => Generic2dDisplay;
    public override float Scale => 1f;
    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        //if (Main.Use2DDisplay)
        //{
            //Set2DTexture(node, "EeveeBaseDisplay");
            //node.transform.GetChild(0).transform.localScale = 0.2f * Vector3.one;
        //}
        //else
        {
            NodeLoader.NodeLoader.LoadNode(node, "mdlVoidSurvivor", mod);
        }
    }
}
public class Level2 : ModHeroLevel<VoidFiendTower>
{
    public override string Description => "Each level gives slightly improved damage for all attacks and abilities.\n『Floo?d」's pierce is also improved each level.";
    public override int Level => 2;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        
    }
}
public class VoidFiendCorrupted : ModHero
{
    public override string BaseTower => TowerType.SuperMonkey;

    public override int Cost => 2000;

    public override string DisplayName => "Corupted Void Fiend";
    public override string Title => "『Co??rupted Am?nesiac】";
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


    public override int MaxLevel => 1;
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
    }
}

