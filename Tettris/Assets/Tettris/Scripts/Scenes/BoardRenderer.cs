using UnityEngine;

namespace Tettris.Scenes.Gameplay
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class BoardRenderer : MonoBehaviour
    {
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        public void Initialize(int width, int height)
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();

            GenerateGridMesh(width, height);
            
            if (_meshRenderer.sharedMaterial == null)
            {
                Material darkMat = new Material(Shader.Find("Sprites/Default"));
                darkMat.color = new Color(0.1f, 0.1f, 0.15f, 0.5f);
                _meshRenderer.material = darkMat;
            }
        }

        private void GenerateGridMesh(int width, int height)
        {
            Mesh mesh = new Mesh();
            mesh.name = "TetrisGrid";

            int numQuads = width * height;
            Vector3[] vertices = new Vector3[numQuads * 4];
            int[] triangles = new int[numQuads * 6];
            Vector2[] uvs = new Vector2[numQuads * 4];

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

            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            _meshFilter.mesh = mesh;
            
            transform.position = new Vector3(0, 0, 0.5f);
        }
    }
}
