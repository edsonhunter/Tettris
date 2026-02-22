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
        private System.Action<Cube> _returnToPool;
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void Init(IBaseTetromino baseTetromino, System.Action<Cube> returnToPool)
        {
            BaseTetromino = baseTetromino;
            _returnToPool = returnToPool;
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
            
            if (_returnToPool != null)
            {
                _returnToPool(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetMaterial(Material material)
        {
            if (_meshRenderer != null)
            {
                _meshRenderer.material = material;
            }
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