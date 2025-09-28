# 3D On-Rails Shooter - Project "Virtua-l Cop"

## 1. Overview

This project contains the complete C# code and architecture for a 3D on-rails shooter game inspired by classics like *Virtua Cop*. The codebase is designed to be modular, scalable, and lightweight, targeting the Android platform using Unity.

All core systems are implemented, including player controls, on-rails movement, diverse enemy AI, a flexible weapon system, special abilities, boss battles, UI management, and a save system. This document provides the necessary instructions to assemble these components into a fully playable game within the Unity Editor.

## 2. Folder Structure

The project is organized into the following structure:

-   `/Scripts`: Contains all C# scripts.
    -   `/AI`: Enemy, Boss, and Civilian AI logic.
    -   `/Debug`: In-game developer console and hitbox visualizer.
    -   `/GameManagers`: High-level managers for the game state, menus, and settings.
    -   `/Player`: Player shooting and on-rails movement scripts.
    -   `/Systems`: Core systems like spawning, achievements, and saving.
    -   `/UI`: UI management scripts.
    -   `/Weapons`: The base weapon class and all specific weapon implementations.
-   `/Scenes`: Contains scene files. The text blueprints for the levels are here.
-   `/Prefabs`: Where you should save your created prefabs (Enemies, Weapons, etc.).
-   `/Resources`: For any assets you want to load dynamically.

## 3. Setup Instructions

To get the game running, you need to assemble the scenes and prefabs in the Unity Editor. Follow these steps carefully.

### Step 3.1: Create Core Prefabs

First, create prefabs for the dynamic objects in the game.

**Weapon Prefabs (e.g., Pistol):**
1.  Create a new empty GameObject named `PistolPrefab`.
2.  Attach the `Pistol.cs` script to it.
3.  Adjust the public properties (`damage`, `fireRate`, etc.) in the Inspector.
4.  Drag the GameObject from the Hierarchy into your `/Prefabs/Weapons` folder to create the prefab.
5.  Repeat this process for `Shotgun.cs` and `SMG.cs`.

**Enemy Prefabs (e.g., BasicThug):**
1.  Create a new 3D object (e.g., a Capsule) and name it `BasicThugPrefab`. This will be your placeholder model.
2.  Attach the `EnemyAI.cs` script.
3.  Attach the `HitboxVisualizer.cs` script for easier debugging.
4.  Add a `Capsule Collider` component if it doesn't have one.
5.  Drag it into your `/Prefabs/Enemies` folder.
6.  Repeat this for `ShieldEnemyAI.cs` and `SniperEnemyAI.cs`, attaching their respective scripts.

### Step 3.2: Assemble a Game Level (e.g., Level 1)

Follow the blueprint in `Scenes/Level1_Downtown.unity` to construct the scene.

1.  **Create Manager GameObjects:**
    *   Create an empty GameObject named `_GameManager`. Attach the `GameManager.cs` script.
    *   Create an empty GameObject named `_UIManager`. Attach the `UIManager.cs` script.
    *   Create an empty GameObject named `_EnemySpawnManager`. Attach the `EnemySpawnManager.cs` script.
    *   Create an empty GameObject named `_AchievementSystem`. Attach the `AchievementSystem.cs` script.

2.  **Set Up the Player:**
    *   Create an empty GameObject named `PlayerRig`.
    *   Attach the `SplinePathFollower.cs` script to `PlayerRig`.
    *   Create a child Camera under `PlayerRig` and name it `MainCamera`. **Tag it as `MainCamera`**.
    *   Attach the `PlayerShooting.cs` script to the `MainCamera`.
    *   In the `PlayerShooting` component, drag the `MainCamera` itself to the `Player Camera` field.
    *   Drag your `PistolPrefab` to the `Current Weapon` field.

3.  **Define the Path:**
    *   Create an empty GameObject named `Path`.
    *   Create several empty child GameObjects under `Path` named `WP_1`, `WP_2`, etc.
    *   Position these waypoints in your scene according to the blueprint.
    *   Select the `PlayerRig` and, in the `Spline Path Follower` component, lock the Inspector and drag all the waypoint GameObjects into the `Waypoints` array.

4.  **Set Up Spawners:**
    *   Create an empty GameObject named `SpawnPoints`.
    *   Create several empty child GameObjects under it, positioned according to the blueprint.
    *   Select the `_EnemySpawnManager`. In the Inspector, configure the `Waves` array. Assign your enemy prefabs and drag the spawn point GameObjects into the `Spawn Points` array for each wave.

5.  **Build the UI:**
    *   Create a Canvas (`GameObject -> UI -> Canvas`).
    *   Add Text (TextMeshPro), Sliders, and Image elements for the HUD (Health, Score, Ammo).
    *   Select the `_UIManager` and drag these UI elements to their corresponding fields in the script component.

## 4. How to Use the Systems

### Achievement System
1.  In the Project window, right-click and go to `Create -> Achievements -> Achievement`.
2.  Create a new Achievement asset (e.g., "FirstKillAchievement").
3.  Fill in the `Achievement Key` (e.g., "FIRST_KILL"), `DisplayName`, and `Description`.
4.  Select your `_AchievementSystem` GameObject in the scene.
5.  Drag your new Achievement asset into the `Achievements` list in the Inspector.
6.  To unlock it, call `AchievementSystem.Instance.UnlockAchievement("FIRST_KILL");` from anywhere in the code (e.g., from `EnemyAI.cs` when `health <= 0`).

### Developer Console
1.  Create a UI Panel, an Input Field (TMP), and a Text (TMP) for log history as children of your main Canvas.
2.  Attach the `DeveloperConsole.cs` script to a manager object.
3.  Drag the UI Panel and Input Field to the corresponding fields on the script.
4.  During runtime, press the backtick key (`) to toggle the console.
5.  Type `help` to see a list of available commands.

---
This guide provides the necessary steps to get the game up and running. From here, you can focus on the creative aspects: level design, asset integration, and balancing. Good luck!