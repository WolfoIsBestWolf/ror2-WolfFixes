```
v1.3.4
Language folder just decided to not be uploaded last time???

v1.3.3
Fixed IL Incompat with other mods affected Lunar Ruin. (ie RiskyMod)

v1.3.2
Fixed Distant Roost never spawning DLC3 interactables, with all 3 dlcs enabled.\
Fixed Scorches Acres never spawning DLC3 interactables, with all 3 dlcs enabled.\
Fixed Helminth Hatchery never spawning DLC1 interactables, with all 3 dlcs enabled.
-(Mod already fixed these before but properly saying this, as fixed)

Improved a lot of admin command descriptions lacking numbers.
Added Orphan Core respawning to it's description.


Fixed Aurielionte Geodes showing up scanners after being broken instead of before.
Fixed Sky Meadows rocks not marking themselves as the killer.
Fixed IL incompatibility with RestoredRaincoat.
Removed a lot of commands for eventual Debug Tookit incorperation
Config to remove just gameplay fixes but keep visuals.
```
```
v1.3.1
Fixed Hearty Stew randomly not working depending on your curse amount.
Fixed Solus Heart softlocking if he has Dios.

Fixed mod config getting added wrongly to RiskOfOptions.
Fixed Warbanner spam on Orphaned Core past level 99. (Not relevant for vanilla, requested)
```
```
v1.3.0
Fixed Warped Echo echo hits no longer activating Planula
Fixed Warped Echo echo hits no longer activating Repulsion Armor Plate
Fixed Warped Echo echo hits double dipping some damage increases (Enemy Watches, Lunar Ruin)
Fixed Warped Echo being able to let you live through OSP, even when OSP is meant to be disabled. *(Configuerable)
Fixed DeusEXMachina buff being consumed on attacks that can't proc. (0 damage/0 proc attacks)

```
```
v1.2.5
!Fixed Temp Shops being x10 more common on SotS maps than what is intended. (Config to turn off if so desired)
!Fixed Operator Transport Drone Command creating infinite errors.

Fixed MulT switching equipment highlighting them, every time.
Fixed Unstable Transmitter effect being removed by cleansing effects.
Fixed Box of Dynamite not granting the lil arrow to Mechanicals.

```
```
v1.2.4
Removed various fixes as they were fixed in AC 1.4.1
-Noted below
-All the Warped Echo bugs
--It works with Armor, Danger, E8 Curse and OSP as it should again

SPEX immune to Void now to not get kicked out by Lost Seers.

Fixed Honor elite rng not being random on some special bosses.
Fixed most new debuffs triggering Growth Nectar.
Fixed Drifter being able to apply Debuffs that do literally nothing.
```
```
v1.2.3
Fixed Stone Flux Pauldron reducing speed twice.
Fixed mod making RegenScrap unrebirthable & not give a random temp with Duplicator
Fixed Warped Echo allowing you to live through OSP, even if OSP is meant to be disabled by curse.
```
```
v1.2.2
Needed WolfoLib update, WFix unchanged.

Fixed language folder not loading.

v1.2.1
Forgot to package important stuff
```
## v1.2.0

The developers are aware of these issues, things just slip through the cracks or kinda broke randomly close to when release builds needed to be finalized.

They be working on it.

### DLC3 Fixes:
#### Gameplay:
- AC making Verdant Falls half as likely as a Stage 1
- *(Fixed in 1.4.1) Voidsent Flame not chain-proccing more Voidsent Flames on other full health enemies.
- Bandolier not working on new drones/devoted lemurians.  *(Wrong auth check)*
- Jailer Drones not assuming position before firing. (Late into dev got messed up)
- Access Node on Repurposed Crater not spawning if Solus Wing was defeated prior.
- Drone Combiner spawning with Devotion.
- Drone Shop not spawning with Sacrifice.
- Solus Wing weakpoints not being immune to VoidDeath.
- *(Fixed in 1.4.1) Iron Aurura not having Solus Amalgamator in the spawnpool.
	- *Is a Loop Boss on Iron Alluvium, but thats replace by the night variant, which they forgot to add it to*

#### Visual:

- Faraday Spur visual radius being half of the attack radius.
- *(Fixed in 1.4.1) Tier2 Drones using the wrong color. (Blazing instead of Magenta)
- "BuffedDrone" arrow not appearing for Turrets/Mechanical allies, despite those being affected as well.
- "BuffedDrone" arrow not disappearing when items disappear.
- *(Fixed in 1.4.1) Collective Elite Buff missing sprite.
- *(Changed in 1.4.1) Collective Elite bubbles being less visible from inside on specifically PC.
- Elite Extractors not having elite colored legs.
- Scorcher Grease will no longer get stuch in the skybox.
- Solus Distributors not casting shadows.
- *(Not a bug?) Temp Distributers having 1 misaligned item.
- *(Fixed in 1.4.1)"Vehicle Seats", such as Drifter showing up on Scanners as ?
- *(Fixed in 1.4.1) Most creatures not using full body pings.
- *(Fixed in 1.4.1) Scrapper Quantity showing "Temp+Perm" instead of just Permament
- Drone Combiner lacking some visuals/outputting errors in log for Clients

 
#### General fixes

False Son Death quote being the wrong ones since launch SotS.

XI Shield Exit not animation not playing since launch SotV.\
XI Ghosts not spawning Ghost Alphas\
XI Elites not spawning Elite Alphas\
(Fixed in 1.4.1) XI's lower half rotating wildly since Devotion

Twisted Scavs immune to Void Implosions *(Like other final bosses)*

-Removed strange white effect from Void Portals. (Introduced in SotS)\
-Fixed Celestial Orb overlapping with Void Orb.\
-Broken Missile Drone often clipping into the ground.
 
-Grandparent & Child now spawn during Parent family event.

All portal messages will now open chat.\
Removed Family event announcements from Dissonance, because they're overwritten.


Dissolved SpawnPoolFixer mod.
- SotV enemy removals now in LittleGameplayTweaks
- Higher Radar Scanner chance now in WQoL


AiBlacklisted Egocentrism from Mithrix;
- *To match other opressive Anti-Melee items being banned*.

Evolution-Blacklisted Kinetic Dampner;
- *Because damage reflection items can make the game unplayable*.

UnBlacklisted Equipment Items from Scavs:
- *Holdover from old versions*.

 
```

v1.1.17
Fixed log spam when using Sawmerang
Fixed Ice Spear fix not actually going through due to wrong config val.
Fixed Broken Flame Drones often clipping into the ground, sometimes being barley visible.

v1.1.16
Fixed an issue with my custom droptables and disabling DLCs.

v1.1.15
Fixed various objects on Helminth not being pingable due to missing Null Check.
Fixed mod unintentionally disabling Empathy Cores, Voidsent, Singularity and Ego for Phase 4 Mithrix.
Fixed evolving quantum state lemurians not actually working.

v1.1.14
Reverted Lunar Ruin fix because it made Ruin from lightning not get removed by geodes.
Warped Echo OSP fix now gives you 0.1s of invul like regular OSP.

v1.1.13
Fixed Lunar Ruin counting as 2 debuffs.
Fixed Elusive Antler not working on Devoted Lemurians (Wrong Auth Check)
Fixed Elusive Antler producing errors instead of working on Stationary Turrets (Missing NullCheck)
Tagged Unstable Transmitter as Healing (Barrier Items are all tagged as Healing)


v1.1.12
Fixed WEcho always bypassing OSP
Fixed Bazaar being able to select 2 Plains or 2 Roosts
Fixed a mod nullRef with HealthComponentToTransform

v1.1.11
Fixed missing orig() making only Eclipse count as wins.

v1.1.10
Fixed an issue with the way I added some bodyFlags

v1.1.9
Fixed Warped Echo ignoring Armor entirely.
Fixed Warped Echo not putting you into danger
Fixed Milky Chrysalis not doing it's flight animation due to mod.
Removed my Grandparent Rock fix that was too small and just added someone elses.
(This is technically fixed in vanilla, the rock is just 60x too small)


v1.1.8 : Part of mod was no longer running on accident
v1.1.7
Fixed Boosted Sear Oil Pools dealing 0% damage instead of 20%
Fixed Simulacrums Fog damage being far lower due to unrelated Void Fields balance changes and it being left forgotten.
Removed Chance Doll from Simulacrum as there are no Shrine of Chance. (If SimulacrumAdditions not installed)
Fixed Treeborn Canopy and Helminth Hatchery being blacklisted from RandomStageOrder
Fixed NegativeRegen not triggering OnDeath effects. (Happiest Mask would never trigger Ice/Mending OnDeath)


v1.1.6
Fixed REXs flower vine effect disappearing too soon.
-Should fix like 3-4 other effects but less noticible.
Fixed Grandparent mini rock being invisible.
Fixed Charging Father Laser spawning a Red line at 111,8,68
Fixed Halcyon Shrine often not having its tip golden.
Double checking for certain skin fixes.

 
v1.1.5
Fixed Boosted Cleaver proc to also be 1.5 

v1.1.4
Phase 4 fix is now off by default due to feedback.
Moved Gilded Corpse blazing fix here
Devoted Lemurian Log fix actually moved here.
Fixed Credit Card failing if you buy multi shops too quickly. (Because hopoo gave it a 0.1 cooldown for no reason)
Fixed Captain Beacons never critting
Fixed Eclipse not incrementing the Win counter.
Fixed some equipment displays added by mod not being present with SS2. 
Fixed Boosted Oil Spill description being gone (for english anyways)
Fixed Chronic Expansion on corpses constantly nullreffing
Fixed REX Syringe impact sound being gone. (Unexplainable)
Fixed some more item descriptions
Fixed Nkuhanas Opinion being too zoomed out
Fixed Ice Spear knocking enemies around due to being on wrong Phys Layer.
Restored Glass Mithrix skin


v1.1.3
Fixed the following no longer slowly appearing in their spawn animation, leading to it looking really ugly in some cases.
-Child
-Parent
-Vagrant
-Lesser Wisp
-Greater wisp
-Lunar Wisp
-Void Reaver
-void Devestator
-Voidling
-Engineer Turret
-Engineer Walking Turret
-XI (technically...)(his body is too broken to use it properly)

Fixed Charged Perferator rolling Crit instead of inheriting.

Added Helminth Tunnel fix as dependency
Added Gilded Chunk lag fix as dependency

v1.1.2
Added some config to bugs people are attached to.
Config will be in game If RiskOfOptions is installed.
Fixed Alpha Constructs from Defense Nucleus not using their unique skin
Fixed XI Construct laser not exploding at end as miscFixes removed it.


v1.1.1
Fixed latest patch breaking multiplayer with non mod owners due to Suppresed Scrap implementation.
Moved fixed Diebacks Mushroom fruit not being implemented correctly

v1.1.0
Fixed Warped Echo negating Eclipse 8 curse entirely.
Fixed Sawmerang projectiles not inflicting Bleed without the equipment.
Milky Chryslis being active will always have wings, regardless if you change or lose the equipment.
Fixed Path of Colossus stage skip.
Fixed Railgunner scope transperency being applied twice to one scope.

Moved from VanillaArtifactPlus :
- Artifact of Glass not making you glass.
- Twisted Elites not giving players and some enemies armor.
- Swarms + Vengence spawning 1 Umbra + 1 Enemy survivor
- 11 Devotion bug fixes.
Devoted lemurians now actually tagged as such.
- Means they spawn with a different sound
- - The check for that is also bugged, so that's another fix.
- Means they work with Spare Drone Parts
- - This is just intended.

Moved from SimulacrumAdditions :
Void Suppressor fixes
- Scrap being untiered
- Disappearing too close at camera
- Hud being too small
- Item being randomly too small



v1.0.1
Fixed Broken Scrappers due to a saftey check going missing in the transition.
Mod will now be where I do ItemTags and EliteDisplays

v1.0.0 
Split from WQoL
Moved some Simu fixes from SimuAdds
Children immune to the Sun
```
```

(TODO)Prestige not spawning a teleporter on Conduit Canyon
(TODO)Teleproter music after solus haunt fucked up
(TODO)Brass no longer intterupt itself
(TODO)All Habitat fruits fucking exploding on stage load

```