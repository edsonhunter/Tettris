using System;
using UnityEngine;

namespace Tettris.Models
{
    public class TetrominoDataObj
    {
        public Guid id { get; }
        public Vector2 GridPos { get; set; }
    }
}