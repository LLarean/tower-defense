---
name: editor-check
description: Verify a change in the user's open Unity Editor through the Unity CLI — recompile, enter Play mode, set up a battle, click UI buttons by name, read battle state, capture the Game view. Use after any change to the Unity layer, UI or scenes when an editor is open.
---

# Проверка в открытом редакторе

Все команды: `unity command <группа> <команда> --caller plugin --skill editor-check ...` из корня проекта.

## Шаги

1. `unity status --json` — редактор в состоянии `ready`. Если не подключается, проверить `unity command console status`:
   при ошибках компиляции редактор в Safe Mode, сначала чинить код (`dotnet build tools/Stage`).
2. Перекомпиляция после правок кода: `script recompile`, затем опрашивать `script recompile_status` до `completed`.
3. `console clear`, затем нужная сцена: `scene open --path Assets/Game/Scenes/<Map>.unity`, затем `editor.playmode start`.
4. Сценарий через `script eval_file --file <путь>`; скрипты из `scripts/` копировать в scratchpad и подставлять параметры:
   - `battle.cs` — даёт золото, расставляет башни всех стихий вдоль первого маршрута, начинает бой, наводит камеру;
   - `click.cs` — нажимает кнопку UI Toolkit по `name` (замените `BUTTON`) и возвращает состояние боя;
   - `state.cs` — время, скорость, волна, монстры, жизни, золото.
5. Скриншот: `capture game_view --save_path Scratch/<имя>.png` — путь считается от `Assets/`, файл появится в `Assets/Scratch` (в `.gitignore`).
   Смотреть через Read; монстры доходят до башен примерно через 20–30 с после старта.
6. Ошибки: `console get --level error --tail 10`.
7. Уборка: `editor.playmode stop`; вернуть изменённые `PlayerPrefs`; убедиться, что `git status` не содержит мусора.

## Подводные камни

- `runtime simulate_key` не всегда попадает в `wasPressedThisFrame` — для UI надёжнее `click.cs`.
- Пути вне проекта `capture` отклоняет.
- `FindFirstObjectByType` в eval-скриптах — нормально: это инструмент проверки, а не игровой код.
- Изменения сцены сохранять явно (`EditorSceneManager.SaveScene`) и смотреть `git diff` сцены.
