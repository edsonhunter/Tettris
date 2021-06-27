using System.Collections;
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
        private ITetromino Tetromino { get; set; }

        public void Init(ITetromino tetromino)
        {
            Tetromino = tetromino;
            Tetromino.StartPosition(_cubes.Select(x => x.transform.position).ToList());
            transform.position = Tetromino.GridPosition;
            Tetromino.OnMove += TetrominoOnMove;
            Tetromino.OnRotate += TetrominoOnRotate;
        }

        private void TetrominoOnRotate(object sender, IList<Vector2> e)
        {
            transform.Rotate(0,0,90f);   
        }

        private void TetrominoOnMove(object sender, Vector3 newPos)
        {
            transform.position += newPos;
            Tetromino.SetPosition(transform.position);
        }

        public void End()
        {
            Tetromino.OnMove -= TetrominoOnMove;
            Tetromino.OnRotate -= TetrominoOnRotate;
        }

        public void ClearLine(int line)
        {
            IList<Cube> cubesToRemove = _cubes.Where(cube => line == Mathf.FloorToInt(cube.GridPosition.y)).ToList();
            for (int cubeIdx = 0; cubeIdx < cubesToRemove.Count; cubeIdx++)
            {
                _cubes.Remove(cubesToRemove[cubeIdx]);
            }
        }
    }
}