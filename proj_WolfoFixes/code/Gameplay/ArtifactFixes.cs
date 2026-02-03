using EntityStates.Fauna;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoFixes
{

    internal class ArtifactFixes
    {
 
        public static void Start()
        {
            On.RoR2.Artifacts.DoppelgangerInvasionManager.CreateDoppelganger += Vengence_NoUmbraForRemoteOpPlayers;
            On.RoR2.MasterCatalog.FindAiMasterIndexForBody += Vengence_BreakingWithRemoteOp;
    
            On.EntityStates.SolusHeart.Death.MissionCompleted.FixedUpdate += Evol_SolusHeartSoftlockWithDios;
            On.DroneTechController.OnDestroy += MetaMorph_StopDronesFollowingOperatorIfOperatorGone;

            //Fix Honor always choosing the same elites for some bosses
            On.RoR2.Artifacts.EliteOnlyArtifactManager.PromoteIfHonor += EliteOnlyArtifactManager_PromoteIfHonor;


            //One guy would spawn with 0 items.
            IL.RoR2.Artifacts.SwarmsArtifactManager.OnSpawnCardOnSpawnedServerGlobal += SwarmsVengenceGooboFix;
        }

        private static void SwarmsVengenceGooboFix(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdfld("RoR2.SpawnCard/SpawnResult", "spawnRequest"),
                x => x.MatchCallvirt("RoR2.DirectorCore", "TrySpawnObject")))
            {
                c.EmitDelegate<Func<SpawnCard.SpawnResult, SpawnCard.SpawnResult>>((result) =>
                {
                    if (result.spawnedInstance)
                    {
                        if (result.spawnRequest.spawnCard is MasterCopySpawnCard)
                        {
                            result.spawnRequest.spawnCard = MasterCopySpawnCard.FromMaster(result.spawnedInstance.GetComponent<CharacterMaster>(), true, true, null);
                        }
                    }
                    return result;
                });
            }
            else
            {
                WolfFixes.log.LogError("IL Failed: SwarmsArtifactManager_OnSpawnCardOnSpawnedServerGlobal");
            }
        }


        private static void EliteOnlyArtifactManager_PromoteIfHonor(On.RoR2.Artifacts.EliteOnlyArtifactManager.orig_PromoteIfHonor orig, CharacterMaster instanceMaster, Xoroshiro128Plus rng, EliteDef[] eliteDefs)
        {
            orig(instanceMaster, Run.instance.bossRewardRng, eliteDefs);
        }

        private static void Evol_SolusHeartSoftlockWithDios(On.EntityStates.SolusHeart.Death.MissionCompleted.orig_FixedUpdate orig, EntityStates.SolusHeart.Death.MissionCompleted self)
        {
            for (int i = 0; i < self.combatSquad.readOnlyMembersList.Count; i++)
            {
                CharacterMaster characterMaster = self.combatSquad.readOnlyMembersList[i];
                if (characterMaster)
                {
                    CharacterBody body = characterMaster.GetBody();
                    if (body)
                    {
                        characterMaster.TrueKill();
                        //body.healthComponent.Suicide(null, null, default(DamageTypeCombo));
                    }
                }
            }
            orig(self);
        }
 
        private static void MetaMorph_StopDronesFollowingOperatorIfOperatorGone(On.DroneTechController.orig_OnDestroy orig, DroneTechController self)
        {
            orig(self);
            self.followActive = false;
            foreach (DroneInfo droneInfo2 in self.AllDrones)
            {
                Debug.Log(droneInfo2.droneDef);
                if (droneInfo2.commandReceiver)
                {
                    droneInfo2.commandReceiver.CommandFollow(false);
                    droneInfo2.bodyStateMachine.SetNextStateToMain();
                }
            }
        }

        

        private static void Vengence_NoUmbraForRemoteOpPlayers(On.RoR2.Artifacts.DoppelgangerInvasionManager.orig_CreateDoppelganger orig, CharacterMaster srcCharacterMaster, Xoroshiro128Plus rng)
        {
            if (srcCharacterMaster && srcCharacterMaster.GetInRemoteOp())
            {
                return;
            }
            orig(srcCharacterMaster,rng);
        }

        private static MasterCatalog.MasterIndex Vengence_BreakingWithRemoteOp(On.RoR2.MasterCatalog.orig_FindAiMasterIndexForBody orig, BodyIndex bodyIndex)
        {
            DroneIndex drone = DroneCatalog.GetDroneIndexFromBodyIndex(bodyIndex);
            if (drone != DroneIndex.None)
            {
                BodyIndex regularDrone = DroneCatalog.GetBodyIndexFromDroneIndex(drone);
                if (regularDrone != BodyIndex.None)
                {
                    return orig(regularDrone);
                }
            }
            return orig(bodyIndex);
        }

     
    }
 
}
