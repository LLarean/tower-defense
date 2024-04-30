using System.Collections.Generic;
using Units;
using UnityEngine;
using Utilities;

namespace GameUtilities
{
    public class EnemiesCreator : MonoBehaviour
    {
        private List<Enemy> _enemies = new();
        
        public List<Enemy> Enemies => _enemies;

        public void Create(Enemy enemy, int numberEnemies)
        {
            ClearEnemies();
            
            for (int i = 0; i < numberEnemies; i++)
            {
                CreateEnemy(enemy);
            }
            
            CustomLogger.Log($"{_enemies.Count} opponents have been created", 2);
        }

        private void ClearEnemies()
        {
            foreach (var enemy in _enemies)
            {
                Destroy(enemy.gameObject);
            }
            
            _enemies.Clear();
        }

        private void CreateEnemy(Enemy enemy)
        {
            Enemy newEnemy = Instantiate(enemy);
            newEnemy.gameObject.SetActive(false);
            newEnemy.Initialize();
            _enemies.Add(newEnemy);
        }
    }
}