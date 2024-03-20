using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathModel
{
    public Transform SpawnPoint; 
    public DestroyPoint DestroyPoint; 
    public List<Transform> WayPoints;
}