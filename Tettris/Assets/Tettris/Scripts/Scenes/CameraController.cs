using UnityEngine;

namespace Tettris.Scenes.Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject _background;
        [SerializeField] private float _panAngle = 3f;
        [SerializeField] private float _dropAngle = 4f;
        [SerializeField] private float _returnSpeed = 10f;

        private Vector3 _currentRotation;
        private TetrisInputHandler _inputHandler;

        public void Initialize(int boardWidth, int boardHeight, TetrisInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
            if (_inputHandler != null)
            {
                _inputHandler.OnMoveLeft += PanLeft;
                _inputHandler.OnMoveRight += PanRight;
                _inputHandler.OnFastDrop += DropShake;
            }

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

        private void Update()
        {
            if (_currentRotation.sqrMagnitude > 0.001f)
            {
                _currentRotation = Vector3.Lerp(_currentRotation, Vector3.zero, Time.deltaTime * _returnSpeed);
                transform.localRotation = Quaternion.Euler(_currentRotation);
            }
            else if (transform.localRotation != Quaternion.identity)
            {
                _currentRotation = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                enabled = false;
            }
        }

        private void TriggerEffect(Vector3 rotationOffset)
        {
            _currentRotation += rotationOffset;
            enabled = true;
        }

        public void PanLeft() => TriggerEffect(new Vector3(0, -_panAngle, 0));
        public void PanRight() => TriggerEffect(new Vector3(0, _panAngle, 0));
        public void DropShake() => TriggerEffect(new Vector3(_dropAngle, 0, 0));

        private void OnDestroy()
        {
            if (_inputHandler != null)
            {
                _inputHandler.OnMoveLeft -= PanLeft;
                _inputHandler.OnMoveRight -= PanRight;
                _inputHandler.OnFastDrop -= DropShake;
            }
        }
    }
}