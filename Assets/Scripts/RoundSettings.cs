using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoundSettings
{
    [Range(.1f, 10)] public int SpawnDelay;
    [Range(1, 100)] public int NumberEnemies;
    public List<Enemy> _enemies;
    public bool IsRandom;
}