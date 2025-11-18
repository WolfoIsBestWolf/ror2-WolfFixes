using BepInEx;
using HG;
using MonoMod.Cil;
using R2API.Utils;
using RoR2;
using RoR2.ExpansionManagement;
using RoR2.Navigation;
using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using WolfoLibrary;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

namespace SpawnPoolFixer
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("Wolfo.DLCSpawnPoolFixer", "DLCSpawnPoolFixer", "1.3.4")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class SotsSpawnPoolFix : BaseUnityPlugin
    {
         
        public void Awake()
        {
            WConfig.InitConfig();
             

           
          

             
         
            //All NodeGraphs with 0 NoCeiling Ground spots

          
           

       

            


           

     

        }


       




     

 
    }
}