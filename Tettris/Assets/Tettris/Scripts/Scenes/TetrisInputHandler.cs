using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tettris.Scenes.Gameplay
{
    public class TetrisInputHandler : MonoBehaviour
    {
        [Header("Input Settings")]
        [SerializeField] private float _minSwipeDistance = 50f;
        
        private Vector2 _touchStartPos;
        private bool _isSwiping;

        public event Action OnMoveLeft;
        public event Action OnMoveRight;
        public event Action OnRotate;
        public event Action OnFastDrop;

        private void Update()
        {
            CheckTouchscreen();
            CheckKeyboard();
        }

        private void CheckKeyboard()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard.leftArrowKey.wasPressedThisFrame) OnMoveLeft?.Invoke();
            else if (keyboard.rightArrowKey.wasPressedThisFrame) OnMoveRight?.Invoke();

            if (keyboard.downArrowKey.wasPressedThisFrame) OnFastDrop?.Invoke();
            if (keyboard.upArrowKey.wasPressedThisFrame) OnRotate?.Invoke();
        }

        private void CheckTouchscreen()
        {
            var touch = Touchscreen.current?.primaryTouch;
            if (touch == null) return;

            if (touch.press.wasPressedThisFrame)
            {
                _touchStartPos = touch.position.ReadValue();
                _isSwiping = true;
            }
            else if (touch.press.wasReleasedThisFrame && _isSwiping)
            {
                _isSwiping = false;
                Vector2 touchEndPos = touch.position.ReadValue();
                Vector2 swipeDelta = touchEndPos - _touchStartPos;

                if (swipeDelta.magnitude < _minSwipeDistance)
                {
                    OnRotate?.Invoke();
                }
                else
                {
                    if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                    {
                        if (swipeDelta.x > 0) OnMoveRight?.Invoke();
                        else OnMoveLeft?.Invoke();
                    }
                    else
                    {
                        if (swipeDelta.y < 0) OnFastDrop?.Invoke();
                    }
                }
            }
        }
    }
}
