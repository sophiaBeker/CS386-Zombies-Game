2D Zombie Shooter

A 2D zombie-shooter game built in Unity and C#. The player navigates multiple arenas, aims with the mouse, and shoots incoming zombies while managing a limited health pool. The project focuses on gameplay systems, enemy behavior, UI, game-state management, audio, and progressive difficulty.

Features:
  Player movement: Move the character using the arrow keys.
  Mouse aiming and shooting: Aim with the mouse and shoot projectiles toward the cursor.
  Zombie enemies: Zombies spawn randomly around the arena and immediately move toward the player.
  Reusable prefabs: Zombie and bullet objects are created using Unity prefabs.
  Multiple arenas: Progress through several arenas with increasing difficulty.
  Health system: The player starts with 5 health and loses health when harmed by zombies.
  Kill tracking: The HUD displays the player's current number of zombies defeated.
  Health bar: The HUD displays the player's remaining health.
  Win condition: Defeat the required number of zombies.
  Lose condition: The player loses when health reaches zero.
  Menus: Includes start, pause, and restart functionality.
  Audio: Includes background music and gameplay sound effects.

Built With:
  Unity
  C#
  Unity Physics / collision systems
  Unity Prefabs
  Unity UI
  Unity audio systems

Controls:
  Input
  Action
  Arrow Keys
  Move the player
  Mouse
  Aim
  Mouse button
  Shoot

Gameplay:
  The objective is to survive the zombie attack and progress through the arenas.
  Start the game from the main menu.
  Use the arrow keys to move around the arena.
  Use the mouse to aim and shoot.
  Defeat zombies while avoiding contact with them.
  Monitor the health bar and zombie kill counter.
  Defeat the required number of zombies to complete an arena.
  Progress through arenas as the difficulty increases.
  If health reaches zero, the game ends.
  Use the pause menu to pause gameplay or restart the game as needed.

Core Gameplay Systems
  Player Controller
      The player controller handles movement through keyboard input and aiming/shooting through mouse input.
  Projectile System
      Bullets are created from a reusable prefab and used as projectiles against zombie enemies.
  Zombie Spawning
      Zombie prefabs are spawned at randomized positions around the arena. Once spawned, zombies immediately begin moving toward the player.
  Health and Win/Lose States
      The player begins with 5 health. The game tracks health and zombie kills to determine whether the player loses or completes the current objective.
  Difficulty Progression
      The game contains several arenas with increasing difficulty, providing progression as the player advances.
  UI and Menus

The HUD communicates important gameplay information, including:
  Current health
  Number of zombies defeated

The project also includes:
  Start menu
  Pause menu
  Restart functionality
  Audio
  The game includes background music and sound effects to provide audio feedback during gameplay.

How to Run:
  Clone or download this repository.
  Open the project in Unity Hub using the Unity version associated with the project.
  Open the main game scene.
  Press Play in the Unity Editor.
  Use the arrow keys and mouse to play.


This project provided hands-on experience with:
  Building gameplay systems in C# and Unity.
  Working with Unity prefabs to create reusable game objects.
  Handling keyboard and mouse input.
  Implementing enemy spawning and pursuit behavior.
  Managing collisions and gameplay interactions.
  Designing health, scoring, and win/lose game-state logic.
  Building HUD elements and menu systems.
  Implementing audio and music.
  Designing progression across multiple arenas.
  Breaking a game into separate systems that can be developed and debugged independently.

Potential Future Improvements:
  Additional zombie types with different movement speeds or behaviors.
  More weapons and projectile types.
  Additional difficulty levels and arena layouts.
  A persistent high-score system.
  More visual effects and animation.

Expanded audio effects and music.

Additional player abilities or power-ups.
