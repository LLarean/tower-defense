using System;
using UnityEngine;

[Serializable]
public class RoundModel
{
    [Range(.1f, 10)] public int SpawnDelay;
    [Range(1, 100)] public int NumberEnemies;
    public Enemy Enemy;
}