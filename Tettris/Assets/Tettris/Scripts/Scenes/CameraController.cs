using UnityEngine;

namespace Tettris.Scenes.Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject _background;

        public void Initialize(int boardWidth, int boardHeight)
        {
            float centerX = (boardWidth - 1) / 2f;
            float centerY = (boardHeight - 1) / 2f;

            if (_camera != null)
            {
                _camera.transform.rotation = Quaternion.identity;

                float padding = 2f;
                float requiredHeight = (boardHeight / 2f) + padding;
                float requiredWidth = ((boardWidth / 2f) + padding) / _camera.aspect;

                if (_camera.orthographic)
                {
                    float zOffset = _camera.transform.position.z;
                    _camera.transform.position = new Vector3(centerX, centerY, zOffset);
                    _camera.orthographicSize = Mathf.Max(requiredHeight, requiredWidth);
                }
                else
                {
                    float maxRequiredDistance = Mathf.Max(requiredHeight, requiredWidth);
                    float requiredZ = -(maxRequiredDistance / Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad));
                    _camera.transform.position = new Vector3(centerX, centerY, requiredZ);
                }

                if (_background != null)
                {
                    _background.transform.position = new Vector3(centerX, centerY, 10f);
                }
            }
        }
    }
}
