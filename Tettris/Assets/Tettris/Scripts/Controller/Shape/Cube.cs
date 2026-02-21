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

        public void Init(IBaseTetromino baseTetromino)
        {
            BaseTetromino = baseTetromino;
            _gridPosition = BaseTetromino.GridPosition;

            BaseTetromino.OnPositionChanged += BaseTetromino_OnPositionChanged;
            BaseTetromino.OnDestroyed += BaseTetromino_OnDestroyed;
        }

        private void BaseTetromino_OnPositionChanged(object sender, Vector2 position)
        {
            _gridPosition = position;
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }

        private void BaseTetromino_OnDestroyed(object sender, System.EventArgs e)
        {
            if (BaseTetromino != null)
            {
                BaseTetromino.OnPositionChanged -= BaseTetromino_OnPositionChanged;
                BaseTetromino.OnDestroyed -= BaseTetromino_OnDestroyed;
            }
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (BaseTetromino != null)
            {
                BaseTetromino.OnPositionChanged -= BaseTetromino_OnPositionChanged;
                BaseTetromino.OnDestroyed -= BaseTetromino_OnDestroyed;
            }
        }
    }
}