# Quirks

What the Odin port keeps from the VB.NET original on purpose, and the few places it differs. Read this before "fixing" anything odd, and add new oddities here.

## The original's behaviour, kept

- **Attack and defend are numbers of dice, and a die only counts when it shows a six** (the original rolls `1d6/6`). The total is then capped at the maximum shown on the Status screen, so a N00b's attack is 0 or 1 and fights drag on. Status shows the maximums, not the dice.
- **Three of the four town gates are one-way.** The north, west and east gates lead to the forest's centre, and the centre only has one way back into town (the south gate, "In").
- **The Machine Guardians are in the ruins from the start**, 15 of them, as well as the one the lever calls up in the machine room. They are the same fierce creature (10 health, 20 attack dice).
- **Running away can fail for want of an exit**: the game picks any of north, east, south or west (or any other exit the room has); if there is no exit that way you "fail to run away" and the enemies hit back. And a successful "runs away!" can still leave you in place if the way you picked needs a key.
- **Holy Water does nothing** but moisten your enemy. It is for sale for 3 Jools anyway.
- **Dead monsters drop everything they carry**; piles of the same stackable thing on the ground merge.
- **A broken weapon or armour is removed** when it breaks; it does not go back to your bag.
- The innkeeper sells the Sewer Key once (for five Cellar Rat Tails).
- The Graveyard Key is carried by the King Rat in the sewer, the Tower Key lies in one grave, the UFO Key is in the tower's top-floor chest, the Machine Key is in a metal box in the UFO.

## Where the port differs

- **Saving**: one save in the browser's local storage, written after every turn and when you choose "Save Game...". "Load Game..." reads it. "Continue" is there on the main menu when a game is waiting. A finished or abandoned game leaves no save. Messages waiting to be read are not saved (the game is saved between turns).
- **No Quit** in the main menu.
- **Picking something up** shows "X takes Y(xN)." The original printed that line and cleared the screen straight away, so nobody saw it.
- **Equipment** lists its slots in a fixed order (weapon, shield, torso, head); the original listed them in the order you equipped them. Items in the bag are listed oldest first.
- The title screen is a styled heading rather than a figlet banner.
- Menus have numbers and the Escape key picks "Never Mind".
- The random numbers come from a small generator seeded from the clock (or `?seed=N` on the page, for testing).
