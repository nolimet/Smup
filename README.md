# Smup

Top‑down shoot 'em up (shmup) prototype built with Unity. This repository contains the full Unity project, including
scenes, scripts, assets, and project settings.

## Overview

Shoot'em up is a simple game....

- Core language: C#
- Engine: Unity 6000.3.0f1
- Scripting runtime: .NET Framework 4.x (per typical Unity 6000 defaults)
- Package manager: Unity Package Manager (UPM)
- Notable packages: `com.cysharp.unitask` (via OpenUPM), Unity 2D packages, UGUI, Timeline
- Notable plugins (in `Assets/Plugins`): Sirenix assemblies present (Odin Inspector/Serialization)

## Requirements

- Unity Hub with Unity Editor 6000.3.0f1 (see `ProjectSettings/ProjectVersion.txt`)
- Git (to clone the repository)
- Optional: JetBrains Rider or Visual Studio for C# editing

## Getting Started (Run in Editor)

1. Clone the repository:
    - `git clone https://<your-repo-host>/Smup.git`
2. Open Unity Hub and add the project folder (`Smup`).
3. When prompted about editor version, install/use Unity `6000.3.0f1`.
4. Open the project. Allow Unity to resolve packages (UPM) on first load.
5. Open a scene to play:
    - Recommended: `Assets/Scenes/MainMenu.unity` or `Assets/Scenes/Game.unity`
6. Press Play in the Editor.

## Build

1. Open Build Settings (File → Build Settings...).
2. Add the desired scenes (ensure your entry scene is first in the list).
3. Select your target platform (e.g., PC, Mac & Linux Standalone, WebGL, etc.).
4. Click Build or Build and Run.

## Entry Points (Scenes)

Located in `Assets/Scenes/`:

- `MainMenu.unity`
- `Game.unity`

## License

This project is licensed under the BSD 3‑Clause License. See `LICENSE` for details.

