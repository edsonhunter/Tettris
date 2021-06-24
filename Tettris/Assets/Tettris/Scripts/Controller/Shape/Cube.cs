using System;
using UnityEngine;

namespace Tettris.Controller.Shape
{
    public class Cube : MonoBehaviour
    {
        [SerializeField]
        private Vector2 GridPosition;

        private void Update()
        {
            GridPosition = transform.position;
        }
    }
}