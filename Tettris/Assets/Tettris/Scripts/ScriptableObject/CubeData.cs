using System.Collections.Generic;
using UnityEngine;

namespace Tettris.ScriptableObject
{
    [CreateAssetMenu(fileName = "Tetris", menuName = "Create Cube", order = 0)]
    public class CubeData : UnityEngine.ScriptableObject
    {
        [SerializeField]
        private List<Material> materials;
        public IReadOnlyList<Material> Materials => materials;

        [SerializeField]
        private GameObject cubePrefab;
        public GameObject CubePrefab => cubePrefab;
    }
}