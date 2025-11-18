using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;

namespace WolfoLibrary
{
    public static class ExtraActions
    {
        public static Action onMithrixPhase1;
        public static Action onVoidlingPhase1;
        public static Action onFalseSonPhase1;
        public static Action onSolusWing;
        public static Action onSolusHeart;
        public static Action<DirectorCardCategorySelection> onMonsterDCCS;

        public static void Start()
        {
            On.EntityStates.Missions.BrotherEncounter.Phase1.OnEnter += BrotherPhase1;
            On.RoR2.VoidRaidGauntletController.Start += VoidlingStart;
            On.EntityStates.MeridianEvent.Phase1.OnEnter += FalseSonPhase1;
            On.EntityStates.SolusWing2.Mission1Tunnel.OnExit += Mission1Tunnel_OnExit;
            On.EntityStates.SolusHeart.CutsceneTransformation.OnEnter += CutsceneTransformation_OnEnter;

            IL.RoR2.ClassicStageInfo.RebuildCards += MonsterDCCSGenerateHook;

        }

        private static void CutsceneTransformation_OnEnter(On.EntityStates.SolusHeart.CutsceneTransformation.orig_OnEnter orig, EntityStates.SolusHeart.CutsceneTransformation self)
        {
            orig(self);
            Action action = onSolusHeart;
            if (action == null)
            {
                return;
            }
            action();
        }

        private static void Mission1Tunnel_OnExit(On.EntityStates.SolusWing2.Mission1Tunnel.orig_OnExit orig, EntityStates.SolusWing2.Mission1Tunnel self)
        {
            orig(self);
            Action action = onSolusWing;
            if (action == null)
            {
                return;
            }
            action();
        }

        private static void VoidlingStart(On.RoR2.VoidRaidGauntletController.orig_Start orig, VoidRaidGauntletController self)
        {
            orig(self);
            Action action = onVoidlingPhase1;
            if (action == null)
            {
                return;
            }
            action();
        }

        public static void MonsterDCCSGenerateHook(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchStfld("RoR2.ClassicStageInfo", "modifiableMonsterCategories")))
            {
                c.EmitDelegate<Func<DirectorCardCategorySelection, DirectorCardCategorySelection>>((dccs) =>
                {
                    if (dccs && dccs is not FamilyDirectorCardCategorySelection)
                    {
                        //Kin & Dissonance & Honor all overwrite after this point
                        //So checking for them is not needed
                        Action<DirectorCardCategorySelection> action = onMonsterDCCS;
                        if (action != null)
                        {
                            action(dccs);
                        }
                    }
                    return dccs;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: SotV_EnemyRemovals");
            }
        }


        private static void FalseSonPhase1(On.EntityStates.MeridianEvent.Phase1.orig_OnEnter orig, EntityStates.MeridianEvent.Phase1 self)
        {
            orig(self);
            Action action = onFalseSonPhase1;
            if (action == null)
            {
                return;
            }
            action();
        }

        private static void BrotherPhase1(On.EntityStates.Missions.BrotherEncounter.Phase1.orig_OnEnter orig, EntityStates.Missions.BrotherEncounter.Phase1 self)
        {
            orig(self);
            Action action = onMithrixPhase1;
            if (action == null)
            {
                return;
            }
            action();
        }
    }

}
