using Tettris.Domain.Interface.Tetronimo;
using UnityEngine;
using SVector2 = System.Numerics.Vector2;

namespace Tettris.Controller.Shape
{
    public class Cube : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _gridPosition;

        private IBaseTetromino BaseTetromino { get; set; }
        private System.Action<Cube> _returnToPool;
        private MeshRenderer _meshRenderer;

        [Header("Cosmic Materials")]
        [SerializeField] private Shader _cosmicPieceShader;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void Init(IBaseTetromino baseTetromino, System.Action<Cube> returnToPool)
        {
            BaseTetromino = baseTetromino;
            _returnToPool = returnToPool;
            _gridPosition = new Vector2(BaseTetromino.GridPosition.X, BaseTetromino.GridPosition.Y);

            transform.position = new Vector3(_gridPosition.x, _gridPosition.y, transform.position.z);

            BaseTetromino.OnPositionChanged += BaseTetromino_OnPositionChanged;
            BaseTetromino.OnDestroyed += BaseTetromino_OnDestroyed;
        }

        private void BaseTetromino_OnPositionChanged(object sender, SVector2 position)
        {
            _gridPosition = new Vector2(position.X, position.Y);
            transform.position = new Vector3(position.X, position.Y, transform.position.z);
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
                
                if (_cosmicPieceShader != null)
                {
                    _meshRenderer.material.shader = _cosmicPieceShader;
                    if (_meshRenderer.material.HasProperty("_Color"))
                    {
                        var col = _meshRenderer.material.color;
                        _meshRenderer.material.SetColor("_RimColor", col);
                    }
                }
                else
                {
                    Debug.LogWarning("CosmicPieceShader is missing in Cube!");
                }
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