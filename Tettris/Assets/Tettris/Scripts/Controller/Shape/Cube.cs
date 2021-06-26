using System;
using UnityEngine;

namespace Tettris.Controller.Shape
{
    public class Cube : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _gridPosition;
        public Vector2 GridPosition => _gridPosition;

        private void Update()
        {
            _gridPosition = transform.position;
        }
    }
}