using System.Collections.Generic;
using UnityEngine;

namespace Game.Stage.Map
{
    public sealed class Landscape : MonoBehaviour
    {
        [Header("Relief")]
        [SerializeField] private int _seed = 1;
        [SerializeField] private float _base = 2f;
        [SerializeField] private float _bumps = 1.5f;
        [SerializeField] private float _hills = 14f;
        [SerializeField] private float _rim = 25f;
        [SerializeField] private float _scale = 0.03f;
        [Header("Road")]
        [SerializeField] private float _road = 2.5f;
        [SerializeField] private float _shoulder = 4f;
        [Header("Paint")]
        [SerializeField] private TerrainLayer _ground;
        [SerializeField] private TerrainLayer _path;
        [SerializeField] private TerrainLayer _rock;
        [Header("Vegetation")]
        [SerializeField] private GameObject[] _trees = new GameObject[0];
        [SerializeField] private int _forest = 120;
        [SerializeField] private float _clearance = 14f;
        [SerializeField] private GameObject[] _grass = new GameObject[0];
        [SerializeField] private int _meadow = 6;

        public int Seed => _seed;

        public float Base => _base;

        public float Bumps => _bumps;

        public float Hills => _hills;

        public float Rim => _rim;

        public float Scale => _scale;

        public float Road => _road;

        public float Shoulder => _shoulder;

        public TerrainLayer Ground => _ground;

        public TerrainLayer Path => _path;

        public TerrainLayer Rock => _rock;

        public IReadOnlyList<GameObject> Trees => _trees;

        public int Forest => _forest;

        public float Clearance => _clearance;

        public IReadOnlyList<GameObject> Grass => _grass;

        public int Meadow => _meadow;
    }
}
