using System;
using UnityEngine;

[Serializable]
public class RoundModel
{
    [Range(.1f, 10)] public int EnemySpawnDelay;
    [Range(1, 100)] public int NumberEnemies;
    public bool IsInfinite;
    public Enemy Enemy;
}