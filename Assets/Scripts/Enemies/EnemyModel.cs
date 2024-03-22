using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyModel
{
    [Range(1, 1000)] public int MaximumHealth = 100;
    [HideInInspector] public Observable<int> CurrentHealth;
    
    [Range(0, 100)] public float MoveSpeed = 20f;
    [HideInInspector] public Observable<float> CurrentMoveSpeed;

    public ResistType ResistType;
    [HideInInspector] public Observable<List<DebuffModel>> DebuffModels = new List<DebuffModel>();
}