using System;
using Tettris.Domain.Interface.Board;
using Tettris.Domain.Interface.Tetronimo;
using UnityEngine;

namespace Tettris.Domain.Board
{
    public class Tile : ITile
    {
        public Vector2 Position { get; private set; }
        public bool Occupy { get; private set; }
        public IBaseTetromino CurrentTetromino { get; private set; }

        public Tile(Vector2 position)
        {
            Position = position;
            CurrentTetromino = null;
        }

        public bool OccupySlot(IBaseTetromino tetromino)
        {
            if (Occupy)
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
            CurrentTetromino = tetromino;
            Occupy = true;
            return true;
        }

        public void ReleaseSlot()
        {
            if (!Occupy)
            {
                return;
            }

            CurrentTetromino = null;
            Occupy = false;
        }
    }
}