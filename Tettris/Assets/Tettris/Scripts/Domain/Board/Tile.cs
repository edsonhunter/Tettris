using System;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
using UnityEngine;

namespace Tettris.Domain.Board
{
    public class Tile : ITile
    {
        public Vector2 Position { get; private set; }
        public bool Occupied { get; private set; }
        public IBaseTetromino CurrentTetromino { get; private set; }

        public Tile(Vector2 position)
        {
            Position = position;
            CurrentTetromino = null;
        }

        public bool CheckSlot(IBaseTetromino tetromino)
        {
            if (Occupied)
            {
                if (CurrentTetromino == null)
                {
                    throw new InvalidOperationException("A occupied tile must have a tetromino");
                }
                
                if (CurrentTetromino.TetronimoId != tetromino.TetronimoId)
                {
                    return false;
                }
            }
            return true;
        }

        public bool OccupySlot(IBaseTetromino tetromino)
        {
            if (Occupied)
            {
                return false;
            }
            CurrentTetromino = tetromino;
            Occupied = true;
            return true;
        }

        public void ReleaseSlot()
        {
            if (!Occupied)
            {
                return;
            }
            CurrentTetromino = null;
            Occupied = false;
        }
    }
}