using MonoMod.Cil;
using RoR2;
using RoR2.Networking;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{
    internal class GameplayMisc
    {
        public static void Start()
        {
 
            //Helminth Roost should realistically be blocked from Stage 1 in WeeklyRun specifically
            //But both of these should be allowed for RandomStage order
            //At least 2 of my mods used it so i'm putting here.
            SceneDef scene = Addressables.LoadAssetAsync<SceneDef>(key: "RoR2/DLC2/habitat/habitat.asset").WaitForCompletion();
            scene.validForRandomSelection = true;
            scene = Addressables.LoadAssetAsync<SceneDef>(key: "RoR2/DLC2/helminthroost/helminthroost.asset").WaitForCompletion();
            scene.validForRandomSelection = true;

            //Halcyon Shrine drop table no longer split between NoSotS/YesSots just Any/YesSots which seems wrong.
            //Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "e291748f54c927a47ad44789d295c39f").WaitForCompletion().bannedItemTags = new ItemTag[] { ItemTag.HalcyoniteShrine };




            //Tanker Grease always leaves the puddle, regardless if it hit anything or not
            //This leads to it spawning in sky boxes which is ugly
            //This happens because ProjectileSimple.lifetime < ProjectileImpactExplosion.lifetime
            GameObject TankerAccelerantProjectile = Addressables.LoadAssetAsync<GameObject>(key: "e075b1933eaeb214180184c6d242f13a").WaitForCompletion();
            TankerAccelerantProjectile.GetComponent<ProjectileSimple>().lifetime = 3;

            //Remove Deployable from Beetle Guard
            GameObject.Destroy(Addressables.LoadAssetAsync<GameObject>(key: "5459e8ded89cd0f4d84219750a7e10ac").WaitForCompletion().GetComponent<Deployable>());


            On.RoR2.EclipseRun.HandlePostRunDestination += FixEclipseRunsKickingOutOfLobby;
        }

        private static void FixEclipseRunsKickingOutOfLobby(On.RoR2.EclipseRun.orig_HandlePostRunDestination orig, EclipseRun self)
        {
            if (NetworkServer.active)
            {
                NetworkManager.singleton.ServerChangeScene("lobby");
            }
        }



        //Fixed in AC
        /*public static void AllowGhostsToSuicideProperly(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.Before,
                x => x.MatchCall("RoR2.HealthComponent", "Suicide"));

            if (c.TryGotoPrev(MoveType.After,
                x => x.MatchLdarg(0)))
            {
                c.EmitDelegate<System.Func<HealthComponent, HealthComponent>>((stock) =>
                {
                    stock.health = 1;
                    return stock;
                });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed : HealthComponent_ServerFixedUpdateHealthComponent_Suicide1");
            }
        }*/




    }



}
