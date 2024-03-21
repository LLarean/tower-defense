using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyModel
{
    [Range(0, 100)] public float MoveSpeed = 20f;
    [HideInInspector] public float CurrentMoveSpeed;
    
    [Range(1, 1000)] public int MaximumHealth = 100;
    [HideInInspector] public int CurrentHealth;
    
    public ResistType ResistType;
    [HideInInspector] public List<DebuffModel> DebuffModels = new List<DebuffModel>();
}