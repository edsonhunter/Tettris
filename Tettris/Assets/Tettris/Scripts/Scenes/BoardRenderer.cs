using UnityEngine;

namespace Tettris.Scenes.Gameplay
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class BoardRenderer : MonoBehaviour
    {
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        private int _width;
        private int _height;
        private Color32[] _colors;
        private readonly Color32 _baseColor = new Color32(25, 25, 38, 127);
        private readonly Color32 _highlightColor = new Color32(45, 45, 60, 127);
        private Mesh _mesh;
        private int[] _lastHighlightedColumns;
        private GameObject _backgroundObj;
        
        [Header("Cosmic Materials")]
        [SerializeField] private Material _cosmicBoardMaterial;
        [SerializeField] private Material _cosmicBackgroundMaterial;

        public void Initialize(int width, int height)
        {
            _width = width;
            _height = height;
            
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();

            GenerateGridMesh(width, height);
            
            if (_meshRenderer.sharedMaterial == null || _meshRenderer.sharedMaterial.name == "Default-Material" || _meshRenderer.sharedMaterial.shader.name == "Sprites/Default")
            {
                if (_cosmicBoardMaterial != null)
                {
                    _meshRenderer.material = _cosmicBoardMaterial;
                }
                else
                {
                    Debug.LogWarning("CosmicBoardMaterial is missing in BoardRenderer!");
                }
            }

            if (_backgroundObj == null)
            {
                _backgroundObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
                _backgroundObj.name = "CosmicBackground";
                _backgroundObj.transform.parent = transform;
                // Position behind the board, scaled to cover the camera
                _backgroundObj.transform.localPosition = new Vector3(width / 2f - 0.5f, height / 2f - 0.5f, 5f);
                _backgroundObj.transform.localScale = new Vector3(width * 3f, height * 3f, 1f);
                
                if (_cosmicBackgroundMaterial != null)
                {
                    Destroy(_backgroundObj.GetComponent<Collider>());
                    _backgroundObj.GetComponent<MeshRenderer>().material = _cosmicBackgroundMaterial;
                }
                else
                {
                    Debug.LogWarning("CosmicBackgroundMaterial is missing in BoardRenderer!");
                }
            }
        }

        private void GenerateGridMesh(int width, int height)
        {
            _mesh = new Mesh();
            _mesh.name = "TetrisGrid";
            _mesh.MarkDynamic();

            int numQuads = width * height;
            Vector3[] vertices = new Vector3[numQuads * 4];
            int[] triangles = new int[numQuads * 6];
            Vector2[] uvs = new Vector2[numQuads * 4];
            _colors = new Color32[numQuads * 4];

            int vIndex = 0;
            int tIndex = 0;

            float gap = 0.05f;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float left = x - 0.5f + gap;
                    float right = x + 0.5f - gap;
                    float bottom = y - 0.5f + gap;
                    float top = y + 0.5f - gap;

                    vertices[vIndex + 0] = new Vector3(left, bottom, 0); 
                    vertices[vIndex + 1] = new Vector3(left, top, 0);    
                    vertices[vIndex + 2] = new Vector3(right, top, 0);   
                    vertices[vIndex + 3] = new Vector3(right, bottom, 0);

                    uvs[vIndex + 0] = new Vector2(0, 0);
                    uvs[vIndex + 1] = new Vector2(0, 1);
                    uvs[vIndex + 2] = new Vector2(1, 1);
                    uvs[vIndex + 3] = new Vector2(1, 0);

                    _colors[vIndex + 0] = _baseColor;
                    _colors[vIndex + 1] = _baseColor;
                    _colors[vIndex + 2] = _baseColor;
                    _colors[vIndex + 3] = _baseColor;

                    triangles[tIndex + 0] = vIndex + 0;
                    triangles[tIndex + 1] = vIndex + 1;
                    triangles[tIndex + 2] = vIndex + 2;
                    triangles[tIndex + 3] = vIndex + 0;
                    triangles[tIndex + 4] = vIndex + 2;
                    triangles[tIndex + 5] = vIndex + 3;

                    vIndex += 4;
                    tIndex += 6;
                }
            }

            _mesh.vertices = vertices;
            _mesh.uv = uvs;
            _mesh.triangles = triangles;
            _mesh.colors32 = _colors;
            _mesh.RecalculateNormals();

            _meshFilter.mesh = _mesh;
            
            transform.position = new Vector3(0, 0, 0.5f);
        }

        public void HighlightColumns(System.Collections.Generic.IEnumerable<int> columns)
        {
            if (_mesh == null || _colors == null) return;

            if (_lastHighlightedColumns != null)
            {
                foreach (int col in _lastHighlightedColumns)
                {
                    if (col >= 0 && col < _width) SetColumnColor(col, _baseColor);
                }
            }

            if (columns != null)
            {
                _lastHighlightedColumns = System.Linq.Enumerable.ToArray(columns);
                foreach (int col in _lastHighlightedColumns)
                {
                    if (col >= 0 && col < _width) SetColumnColor(col, _highlightColor);
                }
            }
            else
            {
                _lastHighlightedColumns = null;
            }

            _mesh.colors32 = _colors;
        }

        private void SetColumnColor(int x, Color32 color)
        {
            for (int y = 0; y < _height; y++)
            {
                int vIndex = (y * _width + x) * 4;
                _colors[vIndex + 0] = color;
                _colors[vIndex + 1] = color;
                _colors[vIndex + 2] = color;
                _colors[vIndex + 3] = color;
            }
        }
    }
}
