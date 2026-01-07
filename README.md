# Catch the Falling Gifts

A Unity 2D mini-game where Santa slides along the snow to catch presents. Each successful catch awards points, missed gifts cost lives, and the spawn speed ramps up over time.

## Prerequisites
- Unity 2022.3 LTS or newer
- .NET SDK installed via Unity Hub (handled automatically)

## Quick Start
1. Open Unity Hub and add this folder as an existing project.
2. Open the `Main` scene located under `Assets/Scenes` (will be created as part of the setup below).
3. Press **Play** to test in the editor.

## Controls
- **A / Left Arrow**: Move Santa left
- **D / Right Arrow**: Move Santa right
- **R**: Restart after game over

## Systems Overview
- Timed spawner rains randomly positioned gifts.
- `SantaController` handles keyboard input, horizontal bounds, and collisions.
- `Gift` objects self-manage downward velocity and notify the manager if missed.
- `GameManager` tracks score, lives, difficulty ramps, and UI updates.

## Key Scripts
- [Assets/Scripts/GameManager.cs](Assets/Scripts/GameManager.cs) boots the scene, builds runtime sprites/UI, and tracks score, lives, and restart flow.
- [Assets/Scripts/SantaController.cs](Assets/Scripts/SantaController.cs) reads keyboard input, constrains Santa to the snow strip, and raises catch events.
- [Assets/Scripts/GiftSpawner.cs](Assets/Scripts/GiftSpawner.cs) emits randomized gifts, scales spawn cadence, and clears leftover drops on restart.
- [Assets/Scripts/Gift.cs](Assets/Scripts/Gift.cs) animates the fall speed, handles acceleration, and notifies the manager when it exits bounds.

## Build / Export
Use `File → Build Settings`, ensure `Main` is in the Scenes In Build list, pick your target (Windows/Mac/Linux), and click **Build**.

## Testing Checklist
- Santa moves smoothly left/right and stays within bounds.
- Gift collisions increment score and destroy the gift.
- Gifts that reach the ground decrement lives.
- Spawn rate and fall speed increase every few catches.
- Game over UI appears at zero lives and restarts with **R**.

## Santa's Gift List Manager (WinForms)
Need a desktop helper to organize Santa's deliveries? Open [GiftListManager/README.md](GiftListManager/README.md) for full details, then run:

```bash
cd GiftListManager
dotnet run
```

Core features include add/update/remove actions, naughty/nice filtering, and a seeded dataset so the board looks alive on first launch.
