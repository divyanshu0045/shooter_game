# Final Steps: From Prototype to Polished Game

This document outlines the high-level phases for taking the complete codebase and turning it into a final, polished game. All work described here is done within the Unity Editor.

## Phase 1: Gameplay Balancing & Tuning

This is the most iterative phase, where you make the game fun to play.

1.  **Tune the Core Loop:** Play through your levels repeatedly. Use the public fields in the Inspector to adjust:
    *   **Weapon Stats:** Is the pistol too weak? Does the shotgun feel powerful enough? Adjust `damage`, `fireRate`, `clipSize`, and `spread` on your weapon prefabs.
    *   **Enemy Stats:** Are enemies too spongy or too weak? Adjust `health`, `damage`, and `attackWindUpTime` on your enemy prefabs.
    *   **Player Speed:** Adjust the `moveSpeed` on the `PlayerRig`'s `SplinePathFollower` component in each scene.
    *   **Scoring:** Adjust the points awarded for hits and kills in the `PlayerShooting` and `EnemyAI` scripts to make the score feel meaningful.

2.  **Adjust Wave Pacing:**
    *   In the `EnemySpawnManager` for each level, tweak the `count` and `spawnInterval` for each wave. Create moments of high intensity and brief moments of calm.

## Phase 2: Finalizing Content & Audio-Visual Polish ("Juice")

This phase is about making the game feel alive and satisfying.

1.  **Finalize VFX:** Ensure your particle effects for muzzle flashes, impacts, and explosions are impactful. Tweak their size, duration, and color.
2.  **Implement UI Animations:** Use the `comboAnimator` hook we created. Create animations for UI elements appearing, the health bar flashing when hit, etc.
3.  **Add Soundscapes:**
    *   **Background Music (BGM):** Add background music for each level.
    *   **Sound Effect Mixing:** Adjust the volume of different sound effects. Gunshots should be punchy, enemy hits should be clear, and UI sounds should be subtle.
4.  **Add Post-Processing:** Use Unity's Post-Processing Stack to add effects like bloom for neon lights, color grading to set the mood, and vignette to focus the player's view.

## Phase 3: Testing & Quality Assurance (QA)

This phase is about finding and fixing what's broken.

1.  **Hunt for Bugs:** Play the game with the intention of breaking it. Fix any errors that appear in the console.
2.  **Test on Target Devices:** Build the game and run it on one or two different Android devices. Check UI scaling and performance. Use the Unity Profiler to identify any performance bottlenecks.
3.  **Get Feedback:** Let friends or other developers play your game. Watch them play without giving instructions. Their confusion will highlight areas where the game is unclear or too difficult.

## Phase 4: Building and Deployment

This is the final step to create the distributable game file.

1.  **Configure Build Settings:**
    *   Go to `File -> Build Settings`. Ensure all your scenes are in the build order.
    *   Switch the platform to **Android**.
    *   Go to `Player Settings`. Set the `Company Name`, `Product Name`, `Version`, and `Icon`.
    *   Set your `Package Name` (e.g., `com.yourcompany.yourgame`).

2.  **Create a Keystore:** If you plan to release on the Google Play Store, you will need to create a keystore to sign your application. Follow Unity's official documentation for this process.

3.  **Build the .apk:**
    *   Back in the `Build Settings` window, click `Build`. Unity will compile your game and create an `.apk` file, which you can then install on a device.

Good luck with the final stages of development!