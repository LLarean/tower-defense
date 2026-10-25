---
name: coop-check
description: Verify co-op in tower-defense with two local game processes — build a Windows development player, run host and guest with an autopilot over LAN or Unity Relay (join by code), compare roster, difficulty and lockstep checksums in their logs. Use after changes to networking, lobby, match, lockstep or anything that affects the simulation.
---

# Проверка кооператива двумя процессами

## Подготовка

1. Скопировать `scripts/Autopilot.cs` в `Assets/Scratch/Runtime/` (папка в `.gitignore`, но компилируется в игру; без аргументов `-td-*` автопилот ничего не делает).
2. В открытом редакторе: `script recompile`, затем `script eval_file --file <scripts/build.cs> --timeout 1500`. Сборка ляжет в `Build/Windows/BattleOfTheElements.exe` (`Build/` в `.gitignore`).
   В `Assets/Scratch` вне папки `Editor` не должно быть скриптов с `UnityEditor` — иначе сборка падает.

## Запуск (в фоне, `run_in_background`)

```bash
cd Build/Windows
./BattleOfTheElements.exe -td-host [-td-online -td-codefile <win-путь>\code.txt] -td-mission Crossroads -td-difficulty 0 -logFile <win-путь>\host.log -screen-fullscreen 0 -screen-width 800 -screen-height 450 &
sleep 3
./BattleOfTheElements.exe -td-join [-td-online -td-codefile <тот же файл>] -logFile <win-путь>\guest.log -screen-fullscreen 0 -screen-width 800 -screen-height 450 &
sleep 90; taskkill //F //IM BattleOfTheElements.exe
```

- `-td-players N` у хоста — сколько игроков ждать перед стартом (по умолчанию 2); гостей запускать столько же раз.
- Без `-td-online` — по IP `127.0.0.1:7790`. С ним — через Unity Relay: хост пишет код в файл, гость ждёт файл и входит по коду. Файл кода удалять перед запуском.
- Каждая копия входит в Unity под своим профилем (`Cloud(role)`), иначе вторая получит «player is already a member of the lobby».
- Автопилот строит по две башни у каждого игрока (точки подобраны под «Луг», на других картах часть может не встать), вызывает волны, на 900-м тике улучшает башни.

## Что сравнивать

- `[Brief]` — у гостя `players`, `difficulty`, `lives` совпадают с хостом, `me` — свой номер.
- `[Sum] tick=N sum=...` — на общих тиках суммы одинаковые (`diff` по строкам `[Sum]`); гость может отставать на пару отметок.
- `[Net]` — нет `disconnect`/`client stopped` посреди боя. `Curl error 28` у хоста — сбой связи с сервисами Unity, прогон повторить.
