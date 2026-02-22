using System;
using System.Collections.Generic;
using Tettris.Controller.Shape;
using Tettris.Domain.Interface.Tetronimo;
using Tettris.ScriptableObject;
using UnityEngine;

namespace Tettris.Scenes.Gameplay
{
    public class CubeSpawner : MonoBehaviour
    {
        [SerializeField]
        private CubeData _cubeData;
        
        [SerializeField]
        private GameObject _startPosition;

        private Queue<Cube> _cubePool = new Queue<Cube>();

        public void SpawnTetromino(ITetromino tetromino)
        {
            Material material = _cubeData.Materials[(int)tetromino.TetrominoType];
            
            foreach (var baseTetromino in tetromino.BaseTetrominos)
            {
                Cube cube = GetCube();
                cube.transform.position = _startPosition.transform.position;
                cube.SetMaterial(material);
                cube.Init(baseTetromino, ReturnCube);
            }
        }

        private Cube GetCube()
        {
            if (_cubePool.Count > 0)
            {
                var cube = _cubePool.Dequeue();
                cube.gameObject.SetActive(true);
                return cube;
            }
            return Instantiate(_cubeData.CubePrefab, _startPosition.transform.position, Quaternion.identity)
                .GetComponent<Cube>();
        }
        
        private void ReturnCube(Cube cube)
        {
            cube.gameObject.SetActive(false);
            _cubePool.Enqueue(cube);
        }
    }
}
