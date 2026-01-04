Bug fix pack that fixes confirmed larger gameplay bugs.\
occasional smaller gameplay stuff\
and various visual & audio issues.



Some fixes are Server Side, some are Client Side, some are Client Specific. 

Generally recommended for everyone.

Also technically is a library mod, this should have no impact on the end user.

# 
### Fixes the following :


Corrects or details various wrong item, survivor, drone, admin command descriptions.


## Item & EquipmentFixes
Warped Echo echoed hits no longer activating Planula & Repulsion Armor Plate. *(Like how it was specifically added to do that in prior versions)*\
Warped Echo echoed hits double-dipping some damage *increases* of the attacker. *(i.e. Watch, Lunar Ruin)*\
Warped Echo letting you live through OSP, even when OSP is meant to be disabled *(Curse, Low Health)*\
Warped Echo reducing damage further after OSP. *(OSP is meant to be last damage limit)*

Deus Ex Machina Blessing being consumed on attacks that cannot proc. *(0 damage or 0 procCoefficient attacks)*

Stone Flux Pauldron reducing speed twice. *(-200% rather than -100%)*
 
Hearty Stew never working on certain amounts of Eclipse curse. *(Due to a floating point discrepancy)*
 
Most AC debuffs activating Growth Nectar. *(Because debuffs are not filtered out automatically, and they forgot to tag them manually)*
 
 
Elusive Antler producing errors on things that move differently. (Stationary Turrets, RemoteOp Drones)\
Elusive Antler not working on Devoted Lemurians due to wrong authority check.

Bandolier not working on some drones & allies due to wrong authority check.
 
Charged Perferator rolling for crit instead of inheriting like other procs.

Unstable Transmitters effect being removed by cleansing effects. *(Accidentally tagged as a cooldown buff)*

Regenerative Scrap being scrappable


Executive Card no longer fails if you open Multi Shops *too quickly*.

For Retool, Enigma, Bottled Chaos, Functional Couple:
- Sawmerang not bleeding if you don't have the equipment.
- Milky Chrysalis flight will always have wings.


Removes Warbanner from Best Buddy's above level 99, to avoid spam and lag.

```
Some item tags were adjusted, to better match others.
- Equipment items unblacklisted for Scavs.
- Nkuhanas Opinion blacklisted from all monsters. (Undodgeable instant death, unpredictable activations)
- Kinetic Dampner blacklisted from all monsters. (Undodgeable instant death as melee)
- Egocentrism blacklisted for Mithrix.  (Too opressive against melee, like Mired Urn)
- Hunters Harpoon missing OnKill.
```
```
Item Visuals:

Faraday Spur visual radius being half of the attack radius.

Mercurial Rachis visual radius being slightly bigger than buff radius.

Deskplant visual being too small.
```
#
## Survivor & Drone Fixes
Jailer Drone just firing, instead of getting into a good position before Jailing.

Chef Boosted Sear Oil Puddles dealing 0% instead of 20% damage.   
 
Acrid now has spawn invulnerability.  

Captain Beacons being unable to crit. 

Drifter's Tinker being able to apply 3 debuffs that do literally nothing.
```
Player Visuals:

REXs flower vines disappearing quickly

Railgunners Scope being more opaque than in the past

"DroneBuff" icon not appearing for non-drone mechanical allies, despite those being buffed as well.\
"DroneBuff" icon not disappearing, if related item effects disappear.

Broken Flame & Missile Drone clip less into the ground.
```
#
## Monster Fixes
XI Construct Elites not spawning Elite Minions.\
XI Construct Ghosts not spawning Ghost Minions.\
XI Construct Exit Shield state not playing.

Lunar Golems having strange unneeded spawn restrictions.\
*(Often prevents them from spawning with Dissonance)*  

Child now immune to the sun. *(Like Parents)*  \
Twisted Scavs now immune to void implosions. *(Like other final bosses)*\
SPEX & Memory Probes now immune to void implosions.

Solus Heart softlocking the game if he has Dios.

 ```
Mob Visuals:

Solus Scorchers Grease no longer spawns midair or get stuck in the sky.\
Solus Extractor legs are now elite colored.  
Solus Distributors not casting shadows.

False Son using wrong Death Message since launch.

Gilded Elite corpses looking like blazing elite.
```



## Stage Fixes

Temporary Item Vendors being 10x more common than intended on SotS stages.

Distant Roost never spawning DLC3 interactables, with all 3 DLCs enabled.\
Scorches Acres never spawning DLC3 interactables, with all 3 DLCs enabled.\
Helminth Hatchery never spawning DLC1 interactables, with all 3 DLCs enabled.


Verdant Falls: No longer half as likely as other stage 1s.\ *(AC changed this for literally no reason)*
Distant Roost: No longer twice as likely. *(1.5x now, they are still variants)*\
Titanic Plains: No longer twice as likely. *^*  
 


Repurposed Crater not spawning the Access Node after Solus Wing. *(I was told this was a bug)*

Bazaar Seers being able to select Pre-Loop variants, Post-Loop.\
Bazaar Seers being able to select 2 Plains/2 Roosts.

Simu-Sanctuary & Simu-Commencement not able to spawn Lunar Pods. *(but still take credits)*

 
A Bulwarks Ambry using the wrong DLC1 spawn pool\
A Bulwarks Ambry spawning the wrong Alpha & XI Construct
 
Child & Grandparent not spawning during Parent family event.

## Gameplay Other Fixes
 
Newt Altars will no longer purchasable after Teleporter. (Like pre SotS)\
Newt Altars will not be purchasable if Teleporter starts with Blue Orb.

Drone Combiner no longer spawns with Devotion.\
Drone Shop can now spawn with Sacrifice.\
Limits Altar of Gold to 1 per stage.
 
Simulacrums Fog ramping up 25x slower than what is intended.\
Removed Chance Doll from Simulacrum, as there are no Shrine of Chance.\
*-(Unless SimulacrumAdditions is installed)*
  
## Artifact Fixes

Fixes Honor always choosing the same Elite types for some special bosses. *(Due to a rng bug)*

Fixes Vengence + Swarms spawning one of them as not an Umbra/Without any items.

Fixed 10 bugs or errors with Artifact of Devotion leading to :
- Lemurian evolution no longer results in all but 1 Lemurian having less items than intended.
- Lemurian wont randomly lose item effects such as Opal, Focus Crystal, etc.
- Lemurians gotten from Trials will be able evolve into elites.
- Lemurian evolution wont fail anymore if 1 or more Lemurians are in a quantum state.
- Lemurian inventories get properly deleted on all run ends.
- Devoted Lemurians now being tagged as such.
	- Different spawn sound
	- Works with Spare Drone Parts & Box of Dynamite. (Strange but intended synergy)
	- *They put the tag on Wild lemurians instead of the devoted ones*


 Artifact of Glass will now make you and minions appear as glass.

## Multiplayer / Client fixes
Prevents Halcyon Shrine entityState nullref on client.\
Removes the log spam from Halcyon Shrines on client.

Aurelionite Fragments & Potentials now spin for clients.\
Prayer Beads desyncing for clients, leading to various issues.\
Spider-Mines constantly beeping for clients

Simulacrum teleporting Clients into the void instead of the Focus.\
Simulacrum not pointing out enemies at the end of waves for Clients.

Drone Combiner having some visual bugs on clients.

## Visual
 
Adds various missing item displays :
- Elite Displays for XI Constructs, Mithrix.
- Perfected Elite Displays for all Monsters
- Fuel Array Display for Engineer Turrets.
 

Deep Void Signals Beam being flat.

Shrine of Shaping and Seed of Life not using the revive effect. (They attempt to spawn a unfinished object)

 
Lunar Exploder not having a subtitleToken.

Halcyon Shrine often not having its tip golden due to a redundant check.

Broken Flame & Missile Drones often spawning in the ground.

Aurelionite Geodes only appearing on Radar Scanner, *after* they are broken, instead of when unbroken.

## Audio
REX m1 not playing impact sound.

War Bonds should no longer play the Horn if 0 missiles would be fired.

Scavengers not playing their Spawn Sound

Soup not playing their Soup sound

## Misc
Logbook Fixes : 
- Eclipse not incrementing Wins / (Thus not unlocking logs)
- Devoted Lemurians being in log instead of wild.
 


Blue/Green/Access Portal messages will now open chat when they appear at the start of a stage.


 
Golden Diebacks Mushruum Fruit not dropping healing orbs or playing effects.


Chef Boost showing stacks of the buff, despite not functioning as such.

Item/Survivor description fixes can be turned off in case you play with rebalance mods.\
But should work fine even if you don't.


Treeborn Canopy & Helminth Hatchery being blacklisted from Random Stage Order.


## For Other Mods
Fixes up some unused content, should have no impact on regular gameplay.
 
Void Eradicators are less unfinished:
- Enables some particles
- Adds price transform
- Item models wont randomly be shrunk down a ton.
- Doesnt vanish when too close to camera
- Suppresed Scrap is tiered and usable, but kept hidden from log.
- Suppresed Items tab wider so item icons arent clipping
 
###

Report bugs to @Wolfo.wolfo in the Risk of Rain 2 discord or RoR2 Modding discord.\
If you're reporting a bug that isn't something obvious include the log file.





