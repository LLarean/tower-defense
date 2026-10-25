---
name: new-map
description: Create a new battle map and campaign mission for tower-defense — level asset with waves and story, scene copied from Meadow with its own terrain and routes, forged terrain, baked lighting, build settings, campaign entry. Use when asked for a new map, mission or level.
---

# Новая карта и миссия

Работает через открытый редактор (скилл `editor-check`); все шаги — `unity command script eval_file`.

## 1. Уровень

`Assets/Game/Config/Levels/<Name>.asset` — копия `Meadow.asset` (арсенал башен общий), затем через `SerializedObject`:
`_title`, `_scene` (= имя сцены), `_briefing`, `_victory`, `_gold`, `_lives`, `_rest`, `_waves[].squads[]` (`_breed`, `_count`, `_interval`, `_delay`, `_route`).
Индекс `_route` — порядок маршрутов в сцене. Пример с полным набором волн — история коммитов «Add the Crossroads map…».

## 2. Сцена

Скопировать `scripts/forge-map.cs` в scratchpad и заменить `NAME`, `SEED` и `ROUTES` (строки `new[] { new UnityEngine.Vector2(x, z), ... },` — по одной на маршрут).
Скрипт копирует сцену и террейн «Луга», расставляет маршруты, назначает уровень, меняет seed ландшафта, запускает `MapForge`, сохраняет сцену и добавляет её в Build Settings.

Геометрия: террейн 240×240 м, участки под башни — квадрат 40…199 по X и Z, край террейна поднят. Маршрут начинается и заканчивается за пределами квадрата участков.
Соседние участки дороги держать в 40–50 м друг от друга: между ними встают башни, которые достают до обоих.
Слияния делать общей точкой и общим «хвостом» у всех маршрутов.

## 3. Свет

`UnityEditor.Lightmapping.Bake()` для новой сцены, затем `SaveScene`. Без этого сцена темнее: ей не хватает окружения.

## 4. Кампания

Добавить уровень в `_missions` у `Assets/Game/Config/Campaign.asset` в нужное место. Открывается миссия после победы в предыдущей.

## 5. Проверка

Play mode на новой сцене, обзорный скриншот сверху, затем короткий бой (`editor-check/scripts/battle.cs`).
Коммиты: уровень и сцена с террейном и светом — один коммит на карту; запись в кампании — отдельным коммитом.
