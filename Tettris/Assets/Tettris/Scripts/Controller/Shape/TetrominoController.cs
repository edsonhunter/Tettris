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

        private float FallSpeed = 5f;
        public ITetromino Tetromino { get; private set; }

        public void Init(ITetromino tetromino)
        {
            Tetromino = tetromino;
            transform.position = Tetromino.StartPosition(_cubes.Select(x => x.transform.position).ToList());
            Tetromino.OnMove += TetrominoOnMove;
            Tetromino.OnRotate += TetrominoOnRotate;

            for (int cubeIdx = 0; cubeIdx < _cubes.Count; cubeIdx++)
            {
                _cubes[cubeIdx].Init(Tetromino.BaseTetrominos[cubeIdx]);
            }
        }

        private void TetrominoOnRotate(object sender, IList<Vector2> e)
        {
            transform.Rotate(0, 0, 90f);
        }

        private void TetrominoOnMove(object sender, Vector3 newPos)
        {
            transform.position += newPos;
        }

        public void End()
        {
            Tetromino.OnMove -= TetrominoOnMove;
            Tetromino.OnRotate -= TetrominoOnRotate;
        }

        public bool ClearLine(int line)
        {
            if (!_cubes.Any(cube => cube.GridPosition.y == line))
            {
                return false;
            }

            for (int cubeIdx = _cubes.Count - 1; cubeIdx >= 0; cubeIdx--)
            {
                var cube = _cubes[cubeIdx];
                var yIndex = Mathf.FloorToInt(Mathf.Abs(cube.GridPosition.y));
                if (yIndex == line || yIndex < 0)
                {
                    _cubes.RemoveAt(cubeIdx);
                    Destroy(cube.gameObject);
                }
            }

            if (_cubes.Count <= 0)
            {
                Destroy(gameObject);
                return false;
            }

            return true;
        }

        public void RowDown(int line)
        {
            if (_cubes.Any(cube => cube.GridPosition.y > line))
            {
                transform.position += Vector3.down;
            }
        }
    }
}