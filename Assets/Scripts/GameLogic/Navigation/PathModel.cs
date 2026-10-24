using System;
using System.Collections.Generic;
using GameLogic.EnemyNavigation;
using UnityEngine;

namespace GameLogic.Navigation
{
    [Serializable]
    public class PathModel
    {
        public List<Transform> WayPoints;
    
        public Transform SpawnPoint; 
        public DestroyPoint DestroyPoint; 
    }
}