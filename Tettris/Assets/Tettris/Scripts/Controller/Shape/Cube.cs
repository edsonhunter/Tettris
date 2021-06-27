using Tettris.Domain.Interface.Tetronimo;
using UnityEngine;

namespace Tettris.Controller.Shape
{
    public class Cube : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _gridPosition;
        public Vector2 GridPosition => _gridPosition;

        private IBaseTetromino BaseTetromino { get; set; }
        
        private void Update()
        {
            _gridPosition = transform.position;
        }

        public void Init(IBaseTetromino baseTetromino)
        {
            BaseTetromino = baseTetromino;
        }
    }
}