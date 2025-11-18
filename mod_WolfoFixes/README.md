Bug fix pack that should be rather light on gameplay and includes some visual/audio fixes.

Some fixes are Server Side, some are Client Side, some are Client Specific. Generally recommended for everyone.

Also technically is a library mod.

#
### Fixes the following :



## Item Fixes
Corrects various wrong item descriptions.

Warped Echo blocking Eclipse 8 curse.\
Warped Echo ignoring Armor.\
Warped Echo ignoring OneShotProtection.\
Warped Echos first hit no longer putting you into danger.
Warped Echos first hit being ignored if OSP was triggered.\

Elusive Antler producing errors on things that move differently. (Stationary Turrets, RemoteOp Drones)\
Elusive Antler not working on Devoted Lemurians due to wrong authority check.

Bandolier not working on some drones due to wrong authority check.
 
Charged Perferator rolling for crit instead of inheriting like other procs.

Regenerative Scrap being scrappable

Some item tags were adjusted, to better match.
- Equipment items unblacklisted for Scavs.
- Nkuhanas Opinion blacklisted for all monsters.
- Egocentrism blacklisted for Mithrix.
- Hunters Harpoon missing OnKill.


## Survivor Fixes
Chef Boosted Sear Oil Puddles dealing 0% instead of 20% damage.   
 
Acrid now has spawn invulnerability.  

Captain Beacons being unable to crit. 
 
## Monster Fixes


XI Construct Elites not spawning Elite Minions.  
XI Construct Ghosts not spawning Ghost Minions.  
XI Construct Exit Shield state not playing.  
XI Construct Tail being incredibly disjointed on higher framerates.  
 
Lunar Golems having strange unneeded spawn restrictions. 
*(Often prevents them from spawning with Dissonance)*

Child now immune to the sun. *(Like Parents)*

Twisted Scavs now immune to Void Implosions. 
*(Like other final bosses)*

Hermit Crabs not being in the Dissonance spawn pool.

Solus Scorchers no longer Grease no longer spawns midair if shot into the sky.  
Solus Extractor legs are now elite colored.

## Equipment Fixes
Executive Card no longer fails if you open Multi Shops *too quickly*.

For Retool, Enigma, Bottled Chaos
- Sawmerang not bleeding if you don't have the equipment.
- Milky Chrysalis flight will always have wings.

## Misc Gameplay Fixes

Distant Roost & Titanic Plains are no longer twice as likely as other Stage 1s. (1.5x now)
Verdant Falls is no longer half as likely as other Stage 1s.

Lava Damage lowered to 2% for allies, when 10% seems intended for players only.
 
Bazaar Seers being able to select Pre-Loop variants, Post-Loop.
Bazaar Seers being able to select 2 Plains/2 Roosts.

Newt Altars will no longer purchasable after Teleporter. (Like pre SotS)\
Newt Altars will not be purchasable if Teleporter starts with Blue Orb.


Simulacrums Fog ramping up 25x slower than what is intended.

Removed Chance Doll from Simulacrum, as there are no Shrine of Chance. (Unless SimulacrumAdditions is installed)

 



Fixes Vengence + Swarms spawning one of them as not an Umbra/Without any items.

Fixed 10 bugs or errors with Artifact of Devotion leading to :
- Lemurian evolution no longer results in all but 1 Lemurian having less items than intended.
- Lemurian wont randomly lose item effects such as Opal, Focus Crystal, etc.
- Lemurians gotten from Trials will be able evolve into elites.
- Lemurian evolution wont fail anymore if 1 or more Lemurians are in a quantum state.
- Lemurian inventories get properly deleted on all run ends.
- Devoted Lemurians now being tagged as such.
- - Different spawn sound + Works with Spare Drone Parts. (Yeah that's intended)
- - They just put it on the normal Lemurians instead of the Devoted ones.



 

## Spawn Pool fixes

A Bulwarks ambry using the wrong dlc1 spawn pool
A Bulwarks ambry spawning the wrong Alpha Construct
A Bulwarks ambry spawning the wrong XI Construct

Limits Altar of Gold to 1 per stage.

Drone Combiner no longer spawns with Devotion.  
Drone Shop can now spawn with Sacrifice.

Child & Grandparent not spawning during Parent family event.

- Simu-Sanctuary & Simu-Commencement not being able to spawn Lunar Buds which would still take credits.

 
 
 
  

 


## Multiplayer / Client fixes
Prevents Halcyon Shrine entityState nullref on client.\
Removes the log spam from Halcyon Shrines on client.

Aurelionite Fragments & Potentials now spin for clients.\
Prayer Beads desyncing for clients, leading to various issues.\
Spider-Mines constantly beeping for clients

Simulacrum teleporting Clients into the void instead of the Focus.\
Simulacrum not pointing out enemies at the end of waves for Clients.


## Visual
Adds various missing item displays :
- Elite Displays for XI Constructs, Mithrix.
- Perfected Elite Displays for all Monsters
- Fuel Array Display for Engineer Turrets.
 
Fixes some monsters not slowly appearing during their spawn animation:
- Child, Parent
- Lesser, Greater, Lunar Wisp
- Void Reaver, Devestator, Voidling
- Engineer Turret, Walker
- Vagrant

Gilded Elite corpses no longer look like blazing elite corpses.

REXs flower vines disappearing quickly

Mercurias Rachis visual radius not matching the actual buff radius.

Deskplant Visual being too small.

Railgunners Scope being more opaque than in the past

Void Command Essence not having particles.\
Deep Void Signals Beam being flat.

Shrine of Shaping and Seed of Life not using the revive effect. (They attempt to spawn a unfinished object)

Artifact of Glass will now make you and minions appear as glass.

Stone Titan pinging the particles instead of the body.\
Mithrix Hammer/Nothing pinging instead of the body.
//Most mobs not having "Full Body Ping" enabled.

Lunar Exploder not having a subtitleToken.

Halcyon Shrine often not having its tip golden due to a redundant check.

Broken Flame & Missile Drones often spawning in the ground.

## Audio
REX m1 not playing impact sound.

War Bonds should no longer play the Horn if 0 missiles would be fired.

Scavengers not playing their Spawn Sound

Soup not playing their Soup sound

## Misc
Log Fixes : 
- Eclipse not incrementing Wins / (Thus not unlocking logs)
- Sulfur Pools Diorama having a red base.
- Void Locust Diorama, Void Devastator being too zoomed in.
- Nkuhanas Opinion being too zoomed out.
- Devoted Lemurians being in log instead of wild.
 

 
Green Orb message will now pop up as you load into the stage so you can actually see it.

 
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
 
## Commands
 
Testing Commands
- simu_softlock
- scanner | Radar Scanner equip + 100 BoostEquipRecharge
- godenemy | Sets enemy level absurdly high making them basically unkillable.
- goto_boss | Teleports you to Mithrix/FalseSon/Teleporter
- no_interactables | For clean stage screenshots.
- evolve_lemurian | Evolves all Devoted Lemurians.
- set_damage | Set damage.
- set_health | Set health and disable regen
- Various list_ commands



Report bugs to @Wolfo.wolfo in the Risk of Rain 2 discord or RoR2 Modding discord.\
If you're reporting a bug that isn't something obvious include the log file.





