using System.Collections.Generic;
using System.Linq;
using Tettris.Domain.Interface.Tetronimo;
using UnityEngine;

namespace Tettris.Controller.Shape
{
    public class TetrominoController : MonoBehaviour
    {
        [SerializeField]
        private List<Cube> _cubes = null;
        public IList<Cube> Cubes => _cubes;

        public ITetromino Tetromino { get; private set; }

        public void Init(ITetromino tetromino)
        {
            Tetromino = tetromino;
            transform.position = Tetromino.StartPosition(_cubes.Select(x => x.transform.position).ToList());

            for (int cubeIdx = 0; cubeIdx < _cubes.Count; cubeIdx++)
            {
                var cube = _cubes[cubeIdx];
                cube.Init(Tetromino.BaseTetrominos[cubeIdx]);
                cube.transform.SetParent(null);
            }
        }

        public void End()
        {
            Destroy(gameObject);
        }
    }
}