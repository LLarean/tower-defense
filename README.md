### Battle of the Elements

A co-op tower defense for 1–4 players where towers strike with elements and elements react with each other.
Water a crowd and shock it, freeze a column and shatter it, poison a pack and set the gas on fire.

![Core](https://github.com/LLarean/tower-defense/actions/workflows/core.yml/badge.svg)
![Unity](https://github.com/LLarean/tower-defense/actions/workflows/unity.yml/badge.svg)

## Playing

| Input | Action |
|---|---|
| `1`…`7` or the bottom bar | Pick a tower to build (hover for details) |
| Left click | Build the picked tower, select a tower or a monster |
| Right click, `Esc` | Cancel; `Esc` again opens the pause menu |
| `U`, `X` | Upgrade or sell the selected tower |
| `Space` | Start the battle or call the next wave early |
| `WASD`, screen edges, wheel | Move and zoom the camera |

The campaign "The Last Architects" has three missions with a prologue, briefings and a boss siege; every mission is playable solo or in co-op on Easy, Normal or Hard.
Co-op is started from the main menu: host online and share the code, or host and join by IP on a local network.
Every player has their own gold, the base lives are shared, and the bounty for every kill is split between all players.

The game speaks English and Russian; it follows the system language and can be switched in the menu settings.
Testers get a zip with a Russian guide, the same as [docs/PLAYTEST.md](docs/PLAYTEST.md).

## Elements

Seven elements put a status on a monster; a second element on the same monster triggers a reaction.
The full table lives in [docs/DESIGN.md](docs/DESIGN.md), the numbers are tuned in `Assets/Game/Config/Chemistry.asset`.

## Project

| Path | What |
|---|---|
| `Assets/Game/Scripts/Core` | Engine-free simulation: monsters, statuses, reactions, towers, waves, economy, lockstep |
| `Assets/Game/Scripts/Stage` | Unity side: scenes, views, controls, HUD, effects, networking |
| `Assets/Game/Scripts/Editor` | Map forge that sculpts, paints and plants terrain around routes |
| `Assets/Game/Config` | Breeds, towers, chemistry, levels and effects as ScriptableObjects |
| `tools` | `dotnet` projects that build and test the code without the editor |

Multiplayer is deterministic lockstep: every peer runs the same simulation and only player orders travel over the network.
The host stamps orders with a tick, guests never run past the confirmed tick, and checksums catch any divergence.

Rules for the code are in [docs/CODESTYLE.md](docs/CODESTYLE.md), the current state and plan in [docs/STATUS.md](docs/STATUS.md).

## Developing

- Unity `6000.3.12f1`, URP.
- Core tests: `dotnet test tools/CoreTests`.
- Unity layer compile check: `dotnet build tools/Stage` and `dotnet build tools/Editor` (needs the editor installed).
- EditMode tests in Unity: Test Runner, assembly `Game.Core.Tests`.
- Online co-op needs the project linked to Unity Cloud: *Edit → Project Settings → Services*.
- CI runs core tests on every push; Unity tests and builds run once `UNITY_LICENSE`, `UNITY_EMAIL` and `UNITY_PASSWORD` secrets are set.
